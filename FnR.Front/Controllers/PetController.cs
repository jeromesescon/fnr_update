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
    public class PetController : Controller
    {
        private FnRDbContext db = new FnRDbContext();

        //
        // GET: /Pet/

        public ActionResult Index()
        {
            var pets = db.Pets.Include(p => p.Breed).Include(p => p.User).Include(r => r.User.Vet);
            return View(pets.ToList());
        }

        //
        // GET: /Pet/Details/5

        public ActionResult Details(int id = 0)
        {
            Pet pet = db.Pets.Include(r => r.Breed).Include(r => r.Breed.PetType).Include(r => r.User).FirstOrDefault(r => r.Id == id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        //
        // GET: /Pet/Create

        public ActionResult Create()
        {
            ViewBag.BreedId = new SelectList(db.Breeds.Include(r => r.PetType), "Id", "DisplayName");
            ViewBag.UserId = new SelectList(db.Users, "Id", "FullName");
            return View();
        }

        //
        // POST: /Pet/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pet pet)
        {
            if (ModelState.IsValid)
            {
                db.Pets.Add(pet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BreedId = new SelectList(db.Breeds.Include(r => r.PetType), "Id", "DisplayName", pet.BreedId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FullName", pet.UserId);
            return View(pet);
        }

        //
        // GET: /Pet/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Pet pet = db.Pets.Find(id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            ViewBag.BreedId = new SelectList(db.Breeds.Include(r => r.PetType), "Id", "DisplayName", pet.BreedId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FullName", pet.UserId);
            return View(pet);
        }

        //
        // POST: /Pet/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pet pet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BreedId = new SelectList(db.Breeds.Include(r => r.PetType), "Id", "DisplayName", pet.BreedId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FullName", pet.UserId);
            return View(pet);
        }

        //
        // GET: /Pet/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Pet pet = db.Pets.Include(r => r.Breed).Include(r => r.Breed.PetType).Include(r => r.User).FirstOrDefault(r => r.Id == id);
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        //
        // POST: /Pet/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pet pet = db.Pets.Find(id);
            var subscriptionIds = db.Subscriptions.Where(r => r.PetId == pet.Id).Select(r => r.Id).ToList();
            foreach (var subscriptionId in subscriptionIds)
                db.Subscriptions.Remove(db.Subscriptions.SingleOrDefault(r => r.Id == subscriptionId));
            db.Pets.Remove(pet);
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