using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FnR.Databases;
using FnR.Helpers;
using FnR.Models;

namespace FnR.Front.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private FnRDbContext db = new FnRDbContext();

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View(db.Users.Include(r => r.Vet).ToList());
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int id = 0)
        {
            User user = db.Users.Include(r => r.Vet).SingleOrDefault(r => r.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            ViewBag.VetList = new SelectList(db.Vets, "Id", "Name");
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                if (user.Email != null)
                {
                    try
                    {
                        EmailHelper.SendEmail(user.Email, "FelixAndRover Registration",
                                              "<h1>Registration Successful</h1><p>You have successfully registered to Felix and Rover App..</p>");
                    }
                    catch
                    {
                    }
                }
                return RedirectToAction("Index");
            }
            ViewBag.VetList = new SelectList(db.Vets, "Id", "Name");
            return View(user);
        }

        //
        // GET: /User/Edit/5

        public ActionResult Edit(int id = 0)
        {
            User user = db.Users.Find(id);
            ViewBag.VetList = new SelectList(db.Vets, "Id", "Name");
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VetList = new SelectList(db.Vets, "Id", "Name");
            return View(user);
        }

        //
        // GET: /User/Delete/5

        public ActionResult Delete(int id = 0)
        {
            User user = db.Users.Include(r => r.Vet).SingleOrDefault(r => r.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Include(r => r.Pets).SingleOrDefault(r => r.Id == id);
            var userPetIds = db.Pets.Where(r => r.UserId == id).Select(r => r.Id).ToList();

            foreach (var userPetId in userPetIds)
            {
                var userPetSubscriptionIds =
                    db.Subscriptions.Where(r => r.PetId == userPetId).Select(r => r.Id).ToList();
                foreach (var userPetSubscriptionId in userPetSubscriptionIds)
                    db.Subscriptions.Remove(db.Subscriptions.SingleOrDefault(r => r.Id == userPetSubscriptionId));

                db.Pets.Remove(db.Pets.SingleOrDefault(r => r.Id == userPetId));

            }
            db.Users.Remove(user);
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