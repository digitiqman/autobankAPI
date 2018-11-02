using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoBank.Models
{
    [Table("Accounts")]
    public class AccountModel
    {
        
        [Key]
        public int Id { get; set; }
        public string AccountName{ get; set; }

        public int AccountNumber{ get; set; }

        public decimal Balance{ get; set; }

        public string Currency { get; set; }

    }

}