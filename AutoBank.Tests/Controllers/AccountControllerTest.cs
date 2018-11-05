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

namespace ChlindoApp.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {

        [TestMethod]
        public void Index()
        {

            // Arrange
            AccountController controller = new AccountController();   

        }


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


        [TestMethod]
        public void accnotfoundtestbalance()
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
        }
    }
}
