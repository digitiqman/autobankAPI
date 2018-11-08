using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoBank;
using Autobank.Controllers;
using System.Collections.Generic;
using AutoBank.Models;
using NUnit.Framework;
using System.Net.Http;
using System.Net;
using System.Web.Http.Results;
using Autobank.Models;
using System.Threading;
using System.Diagnostics;
using System.Web.Http;

namespace ChlindoApp.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {

        private long accounttoconsider = 1090209010;
        private string currencytoconsider = "usd";
        private int amounttoconsider = 10;

        //Test to ensure the response payload is as expected(Balance)
        [TestMethod]
        public void basictestbalanceresponse()
        {

            AccountController controller = new AccountController();
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new System.Uri("http://localhost/api/account/balance")
            };
            controller.Configuration = new System.Web.Http.HttpConfiguration(new System.Web.Http.HttpRouteCollection());

            var result = controller.Balance(1011101);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<AccountResponse>));
        }

        //Test to ensure the response payload is as expected for existing account (Balance)
        [TestMethod]
        public void existingaccountbalanceresponse()
        {
            
            AccountController controller = new AccountController();
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new System.Uri("http://localhost/api/account/balance")
            };
            controller.Configuration = new System.Web.Http.HttpConfiguration(new System.Web.Http.HttpRouteCollection());            

            var result = controller.Balance(accounttoconsider);
            var response = result.ExecuteAsync(CancellationToken.None).Result;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(response.IsSuccessStatusCode);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<AccountResponse>));
            var content = result as OkNegotiatedContentResult<AccountResponse>;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(true, content.Content.Successful);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(accounttoconsider, content.Content.AccountNumber);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("usd", content.Content.Currency);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual("thb", content.Content.Currency);
        }

        //Test to ensure the response payload is as expected when an account does not exist (Balance)
        [TestMethod]
        public void notexistingaccountbalance()
        {
            var accounttoconsider = 1011101;
            AccountController controller = new AccountController();
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new System.Uri("http://localhost/api/account/balance")
            };
            controller.Configuration = new System.Web.Http.HttpConfiguration(new System.Web.Http.HttpRouteCollection());

            var result = controller.Balance(accounttoconsider);
            var response = result.ExecuteAsync(CancellationToken.None).Result;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(response.IsSuccessStatusCode);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<AccountResponse>));
            var content = result as OkNegotiatedContentResult<AccountResponse>;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(false, content.Content.Successful);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(accounttoconsider, content.Content.AccountNumber);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("Account number " + accounttoconsider + " does not exist.", content.Content.Message);
            
        }


        //Test to ensure that the response payload is as expected for a non-existing account (Deposit)
        [TestMethod]
        public async System.Threading.Tasks.Task notexistingaccountdepositresponseAsync()
        {

            AccountRequest requestpayload = new AccountRequest();
            requestpayload.AccountNumber = accounttoconsider + 11; //This is an invalid account number
            requestpayload.Currency = currencytoconsider;
            requestpayload.Amount = amounttoconsider;

            AccountController controller = new AccountController();
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new System.Uri("http://localhost/api/account/deposit")
            };
            controller.Configuration = new System.Web.Http.HttpConfiguration(new System.Web.Http.HttpRouteCollection());

            IHttpActionResult result = await controller.Deposit(requestpayload);

            var response = result.ExecuteAsync(CancellationToken.None).Result;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(response.IsSuccessStatusCode);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<AccountResponse>));
            var content = result as OkNegotiatedContentResult<AccountResponse>;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(true, content.Content.Successful);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("Account with number " + requestpayload.AccountNumber + " and Currency denomination: " + requestpayload.Currency.ToUpper() + " does not exist", content.Content.Message);

        }


        //Test to ensure that the response payload is as expected for an existing account (Deposit)
        [TestMethod]
        public async System.Threading.Tasks.Task existingaccountdepositresponseAsync()
        {

            AccountRequest requestpayload = new AccountRequest();
            requestpayload.AccountNumber = accounttoconsider; 
            requestpayload.Currency = currencytoconsider;
            requestpayload.Amount = amounttoconsider;

            AccountController controller = new AccountController();
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new System.Uri("http://localhost/api/account/deposit")
            };
            controller.Configuration = new System.Web.Http.HttpConfiguration(new System.Web.Http.HttpRouteCollection());

            IHttpActionResult result = await controller.Deposit(requestpayload);

            var response = result.ExecuteAsync(CancellationToken.None).Result;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(response.IsSuccessStatusCode);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<AccountResponse>));
            var content = result as OkNegotiatedContentResult<AccountResponse>;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(true, content.Content.Successful);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("Account Details <i style='color:limegreen;font-weight:bolder'> Credited </i> Successfully.", content.Content.Message);

        }


        //Test to ensure that the response payload is as expected for a non-existing account (Withdraw)
        [TestMethod]
        public async System.Threading.Tasks.Task notexistingaccountwithdrawresponseAsync()
        {

            AccountRequest requestpayload = new AccountRequest();
            requestpayload.AccountNumber = accounttoconsider + 11; //This is an invalid account number
            requestpayload.Currency = currencytoconsider;
            requestpayload.Amount = amounttoconsider;

            AccountController controller = new AccountController();
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new System.Uri("http://localhost/api/account/withdraw")
            };
            controller.Configuration = new System.Web.Http.HttpConfiguration(new System.Web.Http.HttpRouteCollection());

            IHttpActionResult result = await controller.Withdraw(requestpayload);

            var response = result.ExecuteAsync(CancellationToken.None).Result;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(response.IsSuccessStatusCode);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<AccountResponse>));
            var content = result as OkNegotiatedContentResult<AccountResponse>;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(true, content.Content.Successful);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("Account with number " + requestpayload.AccountNumber + " and Currency denomination: " + requestpayload.Currency.ToUpper() + " does not exist", content.Content.Message);

        }


        //Test to ensure that the response payload is as expected for an existing account (WithDraw)
        [TestMethod]
        public async System.Threading.Tasks.Task existingaccountwithdrawresponseAsync()
        {

            AccountRequest requestpayload = new AccountRequest();
            requestpayload.AccountNumber = accounttoconsider;
            requestpayload.Currency = currencytoconsider;
            requestpayload.Amount = amounttoconsider;

            AccountController controller = new AccountController();
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new System.Uri("http://localhost/api/account/withdraw")
            };
            controller.Configuration = new System.Web.Http.HttpConfiguration(new System.Web.Http.HttpRouteCollection());

            IHttpActionResult result = await controller.Withdraw(requestpayload);

            var response = result.ExecuteAsync(CancellationToken.None).Result;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(response.IsSuccessStatusCode);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<AccountResponse>));
            var content = result as OkNegotiatedContentResult<AccountResponse>;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(true, content.Content.Successful);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("Account Details has been <i style='color:red;font-weight:bolder'> Debited </i> Successfully.", content.Content.Message);

        }


        //Test to ensure that the response payload is as expected for an existing account (On Overdraw- Withdrawl)
        [TestMethod]
        public async System.Threading.Tasks.Task accountoverwithdrawresponseAsync()
        {

            AccountRequest requestpayload = new AccountRequest();
            requestpayload.AccountNumber = accounttoconsider;
            requestpayload.Currency = currencytoconsider;
            requestpayload.Amount = amounttoconsider + 100000000;

            AccountController controller = new AccountController();
            controller.Request = new System.Net.Http.HttpRequestMessage
            {
                RequestUri = new System.Uri("http://localhost/api/account/withdraw")
            };
            controller.Configuration = new System.Web.Http.HttpConfiguration(new System.Web.Http.HttpRouteCollection());

            IHttpActionResult result = await controller.Withdraw(requestpayload);

            var response = result.ExecuteAsync(CancellationToken.None).Result;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(response.IsSuccessStatusCode);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<AccountResponse>));
            var content = result as OkNegotiatedContentResult<AccountResponse>;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(false, content.Content.Successful);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("Account cannot be debited, Cash overdraw not allowed!<br/> Attempted to withdraw: " + requestpayload.Amount + requestpayload.Currency.ToUpper(), content.Content.Message);

        }

    }
}
