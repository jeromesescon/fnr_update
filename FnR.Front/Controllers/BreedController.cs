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
    public class BreedController : Controller
    {
        private FnRDbContext db = new FnRDbContext();

        //
        // GET: /Breed/
        public ActionResult Index()
        {
            var breeds = db.Breeds.Include(b => b.PetType);
            return View(breeds.ToList());
        }

        //
        // GET: /Breed/Details/5
        public ActionResult Details(int id = 0)
        {
            Breed breed = db.Breeds.Include(r => r.PetType).FirstOrDefault(r => r.Id == id);
            if (breed == null)
            {
                return HttpNotFound();
            }
            return View(breed);
        }

        //
        // GET: /Breed/Create
        public ActionResult Create()
        {
            ViewBag.PetTypeId = new SelectList(db.PetTypes, "Id", "Name");
            return View();
        }

        //
        // POST: /Breed/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Breed breed)
        {
            if (ModelState.IsValid)
            {
                db.Breeds.Add(breed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PetTypeId = new SelectList(db.PetTypes, "Id", "Name", breed.PetTypeId);
            return View(breed);
        }

        //
        // GET: /Breed/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Breed breed = db.Breeds.Include(r => r.PetType).FirstOrDefault(r => r.Id == id);
            if (breed == null)
            {
                return HttpNotFound();
            }
            ViewBag.PetTypeId = new SelectList(db.PetTypes, "Id", "Name", breed.PetTypeId);
            return View(breed);
        }

        //
        // POST: /Breed/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Breed breed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(breed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PetTypeId = new SelectList(db.PetTypes, "Id", "Name", breed.PetTypeId);
            return View(breed);
        }

        //
        // GET: /Breed/Delete/5
        public ActionResult Delete(int id = 0)
        {
            Breed breed = db.Breeds.Include(r => r.PetType).FirstOrDefault(r => r.Id == id);
            if (breed == null)
            {
                return HttpNotFound();
            }
            return View(breed);
        }

        //
        // POST: /Breed/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Breed breed = db.Breeds.Find(id);
            db.Breeds.Remove(breed);
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