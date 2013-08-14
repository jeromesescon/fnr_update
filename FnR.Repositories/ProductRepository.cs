using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FnR.Models;

namespace FnR.Repositories
{
    public class ProductRepository : Repository<Product>
    {
        private readonly DbContext _context;
        public ProductRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Set<Product>();
                //.Include(r => r.PetType)
                //.Include("PetType.Breeds");
        }

        public Product GetProduct(int productId)
        {
            return _context.Set<Product>().Include(r => r.PetType).SingleOrDefault(r => r.Id == productId);
        }

        public IEnumerable<Product> GetPetProducts(double weight, int petId, string vetUsername)
        {
            var pet =
                _context.Set<Pet>().Include(r => r.Breed).Include(r => r.Breed.PetType).SingleOrDefault(
                    r => r.Id == petId);
            var vet = _context.Set<Vet>()
                .Include(r => r.AvailableProducts)
                .Include("AvailableProducts.PetType")
                .Include(r => r.Subscriptions)
                .Include("Subscriptions.Pet")
                .Include("Subscriptions.Product")
                .SingleOrDefault(r => r.Username == vetUsername);//_repo.GetByUsername(HttpContext.User.Identity.Name);
            var subscribedProductIds = vet.Subscriptions.Where(r => r.PetId == pet.Id).Select(r => r.ProductId);
            var availableProducts = vet.AvailableProducts.Where(r => !subscribedProductIds.Contains(r.Id));
            return 
                availableProducts.Where(r => weight >= r.LowerWeightLimit && weight <= r.HeigherWeightLimit && r.PetTypeId == pet.Breed.PetTypeId);
        }
    }
}
