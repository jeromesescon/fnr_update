using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FnR.WebApi.ViewModels
{
    public class VMCCInfo
    {
        public string NameOnCard { get; set; }
        public string CCNumber { get; set; }
        public int CCExpMonth { get; set; }
        public int CCExpYear { get; set; }
    }
}