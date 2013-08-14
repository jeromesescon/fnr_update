using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FnR.Models.Interfaces;
using FnR.Models.Lookups;

namespace FnR.Models
{
    public class User : IEntity
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Full Name")]
        public string FullName { get { return LastName + ", " + FirstName; } }
        [Required]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Address { get; set; }
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
        public ICollection<Pet> Pets { get; set; }
        public Role Role { get; set; }
        public Status Status { get; set; }
        [ForeignKey("Vet")]
        public int? VetId { get; set; }
        public Vet Vet { get; set; }
        public string TokenCustomerID { get; set; }
        public string RebillCustomerID { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
    }
}
