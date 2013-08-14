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
    public class Product : IEntity
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Product Name")]
        public string Name { get; set; }
        [DisplayName("Product Description")]
        public string Description { get; set; }
        [Required]
        [DisplayName("Product Cost ($)")]
        public double? Cost { get; set; }
        [Required]
        [DisplayName("Actual Price ($)")]
        public double? Price { get; set; }
        [Required]
        [DisplayName("Amount of Product")]
        public int? Amount { get; set; }
        [ForeignKey("PetType")]
        [Required]
        [DisplayName("For Pet Type")]
        public int PetTypeId { get; set; }
        public PetType PetType { get; set; }
        [Required]
        [DisplayName("Lower Pet Weight Limit (Kg)")]
        public double? LowerWeightLimit { get; set; }
        [Required]
        [DisplayName("Higher Pet Weight Limit (Kg)")]
        public double? HeigherWeightLimit { get; set; }
        public string DisplayName { get { return Name + " ($" + Price + ")"; } }
        public ICollection<Vet> Vets { get; set; }
    }
}
