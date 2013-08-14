using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FnR.WebApi.ViewModels
{
    public class VMSubscription
    {
        public int UserId { get; set; }
        public int PetId { get; set; }
        public int ProductId { get; set; }
        public int VetId { get; set; }
        public string NameOnCard { get; set; }
        public string CCNumber { get; set; }
        public string CCExpMonth { get; set; }
        public string CCExpYear { get; set; }
    }
}