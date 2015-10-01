using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JsPlc.Ssc.Link.Repository;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            using (var repo = new RepositoryContext())
            {
                var x = repo.Questions.ToList();
            }

            return View();
        }
    }
}
