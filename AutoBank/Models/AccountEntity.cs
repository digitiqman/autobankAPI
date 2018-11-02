using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Autobank.Models
{
    public class AccountEntity
    {
       
        public int Id { get; set; }
        public string AccountName { get; set; }

        public int AccountNumber { get; set; }

        public decimal Balance { get; set; }

        public string Currency { get; set; }
    }


    public class AccountResponse
    {
        public int AccountNumber { get; internal set; }
        public bool Successful { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public string Message { get; set; }
    }
}