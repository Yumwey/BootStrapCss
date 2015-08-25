using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ttTVMS.Models;
using PagedList;

namespace ttTVMS.Controllers
{
    public class LogController : Controller
    {
        public enum LogTypeEnum
        {
            TaskerVer = 301,
            Screenshot = 110,
            TaskerFile = 201,
            RunMinute = 310,
            RunChannel = 320,
            RunCheckUpdate = 330,
            RunCompleteUpdate = 331,
            CommandExecuted = 500,
        }
        private ttTVContext db = new ttTVContext();
        private ttTVLogContext logdb = new ttTVLogContext();

        [HttpPost]
        public string P(string d, string p, int t, string c)
        {
            return Store(d, p, t, c);
        }

        public string G(string d, string p, int t, string c)
        {
            return Store(d, p, t, c);
        }

        public string MyURL(string d, string p)
        {
            string url = string.Empty;

            Device device = db.Devices.FirstOrDefault(i => i.DeviceCode == d && i.PassCode == p && string.IsNullOrEmpty(i.DefaultURL) == false);
            if (device != null)
                url = device.DefaultURL;

            return url;
        }

        public string MyOfflineChannel(string d, string p)
        {
            string channel = string.Empty;

            Device device = db.Devices.FirstOrDefault(i => i.DeviceCode == d && i.PassCode == p && i.OfflineChannel.HasValue && (i.OfflineChannelValidFrom.HasValue == false || i.OfflineChannelValidFrom.Value < DateTime.Now) && (i.OfflineChannelValidTo.HasValue == false || i.OfflineChannelValidTo.Value > DateTime.Now));
            if (device != null)
                channel = device.OfflineChannel.Value.ToString();

            return channel;
        }

        // Image (get)
        public string I(string d, string p, long id)
        {
            string message = null;

            if (AuthenticateAdminDevice(d, p))
            {
                message = GetMessage(id);
            }

            return message;
        }

        // Image List (get)
        public string IL(string d, string p)
        {
            string message = string.Empty;
            List<string> messages = new List<string>();

            if (AuthenticateAdminDevice(d, p))
            {
                var items =
                    from device in db.Devices
                    join l in logdb.DeviceLogs on device.ID equals l.DeviceID into dl
                    select new
                    {
                        Device = device,
                        LastLog = dl.Where(l => l.LogType == 110).OrderByDescending(r => r.ID).Take(1).FirstOrDefault()
                    };
                foreach (var item in items)
                {
                    if (item.LastLog != null && (item.LastLog.Processed == null || item.LastLog.Processed == 900))
                    {
                        messages.Add(string.Format("{0},{1},{2}", item.Device.ID, item.Device.DeviceCode, item.LastLog.ID));
                    }
                }
                if (messages.Count > 0)
                    message = string.Join(";", messages.ToArray());
            }

            return message;
        }

        // Mark Processed Image
        public string MPI(string d, string p, long id)
        {
            string message = string.Empty;

            if (AuthenticateAdminDevice(d, p))
            {
                DeviceLog devicelog = logdb.DeviceLogs.Find(id);
                devicelog.Processed = 100;
                logdb.Entry(devicelog).State = EntityState.Modified;
                logdb.SaveChanges();
            }

            return message;
        }

        private bool AuthenticateAdminDevice(string d, string p)
        {
            bool authorize = false;
            Device device = db.Devices.FirstOrDefault(i => i.DeviceCode == d && i.PassCode == p);

            if (device != null && device.DeviceType == 1)
                authorize = true;

            return authorize;
        }

        private string GetMessage(long id)
        {
            string message = null;
            DeviceLog devicelog = logdb.DeviceLogs.Find(id);
            if (devicelog != null)
            {
                if (devicelog.Processed != null && devicelog.Processed == 900) // archived
                {
                    // try read from archived file
                    string filePath = Request.MapPath(devicelog.Message);
                    if (System.IO.File.Exists(filePath))
                        message = System.IO.File.ReadAllText(filePath);
                }
                else
                    message = devicelog.Message;
            }
            return message;
        }

