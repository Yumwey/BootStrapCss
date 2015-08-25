#region Using

using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages.Html;
using ttTVAdmin.Models;
using ttTVMS.Models;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Web;
using System.IO;
using System.Collections;
using SelectListItem = System.Web.Mvc.SelectListItem;
using SmartAdminMvc.IDAL;
using SmartAdminMvc.DAL;

#endregion

namespace ttTVAdmin.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private ServiceDeskContext db = new ServiceDeskContext();

        //调用反射机制的方法 
        private static InterfaceTicketsRepository ITickets = DALFactory.CreateTickets();
        // GET: /account/register
        public ActionResult NewTicket()
        {
            var settings = db.Settings;

            List<SelectListItem> items;
            SelectList list;
            SelectListItem item;
            string name;

            name = "Priority";
            items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = name, Value = "", Disabled = true });
            foreach (string value in ServiceDeskSettingManager.PrioritiesList)
                items.Add(new SelectListItem() { Text = value, Value = value });
            list = new SelectList(items, "Value", "Text", null, (string)"", (IEnumerable)(new string[] { "" }) as IEnumerable);
            ViewBag.PrioritiesList = list;

            name = "Ticket Type";
            //items = new List<string>();
            //items.Add(name);
            //items.AddRange(ServiceDeskSettingManager.TicketTypesList);
            //list = new SelectList(items, name, new string[] { name });
            items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = name, Value = "", Disabled = true });
            foreach (string value in ServiceDeskSettingManager.TicketTypesList)
                items.Add(new SelectListItem() { Text = value, Value = value });
            list = new SelectList(items, "Value", "Text", null, (string)"", (IEnumerable)(new string[] { "" }) as IEnumerable);
            ViewBag.TicketTypesList = list;

            name = "Category";
            //items = new List<string>();
            //items.Add(name);
            //items.AddRange(ServiceDeskSettingManager.CategoriesList);
            //list = new SelectList(items, name, new string[] { name });
            items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = name, Value = "", Disabled = true });
            foreach (string value in ServiceDeskSettingManager.CategoriesList)
                items.Add(new SelectListItem() { Text = value, Value = value });
            list = new SelectList(items, "Value", "Text", null, (string)"", (IEnumerable)(new string[] { "" }) as IEnumerable);
            ViewBag.CategoriesList = list;

            UserManager manager = UserManager.Create();
            var users = manager.Users.Where(u => u.UserName != this.User.Identity.Name).Select(u => u.UserName).OrderBy(n => n);

            name = "User";
            //items = new List<string>();
            //items.Add(name);
            //items.AddRange(users);
            //list = new SelectList(items, name, new string[] { name });
            items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = name, Value = "", Disabled = true });
            foreach (string value in users)
                items.Add(new SelectListItem() { Text = value, Value = value });
            list = new SelectList(items, "Value", "Text", null, (string)"", (IEnumerable)(new string[] { "" }) as IEnumerable);
            ViewBag.OwnerList = list;

            TicketCreationModel ticket = new TicketCreationModel();
            //device.PassCode = Guid.NewGuid().ToString();

            return View(ticket);
        }

        // POST: /account/register
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public int NewTicket(TicketCreationModel viewModel)
        //{
        //    int returnValue = -1;
        //    Ticket ticket;
        //    //ticket.TicketId
        //    // Ensure we have a valid viewModel to work with
        //    if (ModelState.IsValid)
        //    {
        //        DateTime now = DateTime.Now;
        //        string user = this.User.Identity.Name;
        //        ticket = new Ticket()
        //        {
        //            AffectsCustomer = viewModel.AffectsCustomer,
        //            Category = viewModel.Category,
        //            CreatedBy = user,
        //            CreatedDate = now,
        //            CurrentStatus = "Active",
        //            CurrentStatusDate = now,
        //            CurrentStatusSetBy = user,
        //            Details = viewModel.Details,
        //            IsHtml = false,
        //            LastUpdateBy = user,
        //            LastUpdateDate = now,
        //            Priority = viewModel.Priority,
        //            PublishedToKb = false,
        //            TagList = viewModel.TagList,
        //            Title = viewModel.Title,
        //            Type = viewModel.Type,
        //            Owner = viewModel.OtherOwner ? viewModel.Owner : user
        //        };
        //        db.Tickets.Add(ticket);

        //        db.SaveChanges();

        //        returnValue = ticket.TicketId;
        //        //return View(viewModel);
        //    }
        //    return returnValue;
        //    //return View(viewModel);

        //    //// Prepare the identity with the provided information
        //    //var user = new IdentityUser
        //    //{
        //    //    UserName = viewModel.Username ?? viewModel.Email,
        //    //    Email = viewModel.Email
        //    //};

        //    //// Try to create a user with the given identity
        //    //try
        //    //{
        //    //    var result = await _manager.CreateAsync(user, viewModel.Password);

        //    //    // If the user could not be created
        //    //    if (!result.Succeeded)
        //    //    {
        //    //        // Add all errors to the page so they can be used to display what went wrong
        //    //        AddErrors(result);

        //    //        return View(viewModel);
        //    //    }

        //    //    // If the user was able to be created we can sign it in immediately
        //    //    // Note: Consider using the email verification proces
        //    //    await SignInAsync(user, false);

        //    //    return RedirectToLocal();
        //    //}
        //    //catch (DbEntityValidationException ex)
        //    //{
        //    //    // Add all errors to the page so they can be used to display what went wrong
        //    //    AddErrors(ex);

        //    //    return View(viewModel);
        //    //}
        //}
       
        public int Create(TicketCreationModel viewmodel)
        {
            string Name = this.User.Identity.Name;
            return ITickets.Create(viewmodel,Name);
        }

        public ActionResult TicketDetails(int id)
        {
            Ticket ticket = db.Tickets.Find(id);

            if (ticket == null)
                return RedirectToAction("Index", "Home");
            else
            {
                TicketViewModel ticketModel = ticket.ToViewModel(true, true, true);

                ViewBag.TicketJson = System.Web.Helpers.Json.Encode(ticketModel);

                return View(ticketModel);
            }
        }

        // GET: home/index
        public ActionResult UnassignedTickets()
        {
            return View();
        }

        public ActionResult AssignedTickets()
        {
            return View();
        }

        public ActionResult TicketsByStatus()
        {
            return View();
        }

        public JsonResult Get(int id)
        {
            Ticket ticket = db.Tickets.Where(t => t.TicketId.Equals(id)).FirstOrDefault();

            var data = ticket.ToViewModel(true, true, true);
            //var data = new { data = ticket.ToViewModel(true, true, true) };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        public JsonResult GetUnassignedCount()
        {
            int count = db.Tickets.Where(t => t.AssignedTo == null).Count();

            return Json(count, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetByStatus(string user, string status)
        {
            IQueryable<Ticket> tickets;
            if (string.IsNullOrEmpty(user))
            {
                if (string.IsNullOrEmpty(status))
                    tickets = db.Tickets;
                else
                    tickets = db.Tickets.Where(t => status.Equals(t.CurrentStatus));
            }
            else
                tickets = db.Tickets.Where(t =>
                    string.IsNullOrEmpty(t.AssignedTo) == false && t.AssignedTo.Equals(user) &&
                    (string.IsNullOrEmpty(status) || status.Equals(t.CurrentStatus))
                    );

            var data = new { data = tickets.ToViewModels(true, true, true) };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssigned(string user)
        {
            IQueryable<Ticket> tickets;
            if (string.IsNullOrEmpty(user))
                tickets = db.Tickets.Where(t => string.IsNullOrEmpty(t.AssignedTo) == false && t.CurrentStatus != "Closed");
            else
                tickets = db.Tickets.Where(t => string.IsNullOrEmpty(t.AssignedTo) == false && t.AssignedTo.Equals(user) && t.CurrentStatus != "Closed");

            var data = new { data = tickets.ToViewModels(true, true, true) };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUnassigned()
        {
            var tickets = db.Tickets.Where(t => t.AssignedTo == null);
            var data = new { data = tickets.ToViewModels(true, true, true) };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComments(int id)
        {
            var comments = db.TicketComments.Where(c => c.TicketId == id).OrderByDescending(c => c.CommentId);
            return Json(comments.ToViewModels(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAttachments(int id)
        {
            var files = db.TicketAttachments.Where(a => a.TicketId == id).OrderByDescending(c => c.FileId);
            return Json(files.ToViewModels(), JsonRequestBehavior.AllowGet);
        }
        //定义的附件的获取和显示方法:图片
        public ActionResult GetAttachment(int id, int? size)
        {
            TicketAttachment file = db.TicketAttachments.Find(id);
            byte[] data = file.FileContents;

            if (size.HasValue)
            {
                switch (size)
                {
                    case 1:
                        data = ImageManager.ResizeImageFile(data, ImageManager.PhotoSize.Small);
                        break;
                    case 2:
                        data = ImageManager.ResizeImageFile(data, ImageManager.PhotoSize.Medium);
                        break;
                    case 3:
                        data = ImageManager.ResizeImageFile(data, ImageManager.PhotoSize.Full);
                        break;
                }
            }

            return File(data, file.FileType, file.FileName);
        }

        //文件上传的方法
        [HttpPost]
        public ActionResult AttachmentUpload()
        {
            bool isSavedSuccessfully = true;
            int count = 0;
            string msg = "";

            int ticketId = string.IsNullOrEmpty(Request.Params["ticketID"]) ?
                0 : int.Parse(Request.Params["ticketID"]);

            try
            {
                //string directoryPath = Server.MapPath("~/Content/photos");
                //if (!Directory.Exists(directoryPath))
                //    Directory.CreateDirectory(directoryPath);
                DateTime now = DateTime.Now;

                foreach (string f in Request.Files)
                {
                    //获取单独上传的文件
                    HttpPostedFileBase file = Request.Files[f];

                    if (file != null && file.ContentLength > 0)
                    {
                        TicketComment comment = new TicketComment();
                        comment.CommentedBy = User.Identity.Name;
                        comment.CommentedDate = now;
                        comment.CommentEvent = "has added an attachment";
                        //comment.CommentEvent = Resources.LocalizedText.HasAddedAnAttachment;
                        comment.IsHtml = false;
                        comment.Comment = string.Format("New file: {0}", file.FileName);
                        //comment.Comment = string.Format(Resources.LocalizedText.NewFile + ": {0}", FileUploader.FileName);
                        comment.TicketId = ticketId;
                        db.TicketComments.Add(comment);

                        //将文件转化成随机流MemoryStream上传到数据库.  数据库端字段设置为varbinary(MAX)
                        byte[] data;
                        using (Stream inputStream = file.InputStream)
                        {
                            MemoryStream memStream = inputStream as MemoryStream;
                            if (memStream == null)
                            {
                                memStream = new MemoryStream();
                                inputStream.CopyTo(memStream);
                            }
                            data = memStream.ToArray();
                        }

                        TicketAttachment attachment = new TicketAttachment();
                        attachment.TicketId = ticketId;
                        attachment.FileName = file.FileName;
                        attachment.FileSize = file.ContentLength;
                        attachment.FileType = file.ContentType;
                        attachment.FileContents = data;
                        attachment.UploadedBy = User.Identity.Name;
                        attachment.UploadedDate = now;
                        db.TicketAttachments.Add(attachment);

                        count++;
                    }
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                isSavedSuccessfully = false;
            }

            return Json(new
            {
                Result = isSavedSuccessfully,
                Count = count,
                Message = msg
            });
        }

        //添加评论

        [HttpPost]
        public string AddComment(long id, string comment)
        {
            string result = null;
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
                result = "Ticket not found.";
            else
            {
                TicketComment tcomment = new TicketComment();
                DateTime now = DateTime.Now;

                tcomment.CommentEvent = string.Format("added comment");
                tcomment.IsHtml = false;
                tcomment.CommentedBy = User.Identity.Name;
                tcomment.CommentedDate = now;
                tcomment.Comment = Server.HtmlEncode(comment).Trim(); //对数据进行编码，防止脚本注入式攻击
                tcomment.TicketId = ticket.TicketId;
                db.TicketComments.Add(tcomment);
                db.SaveChanges();
            }

            return result;
        }

        //上传文件
        [HttpPost]
        public string UpdateField(long id, string field, string ovalue, string value, string comment)
        {
            string result = null;
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
                result = "Ticket not found.";
            else
            {
                if (ovalue.Equals(value) == false)
                {
                    DateTime now = DateTime.Now;
                    string user = User.Identity.Name;

                    Type type = ticket.GetType();
                    PropertyInfo prop = type.GetProperty(field);
                    if (ticket.IsHtml)
                        prop.SetValue(ticket, value);
                    else
                        prop.SetValue(ticket, Server.HtmlEncode(value));
                    ticket.LastUpdateDate = now;
                    ticket.LastUpdateBy = user;

                    TicketComment tcomment = new TicketComment();

                    if (string.IsNullOrEmpty(ovalue))
                        tcomment.CommentEvent = string.Format("set the {0} to '{1}'", field.ToLower(), Server.HtmlEncode(value).Trim());
                    else
                        tcomment.CommentEvent = string.Format("changed the {0} from '{1}' to '{2}'", field.ToLower(), Server.HtmlEncode(ovalue).Trim(), Server.HtmlEncode(value).Trim());
                    //comment.CommentEvent = Resources.LocalizedText.EditedTheDetailsForTheTicket;

                    tcomment.IsHtml = false;
                    tcomment.CommentedBy = user;
                    tcomment.CommentedDate = now;
                    if (string.IsNullOrEmpty(comment))
                        tcomment.CommentEvent = tcomment.CommentEvent + " without comment";
                    //comment.CommentEvent = comment.CommentEvent + " " + Resources.LocalizedText.WithoutComment;
                    else
                        tcomment.Comment = Server.HtmlEncode(comment).Trim();
                    tcomment.TicketId = ticket.TicketId;
                    db.TicketComments.Add(tcomment);
                    db.SaveChanges();
                }
            }

            return result;
        }

        public ActionResult Creat()
        {
            return View();
        }
    }
}