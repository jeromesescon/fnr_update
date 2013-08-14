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
    public class ConditionRepository : Repository<Condition>
    {
        private readonly DbContext _context;
        public ConditionRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Condition> GetPetConditions(int petId)
        {
            return
                _context.Set<Pet>().Include(r => r.Conditions).SingleOrDefault(r => r.Id == petId).Conditions.ToList();
        }

        public IEnumerable<Condition> AddNewPetCondition(int petId, int conditionId)
        {
            var pet = _context.Set<Pet>().Include(r => r.Conditions).SingleOrDefault(r => r.Id == petId);
            if (pet.Conditions == null)
                pet.Conditions = new List<Condition>();
            var condition = _context.Set<Condition>().SingleOrDefault(r => r.Id == conditionId);
            pet.Conditions.Add(condition);
            _context.Entry(condition).State = EntityState.Modified;
            _context.Entry(pet).State = EntityState.Modified;
            _context.SaveChanges();
            return pet.Conditions.ToList();
        }

        public IEnumerable<Condition> RemovePetCondition(int petId, int conditionId)
        {
            var pet = _context.Set<Pet>().Include(r => r.Conditions).SingleOrDefault(r => r.Id == petId);
            pet.Conditions.Remove(pet.Conditions.SingleOrDefault(r => r.Id == conditionId));
            _context.Entry(pet).State = EntityState.Modified;
            _context.SaveChanges();
            return pet.Conditions.ToList();
        }
    }
}
