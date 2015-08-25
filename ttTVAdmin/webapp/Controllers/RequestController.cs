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
    public class RequestController : Controller
    {
        private ttTVContext db = new ttTVContext();

        //
        // GET: /Request/

        public ActionResult Index(int? page)
        {
            int pageSize = 20; 
            int pageNumber = (page ?? 1);

            var requests = db.Requests.OrderByDescending(r => r.ID);

            return View(requests.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Request/Details/5

        public ActionResult Details(long id = 0)
        {
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        ////
        //// GET: /Request/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /Request/Create

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Request request)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Requests.Add(request);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(request);
        //}

        ////
        //// GET: /Request/Edit/5

        //public ActionResult Edit(long id = 0)
        //{
        //    Request request = db.Requests.Find(id);
        //    if (request == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(request);
        //}

        ////
        //// POST: /Request/Edit/5

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Request request)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(request).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(request);
        //}

        //
        // GET: /Request/Delete/5

        public ActionResult Delete(long id = 0)
        {
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        //
        // POST: /Request/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
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