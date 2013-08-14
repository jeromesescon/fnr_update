using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FnR.Repositories.Interfaces
{
    interface IRepository<T>
    {
        T GetEntityByKey(int key);
        T GetEntityByKey(int key, string[] includes);
        IQueryable<T> Get();
        IQueryable<T> Get(string[] includes);
        T CreateEntity(T entity);
        T UpdateEntity(int key, T update);
        void Delete(int key);
    }
}