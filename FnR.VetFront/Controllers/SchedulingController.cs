using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using FnR.Databases;
using System.Data.Entity;
using FnR.Helpers;
using FnR.Models;

namespace FnR.VetFront.Controllers
{
    [Authorize]
    public class SchedulingController : Controller
    {
        FnRDbContext _db = new FnRDbContext();
        //
        // GET: /Scheduling/

        public ActionResult Index()
        {
            var vet = _db.Vets
                .Include("Doctors.Schedules")
                .Include("Doctors.Schedules.Users")
                .Include("Doctors.Schedules.Events")
                .Include(r => r.Doctors)
                .SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);//_db.Vets.Include("Schedules.Users").Include(r => r.Schedules).Include("Schedules.Events").SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            var allSchedules = new List<Schedule>();
            foreach (var doctor in vet.Doctors)
                allSchedules.AddRange(doctor.Schedules.ToList());
            ViewBag.Schedules = allSchedules;//vet.Schedules.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            var vet = _db.Vets.SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            var doctors = _db.Doctors.Where(r => r.VetId == vet.Id);
            ViewBag.Doctors = new SelectList(doctors.ToList(), "Id", "Name");
            //ViewBag.VetId = vet.Id;
            return View(new Schedule());
        }

        [HttpPost]
        public ActionResult Create(Schedule sched)
        {
            //_db.Vets.Include(r => r.Schedules).Include("Schedules.Events").SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            //var doctors = _db.Doctors.Where(r => r.VetId == vet.Id);
            //ViewBag.Doctors = new SelectList(doctors.ToList(), "Id", "Name");

            //var allSchedules = new List<Schedule>();
            //foreach (var doctor in vet.Doctors)
            //    allSchedules.AddRange(doctor.Schedules.ToList());
            //ViewBag.Schedules = allSchedules;//vet.Schedules.ToList();

            if (ModelState.IsValid)
            {
                sched.Events = new Collection<Event>();
                _db.Schedules.Add(sched);
                _db.SaveChanges();

                return RedirectToAction("Index", "Scheduling");
            }
            var vet = _db.Vets.SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            var doctors = _db.Doctors.Where(r => r.VetId == vet.Id);
            ViewBag.Doctors = new SelectList(doctors.ToList(), "Id", "Name");
            return View(sched);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var schedule = _db.Schedules.Include(r => r.Doctor).Include(r => r.Users).Include(r => r.Events).SingleOrDefault(r => r.Id == id);
            var vet = _db.Vets.SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            ViewBag.UserList = _db.Users.Where(r => r.VetId == vet.Id).ToList();
            var doctors = _db.Doctors.Where(r => r.VetId == vet.Id);
            ViewBag.Doctors = new SelectList(doctors.ToList(), "Id", "Name");
            return View(schedule);
        }

        [HttpPost]
        public ActionResult Edit(Schedule schedule)
        {
            var vet = _db.Vets
                .Include("Doctors.Schedules")
                .Include("Doctors.Schedules.Events")
                .Include(r => r.Doctors)
                .SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            if (ModelState.IsValid)
            {
                var sched = _db.Schedules.SingleOrDefault(r => r.Id == schedule.Id);
                _db.Entry(sched).CurrentValues.SetValues(schedule);
                //_db.Entry(schedule).State = EntityState.Modified;
                _db.SaveChanges();
                var allSchedules = new List<Schedule>();
                foreach (var doctor in vet.Doctors)
                    allSchedules.AddRange(doctor.Schedules.ToList());
                ViewBag.Schedules = allSchedules;//vet.Schedules.ToList();
                return RedirectToAction("Index", "Scheduling");
            }
            ViewBag.UserList = _db.Users.Where(r => r.VetId == vet.Id).ToList();
            var doctors = _db.Doctors.Where(r => r.VetId == vet.Id);
            ViewBag.Doctors = new SelectList(doctors.ToList(), "Id", "Name");
            return View(schedule);
        }

        public ActionResult Delete(int id)
        {
            var vet = _db.Vets
                .Include(r => r.Doctors)
                .Include("Doctors.Schedules")
                .Include("Doctors.Schedules.Events")
                .SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            _db.Schedules.Remove(_db.Schedules.SingleOrDefault(r => r.Id == id));
            _db.SaveChanges();
            var allSchedules = new List<Schedule>();
            foreach (var doctor in vet.Doctors)
                allSchedules.AddRange(doctor.Schedules.ToList());
            ViewBag.Schedules = allSchedules;//vet.Schedules.ToList();
            return RedirectToAction("Index", "Scheduling");
        }

        [HttpPost]
        public ActionResult CreateEvent(Event schedEvent)
        {
            _db.Events.Add(schedEvent);
            _db.SaveChanges();
            var vet = _db.Vets.SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            ViewBag.UserList = _db.Users.Where(r => r.VetId == vet.Id).ToList();
            return RedirectToAction("Edit", "Scheduling", new { id = schedEvent.ScheduleId });
        }

