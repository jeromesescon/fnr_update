using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FnR.Databases;
using FnR.Models;

namespace FnR.Front.Controllers
{
    [Authorize]
    public class PetTypeController : Controller
    {
        private FnRDbContext db = new FnRDbContext();

        //
        // GET: /PetType/

        public ActionResult Index()
        {
            return View(db.PetTypes.ToList());
        }

        //
        // GET: /PetType/Details/5

        public ActionResult Details(int id = 0)
        {
            PetType pettype = db.PetTypes.Find(id);
            if (pettype == null)
            {
                return HttpNotFound();
            }
            return View(pettype);
        }

        //
        // GET: /PetType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PetType/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PetType pettype)
        {
            if (ModelState.IsValid)
            {
                db.PetTypes.Add(pettype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pettype);
        }

        //
        // GET: /PetType/Edit/5

        public ActionResult Edit(int id = 0)
        {
            PetType pettype = db.PetTypes.Find(id);
            if (pettype == null)
            {
                return HttpNotFound();
            }
            return View(pettype);
        }

        //
        // POST: /PetType/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PetType pettype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pettype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pettype);
        }

        //
        // GET: /PetType/Delete/5

        public ActionResult Delete(int id = 0)
        {
            PetType pettype = db.PetTypes.Find(id);
            if (pettype == null)
            {
                return HttpNotFound();
            }
            return View(pettype);
        }

        //
        // POST: /PetType/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PetType pettype = db.PetTypes.Find(id);
            db.PetTypes.Remove(pettype);
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