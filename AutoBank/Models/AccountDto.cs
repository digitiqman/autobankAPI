using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AutoBank.Models
{
    public class AccountDto
    {
        public int Id { get; set; }

        [Required]
        public string AccountName { get; set; }

        [Required]
        [Index("uniqaccountnumber", IsUnique = true)]
        public int AccountNumber { get; set; }

        [Required, MinLength(3)]
        [DataType(DataType.Currency)]
        public string Currency { get; set; }

        [Required]
        public decimal Balance { get; set; }
    }
}