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
    public class AndroidVerController : Controller
    {
        private ttTVContext db = new ttTVContext();

        //
        // GET: /AndroidVer/

        public ActionResult Index()
        {
            return View(db.AndroidVers.ToList());
        }

        //
        // GET: /AndroidVer/Details/5

        public ActionResult Details(int id = 0)
        {
            AndroidVer androidver = db.AndroidVers.Find(id);
            if (androidver == null)
            {
                return HttpNotFound();
            }
            return View(androidver);
        }

        //
        // GET: /AndroidVer/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AndroidVer/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AndroidVer androidver)
        {
            if (ModelState.IsValid)
            {
                db.AndroidVers.Add(androidver);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(androidver);
        }

        //
        // GET: /AndroidVer/Edit/5

        public ActionResult Edit(int id = 0)
        {
            AndroidVer androidver = db.AndroidVers.Find(id);
            if (androidver == null)
            {
                return HttpNotFound();
            }
            return View(androidver);
        }

        //
        // POST: /AndroidVer/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AndroidVer androidver)
        {
            if (ModelState.IsValid)
            {
                db.Entry(androidver).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(androidver);
        }

        //
        // GET: /AndroidVer/Delete/5

        public ActionResult Delete(int id = 0)
        {
            AndroidVer androidver = db.AndroidVers.Find(id);
            if (androidver == null)
            {
                return HttpNotFound();
            }
            return View(androidver);
        }

        //
        // POST: /AndroidVer/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AndroidVer androidver = db.AndroidVers.Find(id);
            db.AndroidVers.Remove(androidver);
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