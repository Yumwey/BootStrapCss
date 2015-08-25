using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ttTVMS.Models;

namespace ttTVMS.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class ModelNoController : Controller
    {
        private ttTVContext db = new ttTVContext();

        //
        // GET: /ModelNo/

        public ActionResult Index()
        {
            return View(db.ModelNos.ToList());
        }

        //
        // GET: /ModelNo/Details/5

        public ActionResult Details(long id = 0)
        {
            ModelNo modelno = db.ModelNos.Find(id);
            if (modelno == null)
            {
                return HttpNotFound();
            }
            return View(modelno);
        }

        //
        // GET: /ModelNo/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ModelNo/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ModelNo modelno)
        {
            if (ModelState.IsValid)
            {
                db.ModelNos.Add(modelno);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(modelno);
        }

        //
        // GET: /ModelNo/Edit/5

        public ActionResult Edit(long id = 0)
        {
            ModelNo modelno = db.ModelNos.Find(id);
            if (modelno == null)
            {
                return HttpNotFound();
            }
            return View(modelno);
        }

        //
        // POST: /ModelNo/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ModelNo modelno)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modelno).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(modelno);
        }

        //
        // GET: /ModelNo/Delete/5

        public ActionResult Delete(long id = 0)
        {
            ModelNo modelno = db.ModelNos.Find(id);
            if (modelno == null)
            {
                return HttpNotFound();
            }
            return View(modelno);
        }

        //
        // POST: /ModelNo/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ModelNo modelno = db.ModelNos.Find(id);
            db.ModelNos.Remove(modelno);
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