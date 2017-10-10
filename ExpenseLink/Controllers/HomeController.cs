using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpenseLink.Models;

namespace ExpenseLink.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        private IList<UserInterest> GetUserInterests()
        {
            List<UserInterest> userInterests = new List<UserInterest>();

            UserInterest userInterest = new UserInterest
            {
                Id = 1,
                InterestText = "Basketball",
                IsExperienced = true
            };
            userInterests.Add(userInterest);
            return userInterests;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}