        [HttpGet]
        public ActionResult CreateSchedEvent()
        {
            var vet = _db.Vets
                .Include(r => r.Doctors)
                .Include("Doctors.Schedules")
                .Include("Doctors.Schedules.Events")
                .SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);//_db.Vets.Include("Schedules.Users").Include(r => r.Schedules).Include("Schedules.Events").SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            var allSchedules = new List<Schedule>();
            foreach (var doctor in vet.Doctors)
                allSchedules.AddRange(doctor.Schedules.ToList());
            ViewBag.Schedules = allSchedules;//vet.Schedules.ToList();
            ViewBag.UserList = _db.Users.Include(r => r.Schedules).Include("Schedules.Events").Where(r => r.VetId == vet.Id).ToList();
            ViewBag.Doctors = vet.Doctors.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult CreateSchedEvent(string Title, int UserId, DateTime startDate, string intervalType, int numIntervals, int duration, int DoctorId)
        {
            var vet = _db.Vets
                .Include(r => r.Doctors)
                .Include("Doctors.Schedules")
                .Include("Doctors.Schedules.Events")
                .SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);//_db.Vets.Include(r => r.Schedules).Include("Schedules.Events").SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            var user = _db.Users.Include(r => r.Schedules).SingleOrDefault(r => r.Id == UserId);
            var userIntervalSched =
                user.Schedules.SingleOrDefault(r => r.Name.ToLower() == user.Id + "|interval schedules");

            if (userIntervalSched == null)
            {
                userIntervalSched = new Schedule()
                {
                    Name = user.Id + "|interval schedules",
                    DoctorId = DoctorId,
                    Description = user.FullName + "'s User Interval Events"
                };
                _db.Schedules.Add(userIntervalSched);
                _db.SaveChanges();
                user.Schedules.Add(userIntervalSched);
                _db.Entry(userIntervalSched).State = EntityState.Modified;
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
            }

            var schedEvent = new Event()
            {
                Title = Title,
                ScheduleId = userIntervalSched.Id,
                StartDateTime = startDate,
                EndDateTime = startDate.AddMinutes(duration)
            };
            _db.Events.Add(schedEvent);
            _db.SaveChanges();

            for (int i = 1; i < numIntervals; i++)
            {
                switch (intervalType)
                {
                    case "min":
                        schedEvent.StartDateTime = schedEvent.StartDateTime.AddMinutes(1);
                        break;
                    case "hour":
                        schedEvent.StartDateTime = schedEvent.StartDateTime.AddHours(1);
                        break;
                    case "day":
                        schedEvent.StartDateTime = schedEvent.StartDateTime.AddDays(1);
                        break;
                    case "week":
                        schedEvent.StartDateTime = schedEvent.StartDateTime.AddDays(7);
                        break;
                    case "month":
                        schedEvent.StartDateTime = schedEvent.StartDateTime.AddMonths(1);
                        break;
                }
                schedEvent.EndDateTime = schedEvent.StartDateTime.AddMinutes(duration);
                _db.Events.Add(schedEvent);
                _db.SaveChanges();
            }


