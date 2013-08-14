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
    public class Subscription : IEntity
    {
        public Subscription()
        {
            this.DateSubscribed = Convert.ToDateTime("January 1, 1753");
            this.NextDeliveryDate = Convert.ToDateTime("January 1, 1753");
        }

        public int Id { get; set; }
        [ForeignKey("User")]
        [DisplayName("User")]
        [Required]
        public int? UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("Pet")]
        [DisplayName("Pet")]
        [Required]
        public int? PetId { get; set; }
        public Pet Pet { get; set; }
        [ForeignKey("Product")]
        [DisplayName("Product")]
        [Required]
        public int? ProductId { get; set; }
        public Product Product { get; set; }
        [ForeignKey("Vet")]
        [DisplayName("Vet")]
        [Required]
        public int? VetId { get; set; }
        public Vet Vet { get; set; }
        [DisplayName("Subscription Date")]
        public DateTime DateSubscribed { get; set; }
        [DisplayName("Next Delivery Date")]
        public DateTime NextDeliveryDate { get; set; }
        public bool Sent { get; set; }
        public string RebillID { get; set; }
    }
}
