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
    public class PostVisitCareController : Controller
    {
        private readonly VetRepository _vetRepo;
        private readonly ConditionRepository _conditionRepo;
        private readonly UserRepository _userRepo;
        public PostVisitCareController()
        {
            _vetRepo = new VetRepository(new FnRDbContext());
            _conditionRepo = new ConditionRepository(new FnRDbContext());
            _userRepo = new UserRepository(new FnRDbContext());
        }
        //
        // GET: /PostVisitCare/

        public ActionResult Index()
        {
            var username = HttpContext.User.Identity.Name;
            var currentVet = _vetRepo.GetByUsername(username);
            ViewBag.UserList = _userRepo.Get().Where(r => r.VetId == currentVet.Id).ToList();
            return View(currentVet);
        }

        public string AddCondition(Condition condition)
        {
            return JsonConvert.SerializeObject(_conditionRepo.CreateEntity(condition));
        }

        [HttpGet]
        public ActionResult EditCondition(int id)
        {
            var condition = _conditionRepo.GetEntityByKey(id);
            return View(condition);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditCondition(Condition condition)
        {
            _conditionRepo.UpdateEntity(condition.Id, condition);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void DeleteCondition(int Id)
        {
            _conditionRepo.Delete(Id);
        }

        public string GetUserPets(int userId)
        {
            return JsonConvert.SerializeObject(_userRepo.GetPetsFromUser(userId));
        }

        public string GetPetConditions(int petId)
        {
            var petConditions = _conditionRepo.GetPetConditions(petId);
            return JsonConvert.SerializeObject(petConditions, Formatting.Indented,
                                new JsonSerializerSettings()
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                });
        }

        public string AddPetCondition(int petId, int conditionId)
        {
            var newPetConditions = _conditionRepo.AddNewPetCondition(petId, conditionId);
            return JsonConvert.SerializeObject(newPetConditions, Formatting.Indented,
                                new JsonSerializerSettings()
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                });

        }

        public string RemovePetCondition(int petId, int conditionId)
        {
            var updatedPetConditions = _conditionRepo.RemovePetCondition(petId, conditionId);
            return JsonConvert.SerializeObject(updatedPetConditions, Formatting.Indented,
                                new JsonSerializerSettings()
                                {
                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                });
        }
    }
}
