using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FnR.Models;
using FnR.Databases;

namespace FnR.WarehouseFront.Controllers
{
    public class DeliveryController : Controller
    {
        private FnRDbContext db = new FnRDbContext();

        //
        // GET: /Delivery/

        public ActionResult Index(string fUndelivered = "all", string fDelivered = "all")
        {
            ViewBag.FUndelivered = fUndelivered.ToLower();
            ViewBag.FDelivered = fDelivered.ToLower();
            var subscriptions = db.Subscriptions.Include(s => s.User).Include(s => s.Pet).Include(s => s.Product).Include(s => s.Vet);
            return View(subscriptions.OrderBy(r => r.NextDeliveryDate).ToList());
        }

        public ActionResult Deliver(int id)
        {
            var subscription = db.Subscriptions.SingleOrDefault(r => r.Id == id);
            if(subscription.Sent)
            {
                subscription.Sent = false;
                db.Entry(subscription).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                var newSub = new Subscription
                                 {
                                     PetId = subscription.PetId,
                                     ProductId = subscription.ProductId,
                                     UserId = subscription.UserId,
                                     VetId = subscription.VetId,
                                     DateSubscribed = subscription.DateSubscribed
                                 };
                var deliveryMonth = subscription.NextDeliveryDate.Month + 1;
                var deliveryYear = subscription.NextDeliveryDate.Year;
                const int deliveryDay = 1;
                var deliveryDate = new DateTime(deliveryYear, deliveryMonth, deliveryDay);
                while (deliveryDate.DayOfWeek != DayOfWeek.Monday)
                {
                    deliveryDate = deliveryDate.AddDays(1);
                }
                newSub.NextDeliveryDate = deliveryDate;
                subscription.Sent = true;
                db.Entry(subscription).State = EntityState.Modified;
                db.Subscriptions.Add(newSub);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Delivery/Details/5

        public ActionResult Details(int id = 0)
        {
            Subscription subscription = db.Subscriptions.Find(id);
            if (subscription == null)
            {
                return HttpNotFound();
            }
            return View(subscription);
        }

        //
        // GET: /Delivery/Create

        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.PetId = new SelectList(db.Pets, "Id", "Name");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            ViewBag.VetId = new SelectList(db.Vets, "Id", "Name");
            return View();
        }

        //
        // POST: /Delivery/Create

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
        // GET: /Delivery/Edit/5

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
        // POST: /Delivery/Edit/5

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
        // GET: /Delivery/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Subscription subscription = db.Subscriptions.Find(id);
        //    if (subscription == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(subscription);
        //}

        //
        // POST: /Delivery/Delete/5

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
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