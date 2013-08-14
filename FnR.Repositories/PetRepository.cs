using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FnR.Models;

namespace FnR.Repositories
{
    public class PetRepository : Repository<Pet>
    {
        private DbContext _context;
        public PetRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public Pet GetPet(int petId)
        {
            return _context.Set<Pet>().Include(r => r.Breed).Include(r => r.Breed.PetType).SingleOrDefault(r => r.Id == petId);
        }

        public string GetVet(int petId)
        {
            string vets = string.Empty;
            var petSubs = _context.Set<Subscription>().Include(r => r.Vet).Where(r => r.PetId == petId);
            foreach (var subscription in petSubs)
            {
                if(!vets.Contains(subscription.Vet.Name))
                    vets += subscription.Vet.Name + ", ";
            }
            return vets.Trim().TrimEnd(',');
        }
    }
}
