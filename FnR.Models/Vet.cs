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
    public class Vet : IEntity
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Vet Name")]
        public string Name { get; set; }
        public ICollection<Product> AvailableProducts { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ICollection<User> PetOwners { get; set; }
        public ICollection<Condition> Conditions { get; set; }
        //public ICollection<Schedule> Schedules { get; set; }
        public ICollection<Doctor> Doctors { get; set; }
    }
}
