using System;
using System.IO;
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
    [Authorize(Roles = "Administrators")]
    public class DeviceController : Controller
    {
        private const int funcPrepareTaskerFile = 110;
        private ttTVContext db = new ttTVContext();

        // GET: /Device/

        public ActionResult Index(string err = null)
        {
            ViewBag.Error = err;
            ViewBag.Functions = new SelectList(new SelectListItem[] { 
                    new SelectListItem() { Text = "Prepare Tasker File", Value = funcPrepareTaskerFile.ToString() }, 
                    new SelectListItem() { Text = "Coming soon", Value = "2" } 
            }, "Value", "Text");

            var model = new DeviceSelectionViewModel();
            var items =
                from d in db.Devices
                //join r in db.Requests on d.ID equals r.DeviceID into dr
                //join l in db.DeviceLogs on d.ID equals l.DeviceID into dl
                select new SelectDeviceEditorViewModel
                {
                    ID = d.ID,
                    AndriodVer = d.AndriodVer,
                    CreationDate = d.CreationDate,
                    CustomerID = d.CustomerID,
                    ModelNo = d.ModelNo,
                    DeviceCode = d.DeviceCode,
                    DeviceType = d.DeviceType,
                    TaskerVersion = d.TaskerVersion,
                    TodayNetworkConnectivity = d.TodayNetworkConnectivity,
                    Last30DaysNetworkConnectivity = d.Last30DaysNetworkConnectivity,
                    LastRequestID = d.LastRequestId,
                    LastRequestDate = d.LastRequestTime,
                    LastLogID = d.LastLogId,
                    LastLogDate = d.LastLogTime,
                    LastRunMinute = d.LastRunMinute,
                    TodayShowRate = d.TodayShowRate,
                    LastRunIndex = d.LastRunIndex,
                    LastCommandIssued = d.LastCommandIssued,
                    LastCommandIssueTime = d.LastCommandIssueTime,
                    LastCommandExecuted = d.LastCommandExecuted,
                    LastCommandExecuteTime = d.LastCommandExecuteTime,
                    //Status = (Device.DeviceStatus)d.Status, // d.GetStatus(), //.HasValue? (Device.DeviceStatus)d.Status.Value: Device.DeviceStatus.Unknown,

                    Selected = true
                    //LastRequest = dr.OrderByDescending(r => r.ID).Take(1).FirstOrDefault(),
                    //LastLog = dl.OrderByDescending(r => r.ID).Take(1).FirstOrDefault()
                };

            foreach(var item in items)
                model.Devices.Add(item);

            ////var items = db.Devices
            ////    .Join(db.DeviceLogs, d => d.ID, l => l.DeviceID, (d, l) => new {device = d, log = l}) //.OrderBy(d => d.device.ID).ThenByDescending(d => d.log.ID)
            ////    .Join(db.Requests, d => d.device.ID, r => r.DeviceID, (d, r) => new {device = d.device, log = d.log, req = r})
            ////    .OrderBy(d => d.device.ID)
            ////    .ThenByDescending(d => d.req.ID)
            ////    .ThenByDescending(d => d.log.ID);
            ////var devices = 
            ////    from d in db.Devices
            ////    join r in db.Requests on d.ID equals r.DeviceID into dr 
            ////    join l in db.DeviceLogs on d.ID equals l.DeviceID into dl
            ////    orderby d.ID 
            ////    orderby r. descending
            ////    select new { Devices = d, Requests = dr, Logs = dl};
            ////foreach (var device in db.Devices.Include(d => d.Requests.OrderByDescending(o => o.ID).Take(10)).Include(d => d.DeviceLogs.OrderByDescending(l => l.ID).Take(10)))
            //foreach (var item in items) // (var device in db.Devices.Include(d => d.Requests.Where(dw => DateTime.Now.Subtract(dw.RequestDate).TotalDays ).Include(d => d.DeviceLogs))
            //{
            //    var editorViewModel = new SelectDeviceEditorViewModel()
            //    {
            //        ID = item.Device.ID,
            //        AndriodVer = item.Device.AndriodVer,
            //        CreationDate = item.Device.CreationDate,
            //        CustomerID = item.Device.CustomerID,
            //        DeviceID = item.Device.DeviceID,
            //        DeviceType = item.Device.DeviceType,
            //        ModelNo = item.Device.ModelNo,
            //        LastRequestDate = item.LastRequest == null? (DateTime?)null: item.LastRequest.RequestDate,
            //        LastRequestID = item.LastRequest == null ? (long?)null : item.LastRequest.ID,
            //        LastLogDate = item.LastLog == null ? (DateTime?)null : item.LastLog.CreationDate,
            //        LastLogID = item.LastLog == null ? (long?)null : item.LastLog.ID,
                    
            //        //ID = device.ID,
            //        //AndriodVer = device.AndriodVer,
            //        //CreationDate = device.CreationDate,
            //        //CustomerID = device.CustomerID,
            //        //DeviceID = device.DeviceID,
            //        //DeviceType = device.DeviceType,
            //        //ModelNo = device.ModelNo,
            //        Selected = true
            //    };
            //    //if (device.Requests != null && device.Requests.Count > 0)
            //    //{
            //    //    var lastReq = device.Requests.OrderByDescending(o => o.ID).First();
            //    //    editorViewModel.LastRequestDate = lastReq.RequestDate;
            //    //    editorViewModel.LastRequestID = lastReq.ID;
            //    //}
            //    //if (device.DeviceLogs != null && device.DeviceLogs.Count > 0)
            //    //{
            //    //    var lastRLog = device.DeviceLogs.OrderByDescending(o => o.ID).First();
            //    //    editorViewModel.LastLogDate = lastRLog.CreationDate;
            //    //    editorViewModel.LastLogID = lastRLog.ID;
            //    //}
            //    model.Devices.Add(editorViewModel);
            //}
            //return View(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult Execute(DeviceSelectionViewModel model)
        {
            string err = null;
            // get the ids of the items selected:
            List<long> selectedIds = model.getSelectedIds();

            // Use the ids to retrieve the records for the selected people
            // from the database:
            var selectedDevices = from x in db.Devices
                                  where selectedIds.Contains(x.ID)
                                  select x;

            switch(model.SelectedFunction)
            {
                case funcPrepareTaskerFile:
                    err = CreateDeviceTemplate(selectedDevices);
                    break;
            }

            return RedirectToAction("Index", new { err = err });
        }

        private string CreateDeviceTemplate(IQueryable<Device> selectedDevices)
        {
            string err = null;

            if (selectedDevices.Count() == 0)
                err = "No device selected.";
            else
            {

                string taskerTemplate = Server.MapPath("~/App_Data/Tasker.xml");

                if (System.IO.File.Exists(taskerTemplate))
                {
                    string tokenDevId = "<Str sr=\"arg0\" ve=\"3\">%TTTVDEVID</Str>";
                    string tokenDevIdValueStart = "<Str sr=\"arg1\" ve=\"3\">";
                    string tokenDevIdValueEnd = "</Str>";
                    string tokenPasscode = "xyzPasscode";

                    string taskerTemplateContent = System.IO.File.ReadAllText(taskerTemplate);
                    int startDevIDSearch = taskerTemplateContent.IndexOf(tokenDevId);

                    if (startDevIDSearch < 0)
                        err = "Template doesn't contains Device ID token.";
                    else
                    {
                        startDevIDSearch = taskerTemplateContent.IndexOf(tokenDevIdValueStart, startDevIDSearch);
                        if (startDevIDSearch < 0)
                            err = "Template doesn't contains Device ID (Value) start token.";
                        else
                        {
                            startDevIDSearch += tokenDevIdValueStart.Length;
                            int endDevIDSearch = taskerTemplateContent.IndexOf(tokenDevIdValueEnd, startDevIDSearch);
                            if (endDevIDSearch < 0)
                                err = "Template doesn't contains Device ID (Value) end token.";
                            else
                            {
                                if (taskerTemplateContent.Contains(tokenPasscode) == false)
                                    err = string.Format("Template doesn't contains Passcode token ({0}).", tokenPasscode);
                                else
                                {
                                    foreach (var device in selectedDevices)
                                    {
                                        string devicefolder = Server.MapPath(string.Format("~/d/{0}/{1}", device.DeviceCode, device.PassCode));
                                        string deviceTaskerFile = string.Format("{0}/userbackup.xml", devicefolder);
                                        if (Directory.Exists(devicefolder) == false)
                                            Directory.CreateDirectory(devicefolder);
                                        System.IO.File.WriteAllText(
                                            deviceTaskerFile,
                                            string.Format("{0}{1}{2}", taskerTemplateContent.Substring(0, startDevIDSearch), device.DeviceCode, taskerTemplateContent.Substring(endDevIDSearch)).Replace(tokenPasscode, device.PassCode)
                                            );
                                    }
                                }
                            }
                        }
                    }
                }
                else
                    err = "Tasker Template not exists.";
            }

            return err;
        }

        //
        // GET: /Device/Details/5

        public ActionResult Details(long id = 0)
        {
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        //
        // GET: /Device/Create

        public ActionResult Create()
        {
            ViewBag.AndroidVers = new SelectList(db.AndroidVers, "Version", "Version");
            ViewBag.ModelNos = new SelectList(db.ModelNos, "Number", "Number");

            Device device = new Device();
            device.PassCode = Guid.NewGuid().ToString();

            return View(device);
        }

        //
        // POST: /Device/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Device device)
        {
            if (ModelState.IsValid)
            {
                device.CreationDate = DateTime.Now;
                db.Devices.Add(device);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AndroidVers = new SelectList(db.AndroidVers, "Version", "Version");
            ViewBag.ModelNos = new SelectList(db.ModelNos, "Number", "Number");

            return View(device);
        }

        //
        // GET: /Device/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        //
        // POST: /Device/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Device data)
        {
            if (ModelState.IsValid)
            {
                Device device = db.Devices.Find(data.ID);

                device.AndriodVer = data.AndriodVer;
                device.CustomerID = data.CustomerID;
                device.DefaultURL = data.DefaultURL;
                device.DeploymentDate = data.DeploymentDate;
                device.DeviceCode = data.DeviceCode;
                device.DeviceType = data.DeviceType;
                device.ExtDate1 = data.ExtDate1;
                device.ExtDate2 = data.ExtDate2;
                device.ExtDate3 = data.ExtDate3;
                device.ExtDec1 = data.ExtDec1;
                device.ExtDec2 = data.ExtDec2;
                device.ExtDec3 = data.ExtDec3;
                device.ExtString1 = data.ExtString1;
                device.ExtString2 = data.ExtString2;
                device.ExtString3 = data.ExtString3;
                device.ModelNo = data.ModelNo;
                device.OfflineChannel = data.OfflineChannel;
                device.OfflineChannelValidFrom = data.OfflineChannelValidFrom;
                device.OfflineChannelValidTo = data.OfflineChannelValidTo;
                device.PassCode = data.PassCode;
                device.Remarks = data.Remarks;

                db.Entry(device).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(data);
        }

        //
        // GET: /Device/Delete/5

        public ActionResult Delete(long id = 0)
        {
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        //
        // POST: /Device/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Device device = db.Devices.Find(id);
            db.Devices.Remove(device);
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