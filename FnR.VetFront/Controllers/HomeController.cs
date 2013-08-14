using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FnR.Databases;
using FnR.Front.ViewModels;
using FnR.Repositories;
using VMLogin = FnR.VetFront.ViewModels.VMLogin;

namespace FnR.VetFront.Controllers
{
    public class HomeController : Controller
    {
        private readonly VetRepository _repo;

        public HomeController()
        {
            _repo = new VetRepository(new FnRDbContext());
        }
        //
        // GET: /Home/
        [Authorize]
        public ActionResult Index()
        {
            var vet = _repo.GetByUsername(HttpContext.User.Identity.Name);
            return View(vet);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new VMLogin());
        }

        [HttpPost]
        public ActionResult Login(VMLogin login)
        {
            if(ModelState.IsValid)
            {
                if (_repo.AuthenticateVet(login.Username, login.Password))
                {
                    FormsAuthentication.SetAuthCookie(login.Username, false);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("unauthorized", "Invalid Username / Password");
                return View(login);
            }
            return View(login);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}
