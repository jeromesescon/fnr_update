using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FnR.Models.Interfaces;

namespace FnR.Models
{
    public class Condition : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("Vet")]
        public int VetId { get; set; }
        public Vet Vet { get; set; }
        public ICollection<Pet> Pets { get; set; }
        [ForeignKey("Schedule")]
        public int? ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
    }
}
