using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using FnR.Databases;
using FnR.Models;
using FnR.Repositories;

namespace FnR.WebApi.Controllers.Services
{
    public class UserController : EntitySetController<User, int>
    {
        private readonly UserRepository _repo;
        public UserController()
        {
            _repo = new UserRepository(new FnRDbContext());
        }

        [Queryable]
        public override IQueryable<User> Get()
        {
            return _repo.Get();
        }

        protected override User GetEntityByKey(int key)
        {
            return _repo.GetEntityByKey(key);
        }

        [Queryable]
        public IQueryable<Pet> GetPetsFromUser([FromODataUri] int key)
        {
            return _repo.GetPetsFromUser(key);
        }

        [Queryable]
        public IQueryable<Subscription> GetSubscriptionsFromUser([FromODataUri] int key)
        {
            return _repo.GetSubscriptionsFromUser(key);
        }

        protected override User CreateEntity(User entity)
        {
            return _repo.CreateEntity(entity);
        }

        protected override User UpdateEntity(int key, User update)
        {
            return _repo.UpdateEntity(key, update);
        }

        public override void Delete(int key)
        {
            _repo.Delete(key);
        }

        protected override int GetKey(User entity)
        {
            return entity.Id;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _repo.Dispose();
        }
    }
}
