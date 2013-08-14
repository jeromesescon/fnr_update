using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FnR.WebApi.ViewModels
{
    public class VMCreateCustomer
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CCNumber { get; set; }
        public string CCNameOnCard { get; set; }
        public int CCExpiryMonth { get; set; }
        public int CCExpiryYear { get; set; }
    }
}