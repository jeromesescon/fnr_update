using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FnR.WebApi.ViewModels
{
    public class VMSavePet
    {
        public string Name { get; set; }
        public int PetBreedId { get; set; }
        public DateTime Birthday { get; set; }
        public int UserId { get; set; }
        public double Weight { get; set; }
    }
}