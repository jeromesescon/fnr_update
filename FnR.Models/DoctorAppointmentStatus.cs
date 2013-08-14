using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FnR.Models.Interfaces;

namespace FnR.Models
{
    public class DoctorAppointmentStatus : IEntity
    {
        public int Id { get; set; }
        public int ExpectedWaitTime { get; set; }
        public int ExpectedDurationOfDelay { get; set; }
        //public ICollection<User> Users { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