        private string Store(string d, string p, int t, string c)
        {
            string result = "0";
            var headers = String.Empty;
            foreach (var key in Request.Headers.AllKeys)
                headers += key + "=" + Request.Headers[key] + Environment.NewLine;
            headers += "UserHostName=" + Request.UserHostName + Environment.NewLine;
            headers += "UserHostAddress=" + Request.UserHostAddress + Environment.NewLine;

            string user_IP;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] == null)
                user_IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            else
                user_IP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();

            headers += "ClientIP=" + user_IP + Environment.NewLine;


            DateTime now = DateTime.Now;
            DeviceLog devicelog;
            long deviceID = -1;

            Device device = db.Devices.FirstOrDefault(i => i.DeviceCode == d && i.PassCode == p);
            if (device != null)
                deviceID = device.ID;

            // Special handling for screenshot, always store as file because of its size
            if (t == (int)LogTypeEnum.Screenshot)
            {
                devicelog = new DeviceLog()
                {
                    DeviceID = deviceID,
                    DeviceCode = d,
                    LogType = t,
                    Message = "(tmp)",
                    Request = headers,
                    CreationDate = now
                };
                logdb.DeviceLogs.Add(devicelog);

                logdb.SaveChanges(); // Need to obtain devicelog.ID first

                devicelog.Message = c;
                ArchiveTextFile(devicelog);
            }
            else
            {
                devicelog = new DeviceLog()
                {
                    DeviceID = deviceID,
                    DeviceCode = d,
                    LogType = t,
                    Message = c,
                    Request = headers,
                    CreationDate = now
                };
                logdb.DeviceLogs.Add(devicelog);

                logdb.SaveChanges();
            }

