using Autobank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutoBank.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext("DBConnectionString");
        public ActionResult Index()
        {

            //RETRIEVE ALL ACCOUNT DETAILS
            var allaccounts = db.Account.AsParallel().ToList();

            ViewBag.Title = "Chilindo AutoBank";
            ViewBag.AllAccounts = allaccounts;
            return View();
        }
    }
}
