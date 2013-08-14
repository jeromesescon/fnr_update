using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FnR.Front.ViewModels;

namespace FnR.Front.Controllers
{
    /// <summary>
    /// Administrative controller class. For authentication and authorization.
    /// </summary>
    public class AdminController : Controller
    {
        /// <summary>
        /// Login Initial View.
        /// </summary>
        /// <returns>A login view page.</returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View(new VMLogin());
        }

        /// <summary>
        /// Post function of login to authorize username and password.
        /// </summary>
        /// <param name="login">Login object with username and password.</param>
        /// <returns>A redirecto to administrative dashboard if successfully authorized
        /// and an authorization login otherwise.</returns>
        [HttpPost]
        public ActionResult Login(VMLogin login)
        {
            if(ModelState.IsValid)
            {
                if(login.Username == "admin" && login.Password == "adminpass123")
                {
                    FormsAuthentication.SetAuthCookie(login.Username, true);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("auth-failed", "Invalid Username / Password!");
                return View(login);
            }
            return View(login);
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>A redirect to the login screen.</returns>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Admin");
        }
    }
}
