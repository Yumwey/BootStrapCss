using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ttTVAdmin
{
    public enum SettingChangeResult
    {
        Success,
        Failure,
        Merge
    }

    public static class ServiceDeskSettingManager
    {
        private static ServiceDeskContext db = new ServiceDeskContext();
        private const string SETTINGSEPERATOR = ",";

        #region Enum Properties
        private static Dictionary<string, string[]> cache = new Dictionary<string, string[]>();
        public static string[] GetSettings(string name, string[] defaults = null)
        {
            string[] values;
            if (cache.ContainsKey(name))
                values = cache[name];
            else
            {
                values = GetStringEnumFromDb(name);
                if (values == null)
                {
                    //priorities = CreateDefaultPriorities();
                    values = defaults;
                    SaveStringEnumToDb(name, values);
                }
                cache.Add(name, values);
            }
            return values;
        }
        public static void SetSettings(string name, string[] values)
        {
            cache[name] = values;
            SaveStringEnumToDb(name, values);
        }

        private static string[] priorities;
        public static string[] PrioritiesList
        {
            get
            {
                if (priorities == null)
                {
                    priorities = GetStringEnumFromDb("PriorityList");
                    if (priorities == null)
                    {
                        priorities = CreateDefaultPriorities();
                        SaveStringEnumToDb("PriorityList", priorities);
                    }
                }
                return priorities;
            }
            set
            {
                priorities = value;
                SaveStringEnumToDb("PriorityList", priorities);
            }
        }

        private static string[] categories;
        public static string[] CategoriesList
        {
            get
            {
                if (categories == null)
                {
                    categories = GetStringEnumFromDb("CategoryList");
                    if (categories == null)
                    {
                        categories = CreateDefaultCategories();
                        SaveStringEnumToDb("CategoryList", categories);
                    }
                }
                return categories;
            }
            set
            {
                categories = value;
                SaveStringEnumToDb("CategoryList", categories);
            }
        }

        private static string[] ticketTypes;
        public static string[] TicketTypesList
        {
            get
            {
                if (ticketTypes == null)
                {
                    ticketTypes = GetStringEnumFromDb("TicketTypesList");
                    if (ticketTypes == null)
                    {
                        ticketTypes = CreateDefaultTicketTypes();
                        SaveStringEnumToDb("TicketTypesList", ticketTypes);
                    }
                }
                return ticketTypes;
            }
            set
            {
                ticketTypes = value;
                SaveStringEnumToDb("TicketTypesList", ticketTypes);
            }
        }

        #endregion

        #region Enums Defaults

        private static string[] CreateDefaultPriorities()
        {
            return new string[] { "Low", "Medium", "High" };
        }

        private static string[] CreateDefaultTicketTypes()
        {
            return new string[] { "Question", "Problem", "Request" };
        }

        private static string[] CreateDefaultCategories()
        {
            return new string[] { "Hardware", "Software", "Network/Services", "Non-Technical" };
        }

        #endregion

        #region Enum Rename Methods
        public static SettingChangeResult RenameValue(string name, string oldValueName, string newValueName)
        {
            SettingChangeResult results = SettingChangeResult.Failure;
            string[] newSettings = RenameStringEmunSetting(name, oldValueName, newValueName, GetSettings(name), false, out results);
            //SaveRenamedValue(name, newSettings, oldValueName, newValueName, false);
            db.SaveChanges();
            cache[name] = newSettings;
            return results;
        }

        public static SettingChangeResult RenamePriority(string oldPriorityName, string newPriorityName)
        {
            SettingChangeResult results = SettingChangeResult.Failure;
            string[] newSettings = RenameStringEmunSetting("PriorityList", oldPriorityName, newPriorityName, PrioritiesList, false, out results);
            SaveRenamedPriority(newSettings, oldPriorityName, newPriorityName, false);
            db.SaveChanges();
            priorities = newSettings;
            return results;
        }

        public static SettingChangeResult RenameCategory(string oldCategoryName, string newCategoryName)
        {
            SettingChangeResult results = SettingChangeResult.Failure;
            string[] newSettings = RenameStringEmunSetting("CategoryList", oldCategoryName, newCategoryName, CategoriesList, false, out results);
            SaveRenamedCategory(newSettings, oldCategoryName, newCategoryName, false);
            db.SaveChanges();
            categories = newSettings;
            return results;
        }

        public static SettingChangeResult RenameTicketType(string oldTypeName, string newTypeName)
        {
            SettingChangeResult results = SettingChangeResult.Failure;
            string[] newSettings = RenameStringEmunSetting("TicketTypesList", oldTypeName, newTypeName, TicketTypesList, false, out results);
            SaveRenamedTicketTypes(newSettings, oldTypeName, newTypeName, false);
            db.SaveChanges();
            ticketTypes = newSettings;
            return results;
        }

        //private static void SaveRenamedValue(string name, string[] newSettings, string oldValueName, string newValueName, bool commitChanges)
        //{
        //    DateTime now = DateTime.Now;

        //    string user = HttpContext.Current.User.Identity.Name;
        //    string evt = string.Format("renamed the ticket {0} from {1} to {2} globally.", name, oldValueName, newValueName);

        //    var tickets = db.Tickets.Where(t => t.Value == oldValueName);
        //    foreach (Ticket ticket in tickets)
        //    {
        //        ticket.Value = newValueName;
        //        TicketComment comment = new TicketComment();
        //        comment.CommentedBy = user;
        //        comment.CommentedDate = now;
        //        comment.IsHtml = false;
        //        comment.CommentedBy = user;
        //        comment.CommentEvent = evt;
        //        comment.TicketId = ticket.TicketId;
        //        db.TicketComments.Add(comment);
        //    }
        //    if (commitChanges)
        //    {
        //        db.SaveChanges();
        //    }
        //}

        private static void SaveRenamedPriority(string[] newSettings, string oldPriorityName, string newPriorityName, bool commitChanges)
        {
            DateTime now = DateTime.Now;

            string user = HttpContext.Current.User.Identity.Name;
            string evt = string.Format("renamed the ticket priority from {0} to {1} globally.", oldPriorityName, newPriorityName);

            var tickets = db.Tickets.Where(t => t.Priority == oldPriorityName);
            foreach (Ticket ticket in tickets)
            {
                ticket.Priority = newPriorityName;
                TicketComment comment = new TicketComment();
                comment.CommentedBy = user;
                comment.CommentedDate = now;
                comment.IsHtml = false;
                comment.CommentedBy = user;
                comment.CommentEvent = evt;
                comment.TicketId = ticket.TicketId;
                db.TicketComments.Add(comment);
            }
            if (commitChanges)
            {
                db.SaveChanges();
            }
        }

        private static void SaveRenamedCategory(string[] newSettings, string oldCategoryName, string newCategoryName, bool commitChanges)
        {
            DateTime now = DateTime.Now;

            string user = HttpContext.Current.User.Identity.Name;
            string evt = string.Format("renamed the ticket category from {0} to {1} globally.", oldCategoryName, newCategoryName);
            var tickets = db.Tickets.Where(t => t.Category == oldCategoryName);
            foreach (Ticket ticket in tickets)
            {
                ticket.Category = newCategoryName;
                TicketComment comment = new TicketComment();
                comment.CommentedBy = user;
                comment.CommentedDate = now;
                comment.IsHtml = false;
                comment.CommentedBy = user;
                comment.CommentEvent = evt;
                comment.TicketId = ticket.TicketId;
                db.TicketComments.Add(comment);
            }
            if (commitChanges)
            {
                db.SaveChanges();
            }
        }

        private static void SaveRenamedTicketTypes(string[] newSettings, string oldTypeName, string newTypeName, bool commitChanges)
        {
            DateTime now = DateTime.Now;

            string user = HttpContext.Current.User.Identity.Name;
            string evt = string.Format("renamed the ticket type from {0} to {1} globally.", oldTypeName, newTypeName);
            var tickets = db.Tickets.Where(t => t.Type == oldTypeName);
            foreach (Ticket ticket in tickets)
            {
                ticket.Type = newTypeName;
                TicketComment comment = new TicketComment();
                comment.CommentedBy = user;
                comment.CommentedDate = now;
                comment.IsHtml = false;
                comment.CommentedBy = user;
                comment.CommentEvent = evt;
                comment.TicketId = ticket.TicketId;
                db.TicketComments.Add(comment);
            }
            if (commitChanges)
            {
                db.SaveChanges();
            }
        }
        #endregion

        #region Enum shared methods

        private static string[] GetStringEnumFromDb(string settingName)
        {
            string[] values = null;
            string p = (from settings in db.Settings
                        where settings.SettingName == settingName
                        select settings.SettingValue).SingleOrDefault();
            if (!string.IsNullOrEmpty(p))
            {
                values = p.Split(new string[] { SETTINGSEPERATOR }, StringSplitOptions.RemoveEmptyEntries);
            }
            return values;
        }

        private static void SaveStringEnumToDb(string settingName, string[] items)
        {
            SaveStringEnumToDb(settingName, items, true);
        }

        private static void SaveStringEnumToDb(string settingName, string[] items, bool commitChanges)
        {
            Setting setting = db.Settings.SingleOrDefault(s => s.SettingName == settingName);
            if (setting == null)
            {
                setting = new Setting();
                setting.SettingName = settingName;
                db.Settings.Add(setting);
            }
            setting.SettingValue = string.Join(SETTINGSEPERATOR, items);

            if (commitChanges)
            {
                db.SaveChanges();
            }
        }

        internal static string[] RenameStringEmunSetting(string settingName, string oldSetting, string newSetting, string[] origionalSettings, bool commitChanges, out SettingChangeResult results)
        {
            string[] newSettings = null;
            results = SettingChangeResult.Failure;
            if (oldSetting != newSetting)
            {
                int newLen = origionalSettings.Length;
                bool deleteOldSetting = false;
                //if new setting is same as an existing setting in list, we have to remove the old setting rather than rename it
                if (origionalSettings.Contains(newSetting))
                {
                    deleteOldSetting = true;
                    newLen--;
                }

                List<string> newSettingssList = new List<string>(newLen);
                for (int i = 0; i < origionalSettings.Length; i++)
                {
                    if (origionalSettings[i] == oldSetting)
                    {
                        if (!deleteOldSetting)
                        {
                            newSettingssList.Add(newSetting);
                        }
                    }
                    else
                    {
                        newSettingssList.Add(origionalSettings[i]);
                    }
                }
                newSettings = newSettingssList.ToArray();
                SaveStringEnumToDb(settingName, newSettings, false);

                results = (deleteOldSetting) ? SettingChangeResult.Merge : SettingChangeResult.Success;
                if (commitChanges)
                {
                    db.SaveChanges();
                }
            }
            return newSettings;
        }

        #endregion
    }

}