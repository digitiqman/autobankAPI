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

namespace Autobank.Controllers
{
    [System.Web.Http.RoutePrefix("api/account")]
    //[System.Web.Http.Route("api/[controller]/[action]")]
    public class AccountController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext("DBConnectionString");

        [System.Web.Http.HttpGet]
        [ResponseType(typeof(AccountResponse))]
        [System.Web.Http.Route("Getbalance/{accountNumber}")]
        public IHttpActionResult GetBalance(int accountNumber)
        {

            try
            {
                var accountDetails = db.Account.Where(a => a.AccountNumber == accountNumber).FirstOrDefault();
                if (accountDetails == null)
                {
                    throw new AccountException("Invalid Account Number");
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

        //[HttpPost]
        //[ResponseType(typeof(AccountModel))]
        //public async Task<IHttpActionResult> Deposit(int id)
        //{
        //    AccountModel accountDto = await db.Account.FindAsync(id);
        //    if (accountDto == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(accountDto);
        //}

        //[HttpPost]
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> Withdraw([FromBody] AccountDto accountDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != accountDto.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(accountDto).State = System.Data.Entity.EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AccountModelExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}        

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool AccountModelExists(int accountNumber)
        //{
        //    return db.Account.Count(e => e.AccountNumber == accountNumber) > 0;
        //}

    }
}