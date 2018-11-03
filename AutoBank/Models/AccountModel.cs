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

        [Key]
        public int Id { get; set; }

        [Required]
        public string AccountName{ get; set; }

        [Required]
        [Index("uniqaccountnumber", IsUnique = true)]
        public int AccountNumber{ get; set; }

        [Required]
        [ConcurrencyCheck]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Balance{ get; set; }

        [Required, MinLength(3), MaxLength(3)]
        public string Currency { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public AccountResponse Deposit(AccountRequest account)
        {
            //try
            //{

            //    AccountResponse resp = accountDetails.Deposit(accountData);
            //    AccountResponse response = new AccountResponse
            //    {
            //        AccountNumber = accountDetails.AccountNumber,
            //        Successful = true,
            //        Balance = accountDetails.Balance,
            //        Currency = accountDetails.Currency,
            //        Message = "Account Details Retrieved Successfully."
            //    };
            //    return resp;

            //}
            //catch (AccountException ae)
            //{

            //    AccountResponse response = new AccountResponse
            //    {
            //        AccountNumber = account.AccountNumber,
            //        Successful = false,
            //        Message = ae.Message
            //    };
            //    return response;
            //}
            //catch (Exception ex)
            //{

            //    AccountResponse response = new AccountResponse
            //    {
            //        AccountNumber = account.AccountNumber,
            //        Successful = false,
            //        Message = ex.Message
            //    };
            //    return response;
            //}



            AccountResponse response = new AccountResponse
            {
                AccountNumber = account.AccountNumber,
                Successful = false,
                Message = "sasadasdasd"
            };
            return response;


        }

    }

}