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
    public class SubscriptionController : Controller
    {
        private FnRDbContext db = new FnRDbContext();

        //
        // GET: /Subscription/

        public ActionResult Index()
        {
            var subscriptions = db.Subscriptions.Include(s => s.User).Include(s => s.Pet).Include(s => s.Product).Include(s => s.Vet);
            return View(subscriptions.ToList());
        }

        //
        // GET: /Subscription/Details/5

        public ActionResult Details(int id = 0)
        {
            Subscription subscription =
                db.Subscriptions.Include(r => r.User).Include(r => r.Pet).Include(r => r.Product).Include(r => r.Vet).
                    FirstOrDefault(r => r.Id == id);
            if (subscription == null)
            {
                return HttpNotFound();
            }
            return View(subscription);
        }

        //
        // GET: /Subscription/Create

        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "FullName");
            ViewBag.PetId = new SelectList(db.Pets, "Id", "Name");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "DisplayName");
            ViewBag.VetId = new SelectList(db.Vets, "Id", "Name");
            return View();
        }

        //
        // POST: /Subscription/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                db.Subscriptions.Add(subscription);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", subscription.UserId);
            ViewBag.PetId = new SelectList(db.Pets, "Id", "Name", subscription.PetId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", subscription.ProductId);
            ViewBag.VetId = new SelectList(db.Vets, "Id", "Name", subscription.VetId);
            return View(subscription);
        }

        //
        // GET: /Subscription/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Subscription subscription = db.Subscriptions.Find(id);
            if (subscription == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", subscription.UserId);
            ViewBag.PetId = new SelectList(db.Pets, "Id", "Name", subscription.PetId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", subscription.ProductId);
            ViewBag.VetId = new SelectList(db.Vets, "Id", "Name", subscription.VetId);
            return View(subscription);
        }

        //
        // POST: /Subscription/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Subscription subscription)
        {
            if (ModelState.IsValid)
            {

                db.Entry(subscription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", subscription.UserId);
            ViewBag.PetId = new SelectList(db.Pets, "Id", "Name", subscription.PetId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", subscription.ProductId);
            ViewBag.VetId = new SelectList(db.Vets, "Id", "Name", subscription.VetId);
            return View(subscription);
        }

        //
        // GET: /Subscription/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Subscription subscription = db.Subscriptions.Include(r => r.User).Include(r => r.Pet).Include(r => r.Product).Include(r => r.Vet).
                    FirstOrDefault(r => r.Id == id);
            if (subscription == null)
            {
                return HttpNotFound();
            }
            return View(subscription);
        }

        //
        // POST: /Subscription/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subscription subscription = db.Subscriptions.Find(id);
            db.Subscriptions.Remove(subscription);
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