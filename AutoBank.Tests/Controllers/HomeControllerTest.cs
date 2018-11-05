using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoBank;
using AutoBank.Controllers;

namespace AutoBank.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            //Test the Landing Page of the AutoBanking App
            HomeController controller = new HomeController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual("Chilindo AutoBank", result.ViewBag.Title);
        }
    }
}