            //var schedule = vet.Schedules.SingleOrDefault(r => r.Name.ToLower().Contains(user.Id + "|interval schedules"));
            ViewBag.UserList = _db.Users.Include(r => r.Schedules).Include("Schedules.Events").Where(r => r.VetId == vet.Id).ToList();
            ViewBag.Doctors = vet.Doctors.ToList();
            //_db.Events.Add(schedEvent);
            //_db.SaveChanges();
            //ViewBag.Schedule = schedule;
            return View();
        }

        public ActionResult DeleteEvent(int id, bool redirectIndex)
        {
            var schedEvent = _db.Events.SingleOrDefault(r => r.Id == id);
            var schedId = schedEvent.ScheduleId;
            _db.Events.Remove(schedEvent);
            _db.SaveChanges();
            var vet = _db.Vets.SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            ViewBag.UserList = _db.Users.Where(r => r.VetId == vet.Id).ToList();
            return redirectIndex ? RedirectToAction("Index", "Scheduling") : RedirectToAction("Edit", "Scheduling", new { id = schedId });
        }

        public ActionResult Unassign(int userId, int schedId)
        {
            var user = _db.Users.Include(r => r.Schedules).SingleOrDefault(r => r.Id == userId);
            var schedule = _db.Schedules.Include(r => r.Events).SingleOrDefault(r => r.Id == schedId);
            user.Schedules.Remove(user.Schedules.SingleOrDefault(r => r.Id == schedId));
            _db.Entry(user).State = EntityState.Modified;
            _db.SaveChanges();
            var vet = _db.Vets.SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            ViewBag.UserList = _db.Users.Where(r => r.VetId == vet.Id).ToList(); try
            {
                var body = "<h2>" + schedule.Name + " Appointments Cancelled</h2>";
                body += "<table>";
                body += "<thead>";
                body += "<tr>";
                body += "<th style='padding: 10px; border: solid 1px black; background-color: #eee;'>";
                body += "Event Title";
                body += "</th>";
                body += "<th style='padding: 10px; border: solid 1px black; background-color: #eee;'>";
                body += "Start Date / Time";
                body += "</th>";
                body += "<th style='padding: 10px; border: solid 1px black; background-color: #eee;'>";
                body += "End Date Time";
                body += "</th>";
                body += "</tr>";
                body += "</thead>";
                body += "<tbody>";
                foreach (var schedEvent in schedule.Events)
                {
                    body += "<tr>";
                    body += "<td style='padding: 10px; border: solid 1px black;'>";
                    body += schedEvent.Title;
                    body += "</td>";
                    body += "<td style='padding: 10px; border: solid 1px black;'>";
                    body += schedEvent.StartDateTime.ToString(CultureInfo.InvariantCulture);
                    body += "</td>";
                    body += "<td style='padding: 10px; border: solid 1px black;'>";
                    body += schedEvent.EndDateTime.ToString(CultureInfo.InvariantCulture);
                    body += "</td>";
                    body += "</tr>";
                }
                body += "</tbody>";
                body += "</table>";
                //var attachmentContent = { "BEGIN:VCALENDAR",
                //                        "PRODID:-//Flo Inc.//FloSoft//EN",
                //                        "BEGIN:VEVENT",
                //                        "DTSTART:" + .ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"), 
                //                        "DTEND:" + schEndDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"), 
                //                        "LOCATION:" + strLocation, 
                //                        "DESCRIPTION;ENCODING=ESCAPED-CHAR:" + strDesc,
                //                        "SUMMARY:" + strSubject, 
                //                        "PRIORITY:3", 
                //                        "END:VEVENT", 
                //                        "END:VCALENDAR" 
                //                        }; ;
                //var attachment = new Attachment(schedule.Name + ".ics", attachmentContent);
                //EmailHelper.SendEmailWithAttachment(user.Email, "New Schedule Received", body, attachment);
                EmailHelper.SendEmail(user.Email, "Removed Appointments", body);
            }
            catch (Exception ex)
            {
            }
            return RedirectToAction("Edit", "Scheduling", new { id = schedId });
        }

        public ActionResult AssignSchedule(int ScheduleId, int UserId)
        {
            var user = _db.Users.Include(r => r.Schedules).SingleOrDefault(r => r.Id == UserId);
            var schedule = _db.Schedules.Include(r => r.Events).SingleOrDefault(r => r.Id == ScheduleId);
            if (user.Schedules == null)
                user.Schedules = new Collection<Schedule>();
            _db.Entry(user).State = EntityState.Modified;
            _db.Entry(schedule).State = EntityState.Modified;
            user.Schedules.Add(schedule);
            _db.SaveChanges();
            var vet = _db.Vets.SingleOrDefault(r => r.Username == HttpContext.User.Identity.Name);
            ViewBag.UserList = _db.Users.Where(r => r.VetId == vet.Id).ToList();
            try
            {
                var body = "<h2>" + schedule.Name + " Appointments </h2>";
                body += "<table>";
                body += "<thead>";
                body += "<tr>";
                body += "<th style='padding: 10px; border: solid 1px black; background-color: #eee;'>";
                body += "Event Title";
                body += "</th>";
                body += "<th style='padding: 10px; border: solid 1px black; background-color: #eee;'>";
                body += "Start Date / Time";
                body += "</th>";
                body += "<th style='padding: 10px; border: solid 1px black; background-color: #eee;'>";
                body += "End Date Time";
                body += "</th>";
                body += "</tr>";
                body += "</thead>";
                body += "<tbody>";
                foreach (var schedEvent in schedule.Events)
                {
                    body += "<tr>";
                    body += "<td style='padding: 10px; border: solid 1px black;'>";
                    body += schedEvent.Title;
                    body += "</td>";
                    body += "<td style='padding: 10px; border: solid 1px black;'>";
                    body += schedEvent.StartDateTime.ToString(CultureInfo.InvariantCulture);
                    body += "</td>";
                    body += "<td style='padding: 10px; border: solid 1px black;'>";
                    body += schedEvent.EndDateTime.ToString(CultureInfo.InvariantCulture);
                    body += "</td>";
                    body += "</tr>";
                }
                body += "</tbody>";
                body += "</table>";
                //var attachmentContent = { "BEGIN:VCALENDAR",
                //                        "PRODID:-//Flo Inc.//FloSoft//EN",
                //                        "BEGIN:VEVENT",
                //                        "DTSTART:" + .ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"), 
                //                        "DTEND:" + schEndDate.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"), 
                //                        "LOCATION:" + strLocation, 
                //                        "DESCRIPTION;ENCODING=ESCAPED-CHAR:" + strDesc,
                //                        "SUMMARY:" + strSubject, 
                //                        "PRIORITY:3", 
                //                        "END:VEVENT", 
                //                        "END:VCALENDAR" 
                //                        }; ;
                //var attachment = new Attachment(schedule.Name + ".ics", attachmentContent);
                //EmailHelper.SendEmailWithAttachment(user.Email, "New Schedule Received", body, attachment);
                EmailHelper.SendEmail(user.Email, "New Schedule Received", body);
            }
            catch (Exception ex)
            {
            }
            return RedirectToAction("Edit", "Scheduling", new { id = ScheduleId });
        }
    }
}
