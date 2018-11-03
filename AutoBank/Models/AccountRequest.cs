using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autobank.Models
{

    public class AccountRequest
    {
        public int AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}