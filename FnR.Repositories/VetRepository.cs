using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FnR.Models;

namespace FnR.Repositories
{
    public class VetRepository : Repository<Vet>
    {
        private readonly DbContext _context;
        public VetRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public bool AuthenticateVet(string username, string password)
        {
            return _context.Set<Vet>().SingleOrDefault(r => r.Username == username && r.Password == password) != null;
        }

        public Vet GetByUsername(string username)
        {
            return _context.Set<Vet>()
                .Include(r => r.AvailableProducts)
                .Include("AvailableProducts.PetType")
                .Include(r => r.Subscriptions)
                .Include("Subscriptions.Pet")
                .Include("Subscriptions.Product")
                .Include("Subscriptions.User")
                .Include(r => r.Conditions)
                .SingleOrDefault(r => r.Username == username);
        }

        public void AddAvailableProduct(int vetId, int availableProductId)
        {
            var product = _context.Set<Product>().SingleOrDefault(r => r.Id == availableProductId);
            var vet = _context.Set<Vet>().SingleOrDefault(r => r.Id == vetId);
            vet.AvailableProducts.Add(product);
            _context.Entry(product).State = EntityState.Modified;
            _context.Entry(vet).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void RemoveProductFromList(int vetId, int productId)
        {
            var vet = _context.Set<Vet>().Include(r => r.AvailableProducts).SingleOrDefault(r => r.Id == vetId);
            var productItem = _context.Set<Product>().SingleOrDefault(r => r.Id == productId);
            vet.AvailableProducts.Remove(productItem);
            _context.Entry(productItem).State = EntityState.Modified;
            _context.Entry(vet).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
