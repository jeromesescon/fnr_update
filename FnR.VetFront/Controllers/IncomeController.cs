using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FnR.Databases;
using FnR.Repositories;

namespace FnR.VetFront.Controllers
{
    [Authorize]
    public class IncomeController : Controller
    {private readonly VetRepository _repo;

        public IncomeController()
        {
            _repo = new VetRepository(new FnRDbContext());
        }
        //
        // GET: /Income/

        public ActionResult Index()
        {
            var vet = _repo.GetByUsername(HttpContext.User.Identity.Name);
            var subscriptions = vet.Subscriptions.Where(r => !r.Sent).ToList();
            ViewBag.TotalSubscriptions = subscriptions.Count;
            ViewBag.TotalEarnings = subscriptions.Sum(r => r.Product.Price);
            return View(subscriptions);
        }

    }
}
