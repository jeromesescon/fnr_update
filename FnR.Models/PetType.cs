using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FnR.Models.Interfaces;

namespace FnR.Models
{
    public class PetType : IEntity
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Pet Type")]
        public string Name { get; set; }
        public ICollection<Breed> Breeds { get; set; }
    }
}
