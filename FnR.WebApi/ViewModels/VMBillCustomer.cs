using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FnR.WebApi.ViewModels
{
    public class VMBillCustomer
    {
        public long CustomerID { get; set; }
        public int Amount { get; set; }
        public string InvoiceReference { get; set; }
        public string InvoiceDescription { get; set; }
    }
}