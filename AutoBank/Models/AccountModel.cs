using Autobank.Models;
using AutoBank.Helpers;
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

        private ApplicationDbContext db = new ApplicationDbContext("DBConnectionString");
        public bool wasWithdrawn;

        [Key]
        public int Id { get; set; }

        [Required]
        public string AccountName{ get; set; }

        [Required]
        [Index("uniqaccountnumber", IsUnique = true)]
        public long AccountNumber{ get; set; }

        [Required]
        [ConcurrencyCheck]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Balance{ get; set; }

        [Required, MinLength(3), MaxLength(3)]
        public string Currency { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public void Deposit(decimal amount)
        {
            this.Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            wasWithdrawn = false;
            if (this.Balance < amount)
                return;
            this.Balance -= amount;
            wasWithdrawn = true;
        }

    }

}