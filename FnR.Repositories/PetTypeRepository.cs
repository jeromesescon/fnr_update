using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FnR.Models;

namespace FnR.Repositories
{
    public class PetTypeRepository : Repository<PetType>
    {
        public PetTypeRepository(DbContext context) : base(context)
        {
        }
    }
}
