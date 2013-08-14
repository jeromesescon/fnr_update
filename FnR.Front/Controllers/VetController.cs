using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class VetController : Controller
    {
        private FnRDbContext db = new FnRDbContext();

        //
        // GET: /Vet/

        public ActionResult Index()
        {
            return View(db.Vets.ToList());
        }

        //
        // GET: /Vet/Details/5

        public ActionResult Details(int id = 0)
        {
            Vet vet = db.Vets.Find(id);
            if (vet == null)
            {
                return HttpNotFound();
            }
            return View(vet);
        }

        //
        // GET: /Vet/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Vet/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vet vet)
        {
            if (ModelState.IsValid)
            {
                var allProducts = db.Products;
                vet.AvailableProducts = new Collection<Product>();
                foreach (var product in allProducts)
                {
                    db.Entry(product).State = EntityState.Modified;
                    vet.AvailableProducts.Add(product);
                }
                db.Vets.Add(vet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vet);
        }

        //
        // GET: /Vet/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Vet vet = db.Vets.Find(id);
            if (vet == null)
            {
                return HttpNotFound();
            }
            return View(vet);
        }

        //
        // POST: /Vet/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vet vet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vet);
        }

        //
        // GET: /Vet/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Vet vet = db.Vets.Find(id);
            if (vet == null)
            {
                return HttpNotFound();
            }
            return View(vet);
        }

        //
        // POST: /Vet/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vet vet = db.Vets.Include(r => r.PetOwners).Include(r => r.Subscriptions).SingleOrDefault(r => r.Id == id);
            var subscriptionIds = vet.Subscriptions.Select(r => r.Id).ToList();
            // Find a Default Vet...
            var defaultVet = db.Vets.SingleOrDefault(r => r.Username.ToLower() == "warehouse");
            foreach (var subscriptionId in subscriptionIds)
            {
                var subscription = db.Subscriptions.SingleOrDefault(r => r.Id == subscriptionId);
                subscription.VetId = defaultVet.Id;
                db.Entry(subscription).State = EntityState.Modified;
                //db.Subscriptions.Remove(db.Subscriptions.SingleOrDefault(r => r.Id == subscriptionId));
            }
            var userIds = vet.PetOwners.Where(r => r.VetId == vet.Id).Select(r => r.Id).ToList();
            foreach (var userId in userIds)
            {
                var user = db.Users.SingleOrDefault(r => r.Id == userId);
                user.VetId = defaultVet.Id;
                db.Entry(user).State = EntityState.Modified;
            }
            db.Vets.Remove(vet);
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