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
    public class MessageArchiveController : Controller
    {
        private ttTVContext db = new ttTVContext();

        //
        // GET: /MessageArchive/

        public ActionResult Index(int? page)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            var archive = db.MessageArchives.OrderByDescending(r => r.ID);

            return View(archive.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /MessageArchive/Details/5

        public ActionResult Details(long id = 0)
        {
            MessageArchive messagearchive = db.MessageArchives.Find(id);
            if (messagearchive == null)
            {
                return HttpNotFound();
            }
            return View(messagearchive);
        }

        ////
        //// GET: /MessageArchive/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /MessageArchive/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(MessageArchive messagearchive)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.MessageArchives.Add(messagearchive);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(messagearchive);
        //}

        ////
        //// GET: /MessageArchive/Edit/5

        //public ActionResult Edit(long id = 0)
        //{
        //    MessageArchive messagearchive = db.MessageArchives.Find(id);
        //    if (messagearchive == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(messagearchive);
        //}

        ////
        //// POST: /MessageArchive/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(MessageArchive messagearchive)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(messagearchive).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(messagearchive);
        //}

        //
        // GET: /MessageArchive/Delete/5

        public ActionResult Delete(long id = 0)
        {
            MessageArchive messagearchive = db.MessageArchives.Find(id);
            if (messagearchive == null)
            {
                return HttpNotFound();
            }
            return View(messagearchive);
        }

        //
        // POST: /MessageArchive/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            MessageArchive messagearchive = db.MessageArchives.Find(id);
            db.MessageArchives.Remove(messagearchive);
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