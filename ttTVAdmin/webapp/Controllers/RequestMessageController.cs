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
    [Authorize(Roles = "Administrators")]
    public class RequestMessageController : Controller
    {
        private ttTVContext db = new ttTVContext();

        //
        // GET: /RequestMessage/

        public ActionResult Index(long id, int? page)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            var messages = db.RequestMessages.Where(r => r.RequestID == id).OrderByDescending(r => r.ID);

            //return View(db.RequestMessages.Where(r => r.RequestID == id).Include(m => m.Message).Include(m => m.MessageArchive));
            return View(messages.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /RequestMessage/Details/5

        public ActionResult Details(long id = 0)
        {
            RequestMessage requestmessage = db.RequestMessages.Find(id);
            if (requestmessage == null)
            {
                return HttpNotFound();
            }
            return View(requestmessage);
        }

        ////
        //// GET: /RequestMessage/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /RequestMessage/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(RequestMessage requestmessage)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.RequestMessages.Add(requestmessage);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(requestmessage);
        //}

        ////
        //// GET: /RequestMessage/Edit/5

        //public ActionResult Edit(long id = 0)
        //{
        //    RequestMessage requestmessage = db.RequestMessages.Find(id);
        //    if (requestmessage == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(requestmessage);
        //}

        ////
        //// POST: /RequestMessage/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(RequestMessage requestmessage)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(requestmessage).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(requestmessage);
        //}

        //
        // GET: /RequestMessage/Delete/5

        public ActionResult Delete(long id = 0)
        {
            RequestMessage requestmessage = db.RequestMessages.Find(id);
            if (requestmessage == null)
            {
                return HttpNotFound();
            }
            return View(requestmessage);
        }

        //
        // POST: /RequestMessage/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            RequestMessage requestmessage = db.RequestMessages.Find(id);
            db.RequestMessages.Remove(requestmessage);
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