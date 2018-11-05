using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autobank.Models
{

    public class AccountResponse
    {
        public long AccountNumber { get; internal set; }
        public bool Successful { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public string Message { get; set; }
    }
}