using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using FnR.Models.Interfaces;

namespace FnR.Models
{
    public class Pet : IEntity
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Pet Name")]
        public string Name { get; set; }
        //[ForeignKey("PetType")]
        //[Required]
        //[DisplayName("Pet Type")]
        //public int? PetTypeId { get; set; }
        //public PetType PetType { get; set; }
        [ForeignKey("Breed")]
        [Required]
        [DisplayName("Pet Breed")]
        [DataMember(IsRequired = true)]
        public int? BreedId { get; set; }
        public Breed Breed { get; set; }
        public DateTime Birthday { get; set; }
        [ForeignKey("User")]
        [Required]
        [DisplayName("Pet Owner")]
        public int? UserId { get; set; }
        public User User { get; set; }
        [DisplayName("Weight (Kg)")]
        public double? Weight { get; set; }
        public ICollection<Condition> Conditions { get; set; }
    }
}
