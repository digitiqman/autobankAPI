using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using AutoBank.Helpers;
using Autobank.Models;
using Newtonsoft.Json;
using AutoBank.Models;

namespace Autobank.Controllers
{
    [System.Web.Http.RoutePrefix("api/account")]
    //[System.Web.Http.Route("api/[controller]/[action]")]
    public class AccountController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext("DBConnectionString");
        LogWriter logwriter = new LogWriter();

        [System.Web.Http.HttpGet]
        [ResponseType(typeof(AccountResponse))]
        //[System.Web.Http.Route("balance/{accountNumber}")]
        [System.Web.Http.Route("balance")]
        public IHttpActionResult Balance([FromUri] long accountNumber = -1)
        {
        
            if (!this.ModelState.IsValid || accountNumber == -1)
            {
                ModelState.AddModelError("CorrectionTip", "/?AccountNumber={AccountNumber:Integer}");
                return this.BadRequest(this.ModelState);
            }

            try
            {
                var accountDetails = db.Account.Where(a => a.AccountNumber == accountNumber).FirstOrDefault();
                if (accountDetails == null)
                {
                    throw new AccountException("Account number " + accountNumber + " does not exist.");
                }

                AccountResponse response = new AccountResponse
                {
                    AccountNumber = accountDetails.AccountNumber,
                    Successful = true,
                    Balance = accountDetails.Balance,
                    Currency = accountDetails.Currency,
                    Message = "Account Details Retrieved Successfully."
                };

                return Ok(response);

            }
            catch (AccountException ae)
            {

                AccountResponse response = new AccountResponse
                {
                    AccountNumber = accountNumber,
                    Successful = false,
                    Message = ae.Message
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

        }

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(AccountResponse))]
        [System.Web.Http.Route("deposit")]
        public async Task<IHttpActionResult> Deposit([FromBody]AccountRequest accountData)
        {
            if (!this.ModelState.IsValid)
            {
                ModelState.AddModelError("CorrectionTip", "AccountNumber:Integer, Amount:Integer, Currency:Sting(3)");
                return this.BadRequest(this.ModelState);
            }
            using (db) { 
                AccountModel accountDetails = await db.Account.Where(a => a.AccountNumber == accountData.AccountNumber && a.Currency == accountData.Currency).FirstOrDefaultAsync();            
                bool failedSave;
                do{
                    failedSave = false;
                    try
                    {
                        if (accountDetails == null)
                        {
                            throw new AccountException("Account with number " + accountData.AccountNumber + " and Currency denomination: " + accountData.Currency.ToUpper() + " does not exist");
                        }
                        logwriter.WriteTolog("Before Deposit, Balance is " + accountDetails.Balance + ". Account Number: " + accountDetails.AccountNumber + ". Account Currency: " + accountDetails.Currency);
                        accountDetails.Deposit(accountData.Amount);
                        await db.SaveChangesAsync();
                        logwriter.WriteTolog("After Deposit, Balance is " + accountDetails.Balance + ". Account Number: " + accountDetails.AccountNumber + ". Account Currency: " + accountDetails.Currency);
                        AccountResponse response = new AccountResponse
                        {
                            AccountNumber = accountDetails.AccountNumber,
                            Successful = true,
                            Balance = accountDetails.Balance,
                            Currency = accountDetails.Currency,
                            Message = "Account Details <i style='color:limegreen;font-weight:bolder'> Credited </i> Successfully."
                        };
                        return Ok(response);
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        failedSave = true;
                        var dataentry = ex.Entries.Single();
                        var dbEntry = dataentry.GetDatabaseValues();
                        //Perhaps account details has been deleted.
                        if (dbEntry == null)
                        {
                            throw new AccountException("Details for Account number " + accountDetails.AccountNumber + " does not exist.");
                        }
                        //replace the model with refresh data from the database
                        dataentry.OriginalValues.SetValues(dbEntry);
                        logwriter.WriteTolog("Account Balance has changed. Database Concurrency Occured  on Deposit");
                    }
                    catch (AccountException ae)
                    {
                        AccountResponse response = new AccountResponse
                        {
                            AccountNumber = accountData.AccountNumber,
                            Successful = false,
                            Message = ae.Message
                        };
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError();
                    }                
                } while (failedSave);

                return Ok();
            }
        }        

        [System.Web.Http.HttpPost]
        [ResponseType(typeof(AccountResponse))]
        [System.Web.Http.Route("withdraw")]
        public async Task<IHttpActionResult> Withdraw([FromBody]AccountRequest accountData)
        {
            if (!this.ModelState.IsValid)
            {
                ModelState.AddModelError("CorrectionTip", "AccountNumber:Integer, Amount:Integer, Currency:Sting(3)");
                return this.BadRequest(this.ModelState);
            }
            using (db) { 
                AccountModel accountDetails = await db.Account.Where(a => a.AccountNumber == accountData.AccountNumber && a.Currency == accountData.Currency).FirstOrDefaultAsync();            
                bool failedSave;
                do{
                    failedSave = false;
                    try
                    {
                        if (accountDetails == null)
                        {
                            throw new AccountException("Account with number " + accountData.AccountNumber + " and Currency denomination: " + accountData.Currency.ToUpper() + " does not exist");
                        }
                        logwriter.WriteTolog("Before Withdrawal, Balance is " + accountDetails.Balance + ". Account Number: " + accountDetails.AccountNumber + ". Account Currency: " + accountDetails.Currency);
                        accountDetails.Withdraw(accountData.Amount);
                        await db.SaveChangesAsync();
                        AccountResponse response = new AccountResponse
                        {
                            AccountNumber = accountDetails.AccountNumber,
                            Successful = accountDetails.wasWithdrawn ? true:false,
                            Balance = accountDetails.Balance,
                            Currency = accountDetails.Currency,
                            Message = accountDetails.wasWithdrawn ? "Account Details has been  <i style='color:red;font-weight:bolder'> Debited </i> Successfully." : "Account cannot be debited, Cash overdraw not allowed!<br/> Attempted to withdraw: " +accountData.Amount + accountData.Currency.ToUpper()
                        };
                        logwriter.WriteTolog("After Withdrawal, Balance is " + accountDetails.Balance + ". Account Number: " + accountDetails.AccountNumber + ". Account Currency: " + accountDetails.Currency);
                        return Ok(response);
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        failedSave = true;
                        var dataentry = ex.Entries.Single();
                        var dbEntry = dataentry.GetDatabaseValues();
                        //Perhaps account details has been deleted.
                        if (dbEntry == null)
                        {
                            throw new AccountException("Details for Account number " + accountDetails.AccountNumber + " does not exist.");
                        }
                        //replace the model with refresh data from the database
                        dataentry.OriginalValues.SetValues(dbEntry);
                        logwriter.WriteTolog("Account Balance has changed. Database Concurrency Occured");
                    }
                    catch (AccountException ae)
                    {
                        AccountResponse response = new AccountResponse
                        {
                            AccountNumber = accountData.AccountNumber,
                            Successful = false,
                            Message = ae.Message
                        };
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        return InternalServerError();
                    }                
                } while (failedSave);

                return Ok();
            }
        }                

    }
}