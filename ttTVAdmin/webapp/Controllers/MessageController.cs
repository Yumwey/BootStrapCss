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
    public class MessageController : Controller
    {
        private enum MessageContants
        {
            ActivateQuickSupport,
            ClearCommandQ,
            ClearMessageQ,
            ClearOfflineFiles,
            GetDefaultURL,
            GetDefaultChannel,
            GetOfflineChannel,
            GetOfflinePack,
            GetOpenDefaultURL,
            KillFirefox,
            OpenDefaultURL,
            OpenOffline,
            OpenURL,
            RebootDevice,
            ReportCommandQ,
            ReportCommandRun,
            ReportDefaults,
            ReportOfflineVersion,
            ReportMessageQ,
            ReporTaskerVersion,
            RestoreTasker,
            ShowDeviceID,
            ShowOnline,
            UpdateOfflineChannel,
            UpdateOfflinePack,
            UpdateTasker,
            UpdateStandardWallpaper,
            UploadScreenshot,
            

            UploadTaskerDataFile,
            DecodeImage,
            RestartNetwork
        };

        private ttTVContext db = new ttTVContext();

        //
        // GET: /Message/G/5

        public string G(string id, string p, string r)
        {
            Request request = new Models.Request();
            //List<RequestMessage> reqMessages = new List<RequestMessage>();
            DateTime now = DateTime.Now;
            List<string> parts = new List<string>();
            string content = null;
            long foundDeviceID = 0;
            Device device = null;

            long requestIndex;
            if (string.IsNullOrEmpty(r) == false && long.TryParse(r, out requestIndex))
                request.RequestIndex = requestIndex;

            if (string.IsNullOrEmpty(id) == false && string.IsNullOrEmpty(p) == false)
            {
                device = db.Devices.FirstOrDefault(d => d.DeviceCode == id && d.PassCode == p);
                if (device != null)
                {
                    foundDeviceID = device.ID;

                    List<Message> messages = db.Messages.Where(m => m.DeviceID == device.ID && m.Status == 1 && m.ValidFrom <= now && m.ValidTo >= now && (m.RequestIndex.HasValue == false || (request.RequestIndex.HasValue && request.RequestIndex.Value >= (m.RequestIndex.Value - (m.RequestIndexRange.HasValue? m.RequestIndexRange.Value : 5)) && request.RequestIndex.Value <= (m.RequestIndex.Value + (m.RequestIndexRange.HasValue? m.RequestIndexRange.Value : 5))))).ToList();
                    request.RequestMessages = new List<RequestMessage>(messages.Count);
                    foreach (Message message in messages)
                    {
                        parts.Add(message.MessageContent);

                        request.RequestMessages.Add(new RequestMessage()
                        {
                            MessageID = message.ID,
                            MessageContent = message.MessageContent
                        });

                        if (message.MessageType == 1) // one-time only
                        {
                            // move to archive
                            MessageArchive archive = new MessageArchive()
                            {
                                DeviceID = message.DeviceID,
                                ExtDate1 = message.ExtDate1,
                                ExtDate2 = message.ExtDate2,
                                ExtDate3 = message.ExtDate3,
                                ExtDec1 = message.ExtDec1,
                                ExtDec2 = message.ExtDec2,
                                ExtDec3 = message.ExtDec3,
                                ExtString1 = message.ExtString1,
                                ExtString2 = message.ExtString2,
                                ExtString3 = message.ExtString3,
                                MessageContent = message.MessageContent,
                                MessageID = message.ID,
                                MessageType = message.MessageType,
                                Remarks = message.Remarks,
                                Status = message.Status,
                                ValidFrom = message.ValidFrom,
                                ValidTo = message.ValidTo,
                                CreationDate = DateTime.Now,
                                RequestIndex = message.RequestIndex
                            };
                            db.MessageArchives.Add(archive);

                            // delete from Messages db table
                            db.Messages.Remove(message);
                        }
                    }

                    content = string.Join(",", parts.ToArray());
                }
            }
            
            request.DeviceID = foundDeviceID;
            if (foundDeviceID == 0)
                request.Remarks = id;
            request.Message = content;
            request.RequestDate = now;
            db.Requests.Add(request);

            db.SaveChanges();

            #region Record Statistics
            if (device != null)
            {
                // Record statistics
                DateTime? lastRequestTime = device.LastRequestTime;

                device.LastRequestId = request.ID;
                device.LastRequestMessage = request.Message;
                device.LastRequestTime = request.RequestDate;
                if(string.IsNullOrEmpty(content) == false)
                {
                    device.LastCommandIssued = request.Message;
                    device.LastCommandIssueTime = request.RequestDate;
                }

                bool dayChanged = true;
                bool monthChanged = true;
                bool yearChanged = true;
                if(lastRequestTime.HasValue)
                {
                    dayChanged = (lastRequestTime.Value.Day != now.Day);
                    monthChanged = (lastRequestTime.Value.Month != now.Month);
                    yearChanged = (lastRequestTime.Value.Year != now.Year);
                }

                DateTime nowTime = new DateTime(1900, 1, 1, now.Hour, now.Minute, now.Second);

                if(dayChanged)
                {
                    device.YesterdayEarliestRequestTime = device.TodayEarliestRequestTime;
                    device.TodayEarliestRequestTime = now;
                    device.YesterdayLatestRequestTime = lastRequestTime;

                    if (device.ThisMonthEarliestRequestTime.HasValue == false || 
                        new DateTime(1900, 1, 1, device.ThisMonthEarliestRequestTime.Value.Hour, device.ThisMonthEarliestRequestTime.Value.Minute, device.ThisMonthEarliestRequestTime.Value.Second) > nowTime)
                        device.ThisMonthEarliestRequestTime = now;

                    if (device.ThisMonthLatestRequestTime.HasValue == false ||
                        new DateTime(1900, 1, 1, device.ThisMonthLatestRequestTime.Value.Hour, device.ThisMonthLatestRequestTime.Value.Minute, device.ThisMonthLatestRequestTime.Value.Second) < device.YesterdayLatestRequestTime)
                        device.ThisMonthLatestRequestTime = device.YesterdayLatestRequestTime;

                    if (device.TodayRequestCount.HasValue)
                        device.YesterdayRequestCount = device.TodayRequestCount;

                    device.TodayRequestCount = 1;

                    if(device.TodayNetworkConnectivity.HasValue)
                    {
                        List<string> pastNetworkConnectivity = new List<string>(30);
                        if (device.Last30DaysNetworkConnectivity != null)
                            pastNetworkConnectivity.AddRange(device.Last30DaysNetworkConnectivity.Split(new char[] { ',' }));
                        pastNetworkConnectivity.Add(device.TodayNetworkConnectivity.ToString());
                        while (pastNetworkConnectivity.Count > 30)
                            pastNetworkConnectivity.RemoveAt(0);

                        device.Last30DaysNetworkConnectivity = string.Join(",", pastNetworkConnectivity.ToArray());
                    }

                    device.TodayNetworkConnectivity = 100;
                }
                else
                {
                    device.TodayRequestCount++;

                    int expectedRequests = Convert.ToInt32(Math.Floor(now.Subtract(device.TodayEarliestRequestTime.Value).TotalMinutes / 2)); // Assume a request every 2 minutes
                    if (expectedRequests == 0)
                        expectedRequests = 1;
                    device.TodayNetworkConnectivity = Convert.ToInt32(Math.Floor((double)(device.TodayRequestCount.Value * 100 / expectedRequests)));
                }

                if(monthChanged)
                {
                    device.ThisMonthEarliestRequestTime = now;
                    device.ThisMonthLatestRequestTime = null;

                    if (device.ThisMonthRequestCount.HasValue)
                        device.LastMonthRequestCount = device.ThisMonthRequestCount;

                    device.ThisMonthRequestCount = 1;
                }
                else
                {
                    device.ThisMonthRequestCount++;
                }

                if (yearChanged)
                {
                    device.ThisYearRequestCount = 1;
                }
                else
                {
                    device.ThisYearRequestCount++;
                }

                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
            }
            #endregion Record Statistics

            return content;
        }

        //
        // GET: /Message/

        [Authorize(Roles = "Administrators")]
        public ActionResult Index(int? page)
        {
            ttTVMS.Models.MessageViewModel viewModel = new MessageViewModel();
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            var messages = db.Messages.OrderByDescending(r => r.ID);

            List<SelectListItem> selectItems = new List<SelectListItem>();
            Array msgConstants = Enum.GetValues(typeof(MessageContants));
            Array.Sort(msgConstants);
            foreach (int msg in msgConstants)
            {
                selectItems.Add(new SelectListItem() { Text = Enum.GetName(typeof(MessageContants), msg), Value = msg.ToString() });
            }

            ViewBag.Functions = new SelectList(selectItems.ToArray(), "Value", "Text");

            var model = new DeviceSelectionViewModel();
            var items =
                from d in db.Devices
                    select d;
                //join r in db.Requests on d.ID equals r.DeviceID into dr
                //join l in db.DeviceLogs on d.ID equals l.DeviceID into dl
                //select new
                //{
                //    Device = d,
                //    LastRequest = dr.OrderByDescending(r => r.ID).Take(1).FirstOrDefault(),
                //    LastLog = dl.OrderByDescending(r => r.ID).Take(1).FirstOrDefault()
                //};

            foreach (var item in items) // (var device in db.Devices.Include(d => d.Requests.Where(dw => DateTime.Now.Subtract(dw.RequestDate).TotalDays ).Include(d => d.DeviceLogs))
            {
                var editorViewModel = new SelectDeviceEditorViewModel()
                {
                    ID = item.ID, // item.Device.ID,
                    AndriodVer = item.AndriodVer, // .Device.AndriodVer,
                    CreationDate = item.CreationDate, // .Device.CreationDate,
                    CustomerID = item.CustomerID, // .Device.CustomerID,
                    DeviceCode = item.DeviceCode, // .Device.DeviceCode,
                    DeviceType = item.DeviceType, // .Device.DeviceType,
                    ModelNo = item.ModelNo, // .Device.ModelNo,
                    LastRequestDate = item.LastRequestTime, // item.LastRequest == null ? (DateTime?)null : item.LastRequest.RequestDate,
                    LastRequestID = item.LastRequestId, // item.LastRequest == null ? (long?)null : item.LastRequest.ID,
                    LastLogDate = item.LastLogTime, // item.LastLog == null ? (DateTime?)null : item.LastLog.CreationDate,
                    LastLogID = item.LastLogId, // item.LastLog == null ? (long?)null : item.LastLog.ID,

                    Selected = item.DeviceType != 1
                };
                model.Devices.Add(editorViewModel);
            }
            viewModel.Devices = model;
            viewModel.Messages = messages.ToPagedList(pageNumber, pageSize);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Send(MessageViewModel model)
        {
            // get the ids of the items selected:
            List<long> selectedIds = model.Devices.getSelectedIds();

            // Use the ids to retrieve the records for the selected people
            // from the database:
            var selectedDevices = from x in db.Devices
                                  where selectedIds.Contains(x.ID)
                                  select x;

            string message = null;
            switch (model.Devices.SelectedFunction)
            {
                case (int)MessageContants.RebootDevice:
                    message = "rb";
                    break;
                case (int)MessageContants.KillFirefox:
                    message = "kf";
                    break;
                case (int)MessageContants.ActivateQuickSupport:
                    message = "qs";
                    break;
                case (int)MessageContants.OpenURL:
                    message = "ou";
                    break;
                case (int)MessageContants.UploadScreenshot:
                    message = "uss";
                    break;
                case (int)MessageContants.ReporTaskerVersion:
                    message = "rtv";
                    break;
                case (int)MessageContants.UpdateTasker:
                    message = "ut";
                    break;
                case (int)MessageContants.UpdateStandardWallpaper:
                    message = "usw";
                    break;
                case (int)MessageContants.DecodeImage:
                    message = "di";
                    break;
                case (int)MessageContants.UploadTaskerDataFile:
                    message = "utdf";
                    break;
                case (int)MessageContants.RestartNetwork:
                    message = "rn";
                    break;
                case (int)MessageContants.GetDefaultURL:
                    message = "gdu";
                    break;
                case (int)MessageContants.GetOpenDefaultURL:
                    message = "godu";
                    break;
                case (int)MessageContants.OpenDefaultURL:
                    message = "odu";
                    break;
                case (int)MessageContants.ShowDeviceID:
                    message = "sdid";
                    break;
                case (int)MessageContants.GetDefaultChannel:
                    message = "gdc";
                    break;
                case (int)MessageContants.GetOfflineChannel:
                    message = "goc";
                    break;
                case (int)MessageContants.GetOfflinePack:
                    message = "gop";
                    break;
                case (int)MessageContants.OpenOffline:
                    message = "of";
                    break;
                case (int)MessageContants.RestoreTasker:
                    message = "rt";
                    break;
                case (int)MessageContants.ShowOnline:
                    message = "son";
                    break;
                case (int)MessageContants.UpdateOfflineChannel:
                    message = "uoc";
                    break;
                case (int)MessageContants.UpdateOfflinePack:
                    message = "uop";
                    break;
                case (int)MessageContants.ReportOfflineVersion:
                    message = "rov";
                    break;
                case (int)MessageContants.ReportDefaults:
                    message = "rdef";
                    break;
                case (int)MessageContants.ClearCommandQ:
                    message = "ccq";
                    break;
                case (int)MessageContants.ClearMessageQ:
                    message = "cmq";
                    break;
                case (int)MessageContants.ReportCommandQ:
                    message = "rcq";
                    break;
                case (int)MessageContants.ReportCommandRun:
                    message = "rcr";
                    break;
                case (int)MessageContants.ReportMessageQ:
                    message = "rmq";
                    break;
                case (int)MessageContants.ClearOfflineFiles:
                    message = "cof";
                    break;
                //case (int)MessageContants.:
                //    message = "";
                //    break;
            }

            SendMessage(message, selectedDevices.ToList(), model.Devices.RequestIndex, model.Devices.RequestIndexRange);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        private void SendMessage(string msg, List<Device> devices, long? index, int? indexRange)
        {
            foreach (Device device in devices)
            {
                Message message = new Message()
                {
                    MessageContent = msg,
                    DeviceID = device.ID,
                    MessageType = 1,
                    Status = 1,
                    ValidFrom = new DateTime(DateTime.Now.Year, 1, 1),
                    ValidTo = new DateTime(DateTime.Now.Year + 1, 12, 31),
                    RequestIndex = index,
                    RequestIndexRange = indexRange
                };
                db.Messages.Add(message);
            }
        }

        //
        // GET: /Message/Details/5

        [Authorize(Roles = "Administrators")]
        public ActionResult Details(long id = 0)
        {
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        //
        // GET: /Message/Create

        [Authorize(Roles = "Administrators")]
        public ActionResult Create(long arcid = 0)
        {
            ViewBag.Devices = new SelectList(db.Devices, "ID", "DeviceCode");

            if (arcid == 0)
            {
                Message message = new Message()
                {
                    MessageType = 1,
                    Status = 1,
                    ValidFrom = new DateTime(DateTime.Now.Year, 1, 1),
                    ValidTo = new DateTime(DateTime.Now.Year + 1, 12, 31)
                };

                return View(message);
            }
            else
            {
                Message msg = null;
                MessageArchive arc = db.MessageArchives.Find(arcid);
                if (arc != null)
                {
                    msg = new Message()
                    {
                        DeviceID = arc.DeviceID,
                        ExtDate1 = arc.ExtDate1,
                        ExtDate2 = arc.ExtDate2,
                        ExtDate3 = arc.ExtDate3,
                        ExtString1 = arc.ExtString1,
                        ExtString2 = arc.ExtString2,
                        ExtString3 = arc.ExtString3,
                        ExtDec1 = arc.ExtDec1,
                        ExtDec2 = arc.ExtDec2,
                        ExtDec3 = arc.ExtDec3,
                        MessageContent = arc.MessageContent,
                        MessageType = arc.MessageType,
                        Remarks = arc.Remarks,
                        Status = arc.Status,
                        ValidFrom = arc.ValidFrom,
                        ValidTo = arc.ValidTo
                    };
                }
                return View(msg);
            }
        }

        //
        // POST: /Message/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrators")]
        public ActionResult Create(Message message)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Devices = new SelectList(db.Devices, "ID", "DeviceID");

            return View(message);
        }

        //
        // GET: /Message/Edit/5

        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(long id = 0)
        {
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        //
        // POST: /Message/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrators")]
        public ActionResult Edit(Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(message);
        }

        //
        // GET: /Message/Delete/5

        [Authorize(Roles = "Administrators")]
        public ActionResult Delete(long id = 0)
        {
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        //
        // POST: /Message/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrators")]
        public ActionResult DeleteConfirmed(long id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}