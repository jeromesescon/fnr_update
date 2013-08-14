using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FnR.Models.Interfaces;

namespace FnR.Models
{
    public class Breed : IEntity
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Breed Name")]
        public string Name { get; set; }
        // IDEAL Breed Weight in Kg
        [Required]
        [DisplayName("Ideal Weight (Kg)")]
        public double IdealWeight { get; set; }
        [ForeignKey("PetType")]
        [Required]
        [DisplayName("Pet Type")]
        public int? PetTypeId { get; set; }
        public PetType PetType { get; set; }
        public string DisplayName { get { return "(" + PetType.Name + ") " + Name + " - [" + IdealWeight + "Kg]"; } }
    }
}
