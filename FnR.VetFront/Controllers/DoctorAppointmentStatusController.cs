using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FnR.Databases;
using FnR.Models;

namespace FnR.VetFront.Controllers
{
    public class DoctorAppointmentStatusController : Controller
    {
        private FnRDbContext db = new FnRDbContext();

        //
        // GET: /DoctorAppointmentStatus/

        public ActionResult Index()
        {
            var vet = db.Vets.SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            var doctors = db.Doctors.Where(r => r.VetId == vet.Id);

            using (var _db = new FnRDbContext())
            {
                foreach (var doctor in doctors)
                {
                    if (doctor != null)
                    {
                        var doctorForAppointmet = new DoctorAppointmentStatus
                            {
                                DoctorId = doctor.Id,
                                ExpectedWaitTime = 0,
                                ExpectedDurationOfDelay = 0
                            };
                        if (!_db.DoctorAppointmentStatuses.Where(r => r.DoctorId == doctorForAppointmet.DoctorId).Any())
                        {
                            _db.DoctorAppointmentStatuses.Add(doctorForAppointmet);
                            _db.SaveChanges();
                        }
                    }
                }
            }

            var doctorAppointmentStatuses = db.DoctorAppointmentStatuses.Include(d => d.Doctor.Vet).Where(r => r.Doctor.VetId == vet.Id);
            return View(doctorAppointmentStatuses.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            var vet = db.Vets.SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            var doctors = db.Doctors.Where(r => r.VetId == vet.Id);
            ViewBag.Doctors = new SelectList(doctors.ToList(), "Id", "Name");
            //ViewBag.VetId = vet.Id;
            return View(new DoctorAppointmentStatus());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DoctorAppointmentStatus doctorAppointmentStatus)
        {
            if (ModelState.IsValid)
            {
                db.DoctorAppointmentStatuses.Add(doctorAppointmentStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VetId = new SelectList(db.Vets, "Id", "Name", doctorAppointmentStatus.Doctor.VetId);
            return View(doctorAppointmentStatus);
        }

        //GET
        public ActionResult Edit(int id = 0)
        {
            DoctorAppointmentStatus doctorAppointmentStatus = db.DoctorAppointmentStatuses.Include(r => r.Doctor.Vet).SingleOrDefault(r => r.Id == id);
            if (doctorAppointmentStatus == null)
            {
                return HttpNotFound();
            }
            ViewBag.VetId = new SelectList(db.Vets, "Id", "Name", doctorAppointmentStatus.Doctor.VetId);
            return View(doctorAppointmentStatus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DoctorAppointmentStatus doctorAppointmentStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctorAppointmentStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VetId = new SelectList(db.Vets, "Id", "Name", doctorAppointmentStatus.Doctor.VetId);
            return View(doctorAppointmentStatus);
        }

        // GET: 

        public ActionResult Delete(int id = 0)
        {
            DoctorAppointmentStatus doctorAppointmentStatus = db.DoctorAppointmentStatuses.Find(id);
            var doctor = db.Doctors.SingleOrDefault(r => r.Id == doctorAppointmentStatus.DoctorId);

            if (doctor != null)
                doctorAppointmentStatus.Doctor.Name = doctor.Name;

            if (doctorAppointmentStatus == null)
            {
                return HttpNotFound();
            }
            return View(doctorAppointmentStatus);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DoctorAppointmentStatus doctorAppointmentStatus = db.DoctorAppointmentStatuses.Find(id);
            db.DoctorAppointmentStatuses.Remove(doctorAppointmentStatus);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id = 0)
        {
            DoctorAppointmentStatus doctorAppointmentStatus = db.DoctorAppointmentStatuses.Find(id);
            var doctor = db.Doctors.SingleOrDefault(r => r.Id == doctorAppointmentStatus.DoctorId);

            if (doctor != null)
                doctorAppointmentStatus.Doctor.Name = doctor.Name;

            if (doctorAppointmentStatus == null)
            {
                return HttpNotFound();
            }
            return View(doctorAppointmentStatus);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