            #region Record Statistics
            // Record statistics
            bool check50Run = false;
            bool parseRunIndex = false;
            if (device != null)
            {
                switch (t)
                {
                    case (int)LogTypeEnum.Screenshot:
                        device.Screenshot = c;
                        device.ScreenshotTime = now;
                        break;
                    case (int)LogTypeEnum.TaskerVer:
                        device.TaskerVersion = c;
                        device.TaskerVersionTime = now;
                        break;
                    case (int)LogTypeEnum.RunMinute:
                        #region RunMinute
                        DateTime? lastRunMinute = device.LastRunMinute;

                        device.LastRunMinute = now;

                        bool dayChanged = true;
                        bool monthChanged = true;
                        bool yearChanged = true;
                        if (lastRunMinute.HasValue)
                        {
                            dayChanged = (lastRunMinute.Value.Day != now.Day);
                            monthChanged = (lastRunMinute.Value.Month != now.Month);
                            yearChanged = (lastRunMinute.Value.Year != now.Year);
                        }

                        DateTime nowTime = new DateTime(1900, 1, 1, now.Hour, now.Minute, now.Second);

                        if (dayChanged)
                        {
                            device.YesterdayEarliestRunMinute = device.TodayEarliestRunMinute;
                            device.TodayEarliestRunMinute = now;
                            device.YesterdayLatestRunMinute = lastRunMinute;

                            if (device.ThisMonthEarliestRunMinute.HasValue == false ||
                                new DateTime(1900, 1, 1, device.ThisMonthEarliestRunMinute.Value.Hour, device.ThisMonthEarliestRunMinute.Value.Minute, device.ThisMonthEarliestRunMinute.Value.Second) > nowTime)
                                device.ThisMonthEarliestRunMinute = now;

                            if (device.ThisMonthLatestRunMinute.HasValue == false ||
                                new DateTime(1900, 1, 1, device.ThisMonthLatestRunMinute.Value.Hour, device.ThisMonthLatestRunMinute.Value.Minute, device.ThisMonthLatestRunMinute.Value.Second) < device.YesterdayLatestRunMinute)
                                device.ThisMonthLatestRunMinute = device.YesterdayLatestRunMinute;

                            if (device.TodayRunMinuteCount.HasValue)
                                device.YesterdayRunMinuteCount = device.TodayRunMinuteCount;

                            device.TodayRunMinuteCount = 1;

                            if (device.TodayShowRate.HasValue)
                            {
                                List<string> pastShowRate = new List<string>(30);
                                if (device.Last30DaysShowRate != null)
                                    pastShowRate.AddRange(device.Last30DaysShowRate.Split(new char[] { ',' }));
                                pastShowRate.Add(device.TodayShowRate.ToString());
                                while (pastShowRate.Count > 30)
                                    pastShowRate.RemoveAt(0);

                                device.Last30DaysShowRate = string.Join(",", pastShowRate.ToArray());
                            }

                            device.TodayShowRate = 100;
                        }
                        else
                        {
                            device.TodayRunMinuteCount++;

                            int expectedRunMinutes = Convert.ToInt32(Math.Floor(now.Subtract(device.TodayEarliestRunMinute.Value).TotalMinutes)); // Assume a request every 1 minutes
                            if (expectedRunMinutes == 0)
                                expectedRunMinutes = 1;
                            device.TodayShowRate = Convert.ToInt32(Math.Floor((double)(device.TodayRunMinuteCount.Value * 100 / expectedRunMinutes)));
                        }

                        if (monthChanged)
                        {
                            device.ThisMonthEarliestRunMinute = now;
                            device.ThisMonthLatestRunMinute = null;

                            if (device.ThisMonthRunMinuteCount.HasValue)
                                device.LastMonthRunMinuteCount = device.ThisMonthRunMinuteCount;

                            device.ThisMonthRunMinuteCount = 1;
                        }
                        else
                        {
                            device.ThisMonthRunMinuteCount++;
                        }

                        if (yearChanged)
                        {
                            device.ThisYearRunMinuteCount = 1;
                        }
                        else
                        {
                            device.ThisYearRunMinuteCount++;
                        }
                        #endregion RunMinute

                        parseRunIndex = true;
                        break;
                    case (int)LogTypeEnum.RunChannel:
                        device.LastRunChannelTime = now;

                        check50Run = true;
                        parseRunIndex = true;
                        break;
                    case (int)LogTypeEnum.RunCheckUpdate:
                        device.LastRunCheckUpdateTime = now;

                        parseRunIndex = true;
                        break;
                    case (int)LogTypeEnum.RunCompleteUpdate:
                        device.LastRunCompleteUpdateTime = now;

                        parseRunIndex = true;
                        break;
                    case (int)LogTypeEnum.CommandExecuted:
                        device.LastCommandExecuted = c;
                        device.LastCommandExecuteTime = now;
                        break;
                };

                #region Parse Run index
                if (parseRunIndex)
                {
                    int runIndex = -1;
                    int i = c.IndexOf("r,");
                    if (i >= 0)
                    {
                        i += 2;
                        int j = c.IndexOf(';');
                        if (j >= 0)
                            int.TryParse(c.Substring(i, j - i), out runIndex);
                        else
                            int.TryParse(c.Substring(i), out runIndex);
                    }
                    if (runIndex != -1)
                    {
                        if (check50Run && device.LastRunIndex.HasValue && runIndex == 1)
                        {
                            // Run index just reset, store history
                            List<string> pastRun = new List<string>(50);
                            if (device.Last50Run != null)
                                pastRun.AddRange(device.Last50Run.Split(new char[] { ',' }));
                            pastRun.Add(device.LastRunIndex.ToString());
                            while (pastRun.Count > 50)
                                pastRun.RemoveAt(0);

                            device.Last50Run = string.Join(",", pastRun.ToArray());
                        }
                        device.LastRunIndex = runIndex;
                        device.LastRunIndexTime = now;
                    }
                }
                #endregion

                device.LastLogTime = now;
                device.LastLogType = t;
                device.LastLogId = devicelog.ID;

                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
            }
            #endregion Record Statistics

            result = "1";

