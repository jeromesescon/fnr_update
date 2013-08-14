using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FnR.Models.Interfaces;
using FnR.Repositories.Interfaces;

namespace FnR.Repositories
{
    public class Repository<RepoType> : IRepository<RepoType> where RepoType : class, IEntity
    {
        private readonly DbContext _context;
        public Repository(DbContext context)
        {
            _context = context;
        }

        public RepoType GetEntityByKey(int key)
        {
            return _context.Set<RepoType>().FirstOrDefault(r => r.Id == key);
        }

        public RepoType GetEntityByKey(int key, string[] includes)
        {
            var entities = Get(includes);
            return entities.SingleOrDefault(r => r.Id == key);
        }

        public IQueryable<RepoType> Get()
        {
            return _context.Set<RepoType>().AsQueryable();
        }

        public IQueryable<RepoType> Get(string[] includes)
        {
            var entities = _context.Set<RepoType>();
            if (!includes.Any())
                return entities;
            
            foreach (var childEntity in includes)
                entities.Include(childEntity);
            return entities;
        }

        public RepoType CreateEntity(RepoType entity)
        {
            _context.Set<RepoType>().Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public RepoType UpdateEntity(int key, RepoType update)
        {
            update.Id = key;
            _context.Set<RepoType>().Attach(update);
            _context.Entry(update).State = EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        public void Delete(int key)
        {
            var entity = _context.Set<RepoType>().FirstOrDefault(r => r.Id == key);
            _context.Set<RepoType>().Remove(entity);
            _context.SaveChanges();

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
