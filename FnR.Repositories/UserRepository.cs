using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FnR.Models;
using FnR.Repositories.Interfaces;

namespace FnR.Repositories
{
    public class UserRepository : Repository<User>
    {
        private readonly DbContext _context;
        public UserRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Pet> GetPetsFromUser(int key)
        {
            return _context.Set<Pet>().Where(r => r.UserId == key);
        }

        public IQueryable<Subscription> GetSubscriptionsFromUser(int key)
        {
            return _context.Set<Subscription>().Where(r => r.UserId == key);
        }

        public User GetByUsername(string username)
        {
            return _context.Set<User>().SingleOrDefault(r => r.Username == username);
        }

        public User GetUser(int userId)
        {
            return _context.Set<User>().Include(r => r.Pets).SingleOrDefault(r => r.Id == userId);
        }

        public string GetVet(int userId)
        {
            string vets = string.Empty;
            var userSubs = _context.Set<Subscription>().Include(r => r.Vet).Where(r => r.UserId == userId);
            foreach (var subscription in userSubs)
            {
                if (!vets.Contains(subscription.Vet.Name))
                    vets += subscription.Vet.Name + ", ";
            }
            return vets.Trim().TrimEnd(',');
        }

        public IEnumerable<User> GetVetSubscribedUsers(int vetId)
        {
           return _context.Set<User>().Where(r => r.VetId == vetId);
        }
    }
}
