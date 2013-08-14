using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FnR.Databases;
using FnR.Models;
using FnR.Repositories;
using Newtonsoft.Json;

namespace FnR.VetFront.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly VetRepository _repo;
        private readonly SubscriptionRepository _subRepo;
        private readonly UserRepository _userRepo;
        private readonly ProductRepository _productRepo;

        public SubscriptionController()
        {
            _repo = new VetRepository(new FnRDbContext());
            _subRepo = new SubscriptionRepository(new FnRDbContext());
            _userRepo = new UserRepository(new FnRDbContext());
            _productRepo = new ProductRepository(new FnRDbContext());
        }
        //
        // GET: /Subscription/

        public ActionResult Index()
        {
            return View(_repo.GetByUsername(HttpContext.User.Identity.Name).Subscriptions.Where(r => !r.Sent));
        }

        [HttpGet]
        public ActionResult Create()
        {
            var vet = _repo.GetByUsername(HttpContext.User.Identity.Name);
            ViewBag.VetId = vet.Id;
            ViewBag.UserList = new SelectList(_userRepo.GetVetSubscribedUsers(vet.Id), "Id", "FullName");
            ViewBag.AddedSubscriptions = vet.Subscriptions;
            return View(new Subscription());
        }

        [HttpPost]
        public ActionResult Create(Subscription subscription)
        {
            if(ModelState.IsValid)
            {
                _subRepo.CreateSubscription(subscription);
                return RedirectToAction("Index");
            } 
            var vet = _repo.GetByUsername(HttpContext.User.Identity.Name);
            ViewBag.VetId = vet.Id;
            ViewBag.UserList = new SelectList(_userRepo.GetVetSubscribedUsers(vet.Id), "Id", "FullName");
            ViewBag.AddedSubscriptions = vet.Subscriptions.Where(r => !r.Sent);
            return View(subscription);
        }

        [HttpGet]
        public string GetUserPets(int userId)
        {
            var userPets = _userRepo.GetUser(userId).Pets;
            return JsonConvert.SerializeObject(userPets, Formatting.Indented,
                                               new JsonSerializerSettings()
                                                   {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
        }

        [HttpGet]
        public string GetPetProducts(double weight, int petId)
        {
            var petProducts = _productRepo.GetPetProducts(weight, petId, HttpContext.User.Identity.Name);
            return JsonConvert.SerializeObject(petProducts, Formatting.Indented,
                                               new JsonSerializerSettings()
                                                   {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _subRepo.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
