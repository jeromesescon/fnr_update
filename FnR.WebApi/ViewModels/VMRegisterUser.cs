using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FnR.WebApi.ViewModels
{
    public class VMRegisterUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? VetId { get; set; }
        public string TokenCustomerID { get; set; }
        public string RebillCustomerID { get; set; }
        public string NameOnCard { get; set; }
        public string CCNumber { get; set; }
        public int CCExpMonth { get; set; }
        public int CCExpYear { get; set; }
    }
}