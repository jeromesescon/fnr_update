using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FnR.Databases;
using FnR.Repositories;
using Newtonsoft.Json;

namespace FnR.VetFront.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly VetRepository _repo;
        private readonly ProductRepository _productRepo;

        public ProductController()
        {
            _productRepo = new ProductRepository(new FnRDbContext());
            _repo = new VetRepository(new FnRDbContext());
        }
        //
        // GET: /Product/

        public ActionResult Index()
        {
            return View(_repo.GetByUsername(HttpContext.User.Identity.Name).AvailableProducts);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var addedProducts = _repo.GetByUsername(HttpContext.User.Identity.Name).AvailableProducts;
            var addedProductIds = addedProducts.Select(r => r.Id);
            ViewBag.AddedProducts = addedProducts;
            ViewBag.AvailableProducts = new SelectList(_productRepo.GetAllProducts().Where(r => !addedProductIds.Contains(r.Id)), "Id", "DisplayName");
            return View();
        }

        [HttpPost]
        public ActionResult Create(int AvailableProduct)
        {
            var vet = _repo.GetByUsername(HttpContext.User.Identity.Name);
            _repo.AddAvailableProduct(vet.Id, AvailableProduct);
            var addedProducts = _repo.GetByUsername(HttpContext.User.Identity.Name).AvailableProducts;
            var addedProductIds = addedProducts.Select(r => r.Id);
            ViewBag.AddedProducts = addedProducts;
            ViewBag.AvailableProducts = new SelectList(_productRepo.GetAllProducts().Where(r => !addedProductIds.Contains(r.Id)), "Id", "DisplayName");
            return View();
        }

        public ActionResult RemoveProduct(int productId)
        {
            var vet = _repo.GetByUsername(HttpContext.User.Identity.Name);
            _repo.RemoveProductFromList(vet.Id, productId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public string GetProduct(int productId)
        {
            return JsonConvert.SerializeObject(_productRepo.GetProduct(productId), Formatting.Indented,
                                               new JsonSerializerSettings()
                                                   {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});
        }
    }
}