            return result;
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult Image(long id = 0)
        {
            DeviceLog devicelog = logdb.DeviceLogs.Find(id);
            if (devicelog == null)
            {
                return HttpNotFound();
            }
            byte[] data = System.Convert.FromBase64String(devicelog.Message);
            string filePath = Request.MapPath("~/Content/tmp.png");
            System.IO.File.WriteAllBytes(filePath, data);

            string testPathSource = Request.MapPath("~/Content/test.png");
            string testPathTarge = Request.MapPath("~/Content/test.png.txt");
            //byte[] testContent = System.IO.File.ReadAllBytes(testPathSource);
            //System.IO.File.WriteAllText(testPathTarge, System.Convert.ToBase64String(testContent));
            string testContent = System.IO.File.ReadAllText(testPathTarge);
            Base64Decoder decoder = new Base64Decoder(testContent.ToCharArray());
            System.IO.File.WriteAllBytes(testPathSource, decoder.GetDecoded());
            //System.IO.File.WriteAllText(testPathTarge, System.Convert.ToBase64String(testContent));

            return File(filePath, "image/png");
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult TextFile(long id = 0)
        {
            return SaveTextFile(id, "~/Content/Tmp.txt");
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult TaskerDataFile(long id = 0)
        {
            return SaveTextFile(id, "~/App_Data/Tasker.xml");
        }

        private ActionResult SaveTextFile(long id = 0, string file = null)
        {
            DeviceLog devicelog = logdb.DeviceLogs.Find(id);
            if (devicelog == null)
            {
                return HttpNotFound();
            }

            string decoded = Server.UrlDecode(devicelog.Message);
            string testPathSource = Request.MapPath(file);
            System.IO.File.WriteAllText(testPathSource, decoded);

            return File(testPathSource, "Text/plain");
        }

        [Authorize(Roles = "Administrators")]
        public string BatchArchive(int daysOld = 10, int logType = 110)
        {
            DateTime before10Days = DateTime.Now.Subtract(new TimeSpan(daysOld, 0, 0, 0));
            var deviceLogIds = logdb.DeviceLogs.Where(d => d.CreationDate < before10Days && d.LogType == logType && (d.Processed == null || d.Processed != 900)).Select(u => u.ID).ToList();
            foreach (var deviceLogId in deviceLogIds)
            {
                ArchiveTextFile(deviceLogId);
            }

            return string.Join(", ", deviceLogIds.ToArray());
        }

        private void ArchiveTextFile(long id = 0)
        {
            DeviceLog devicelog = logdb.DeviceLogs.Find(id);
            if (devicelog != null)
            {
                ArchiveTextFile(devicelog);
            }
        }

        private void ArchiveTextFile(DeviceLog devicelog)
        {
            string decoded = Server.UrlDecode(devicelog.Message);

            string file = string.Format("~/App_Data/Archive/{0}-{1}-{2}/{3}/{4}-{5}.txt", devicelog.CreationDate.Year, devicelog.CreationDate.Month, devicelog.CreationDate.Day, devicelog.CreationDate.Hour, devicelog.DeviceID, devicelog.ID);
            string filePath = Request.MapPath(file);

            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));
            System.IO.File.WriteAllText(filePath, decoded);

            devicelog.Message = file;
            devicelog.Processed = 900; // Archived

            logdb.Entry(devicelog).State = EntityState.Modified;
            logdb.SaveChanges();
        }

        //
        // GET: /Log/

        [Authorize(Roles = "Administrators")]
        public ActionResult Index(long? devID, int? page, int? type)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            IQueryable<DeviceLog> devicelogs;
            //if (devID.HasValue)
            devicelogs = logdb.DeviceLogs.Where(l => (devID.HasValue == false || l.DeviceID == devID.Value) && (type.HasValue == false || type.Value == l.LogType)).OrderByDescending(l => l.ID); //.Include(d => d.Device);
            //else
            //    devicelogs = logdb.DeviceLogs; //.Include(d => d.Device);

            //devicelogs = devicelogs.OrderByDescending(l => l.ID);

            ViewBag.DevID = devID;
            ViewBag.LogType = type;

