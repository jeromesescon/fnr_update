using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FnR.Models;
using FnR.Databases;

namespace FnR.VetFront.Controllers
{
    public class DoctorsController : Controller
    {
        private FnRDbContext db = new FnRDbContext();

        //
        // GET: /Doctors/

        public ActionResult Index()
        {
            var vet = db.Vets.SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            var doctors = db.Doctors.Include(d => d.Vet).Where(r => r.VetId == vet.Id);
            return View(doctors.ToList());
        }

        //
        // GET: /Doctors/Details/5

        public ActionResult Details(int id = 0)
        {
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        //
        // GET: /Doctors/Create

        public ActionResult Create()
        {
            //ViewBag.VetId = new SelectList(db.Vets, "Id", "Name");
            var vet = db.Vets.SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            ViewBag.VetName = vet.Name;
            ViewBag.VetId = vet.Id;
            return View();
        }

        //
        // POST: /Doctors/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Doctors.Add(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VetId = new SelectList(db.Vets, "Id", "Name", doctor.VetId);
            return View(doctor);
        }

        //
        // GET: /Doctors/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Doctor doctor = db.Doctors.Include(r => r.Vet).SingleOrDefault(r => r.Id == id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            ViewBag.VetId = new SelectList(db.Vets, "Id", "Name", doctor.VetId);
            return View(doctor);
        }

        //
        // POST: /Doctors/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VetId = new SelectList(db.Vets, "Id", "Name", doctor.VetId);
            return View(doctor);
        }

        //
        // GET: /Doctors/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        //
        // POST: /Doctors/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            db.Doctors.Remove(doctor);
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