            return View(devicelogs.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Log/Details/5

        [Authorize(Roles = "Administrators")]
        public ActionResult Details(long id = 0)
        {
            DeviceLog devicelog = logdb.DeviceLogs.Find(id);
            if (devicelog == null)
            {
                return HttpNotFound();
            }
            return View(devicelog);
        }

        // GET: /Log/Archive/5

        [Authorize(Roles = "Administrators")]
        public ActionResult Archive(long id = 0)
        {
            ArchiveTextFile(id);

            return RedirectToAction("Index");
        }

        ////
        //// GET: /Log/Create

        //public ActionResult Create()
        //{
        //    ViewBag.DeviceID = new SelectList(db.Devices, "ID", "DeviceID");
        //    return View();
        //}

        ////
        //// POST: /Log/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(DeviceLog devicelog)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.DeviceLogs.Add(devicelog);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.DeviceID = new SelectList(db.Devices, "ID", "DeviceID", devicelog.DeviceID);
        //    return View(devicelog);
        //}

        ////
        //// GET: /Log/Edit/5

        //public ActionResult Edit(long id = 0)
        //{
        //    DeviceLog devicelog = db.DeviceLogs.Find(id);
        //    if (devicelog == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.DeviceID = new SelectList(db.Devices, "ID", "DeviceID", devicelog.DeviceID);
        //    return View(devicelog);
        //}

        ////
        //// POST: /Log/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(DeviceLog devicelog)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(devicelog).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.DeviceID = new SelectList(db.Devices, "ID", "DeviceID", devicelog.DeviceID);
        //    return View(devicelog);
        //}

        //
        // GET: /Log/Delete/5

        [Authorize(Roles = "Administrators")]
        public ActionResult Delete(long id = 0)
        {
            DeviceLog devicelog = logdb.DeviceLogs.Find(id);
            if (devicelog == null)
            {
                return HttpNotFound();
            }
            return View(devicelog);
        }

        //
        // POST: /Log/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrators")]
        public ActionResult DeleteConfirmed(long id)
        {
            DeviceLog devicelog = logdb.DeviceLogs.Find(id);
            logdb.DeviceLogs.Remove(devicelog);
            logdb.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            logdb.Dispose();
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// Summary description for Base64Decoder.
    /// </summary>
    public class Base64Decoder
    {
        char[] source;
        int length, length2, length3;
        int blockCount;
        int paddingCount;
        public Base64Decoder(char[] input)
        {
            int temp = 0;
            source = input;
            length = input.Length;

            //find how many padding are there
            for (int x = 0; x < 2; x++)
            {
                if (input[length - x - 1] == '=')
                    temp++;
            }
            paddingCount = temp;
            //calculate the blockCount;
            //assuming all whitespace and carriage returns/newline were removed.
            blockCount = length / 4;
            length2 = blockCount * 3;
        }

        public byte[] GetDecoded()
        {
            byte[] buffer = new byte[length];//first conversion result
            byte[] buffer2 = new byte[length2];//decoded array with padding

            for (int x = 0; x < length; x++)
            {
                buffer[x] = char2sixbit(source[x]);
            }

            byte b, b1, b2, b3;
            byte temp1, temp2, temp3, temp4;

            for (int x = 0; x < blockCount; x++)
            {
                temp1 = buffer[x * 4];
                temp2 = buffer[x * 4 + 1];
                temp3 = buffer[x * 4 + 2];
                temp4 = buffer[x * 4 + 3];

                b = (byte)(temp1 << 2);
                b1 = (byte)((temp2 & 48) >> 4);
                b1 += b;

                b = (byte)((temp2 & 15) << 4);
                b2 = (byte)((temp3 & 60) >> 2);
                b2 += b;

                b = (byte)((temp3 & 3) << 6);
                b3 = temp4;
                b3 += b;

                buffer2[x * 3] = b1;
                buffer2[x * 3 + 1] = b2;
                buffer2[x * 3 + 2] = b3;
            }
            //remove paddings
            length3 = length2 - paddingCount;
            byte[] result = new byte[length3];

            for (int x = 0; x < length3; x++)
            {
                result[x] = buffer2[x];
            }

            return result;
        }

        private byte char2sixbit(char c)
        {
            char[] lookupTable = new char[64]
          {  

    'A','B','C','D','E','F','G','H','I','J','K','L','M','N',
    'O','P','Q','R','S','T','U','V','W','X','Y', 'Z',
    'a','b','c','d','e','f','g','h','i','j','k','l','m','n',
    'o','p','q','r','s','t','u','v','w','x','y','z',
    '0','1','2','3','4','5','6','7','8','9','+','/'};
            if (c == '=')
                return 0;
            else
            {
                for (int x = 0; x < 64; x++)
                {
                    if (lookupTable[x] == c)
                        return (byte)x;
                }
                //should not reach here
                return 0;
            }

        }

    }
}