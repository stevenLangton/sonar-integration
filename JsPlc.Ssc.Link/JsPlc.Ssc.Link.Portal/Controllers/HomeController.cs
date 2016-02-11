﻿using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using JsPlc.Ssc.Link.Portal.Controllers.Base;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class HomeController : LinkBaseController
    {
        public ActionResult Index()
        {
            TempData["IPAddress"] = GetIPAddress();
            return View("Welcome");
        }

        // GET: /Home/LinkMeetings/
        // ## COLLEAGUE's meetings
        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.Authorize]
        public ActionResult LinkMeetings()
        {
            TempData["tabName"] = "meetings";
            TempData["ViewType"] = "MyMeetings";
            return View("LinkMeetings");
        }

        public ActionResult Contact()
        {
            TempData["tabName"] = "Contact";
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Welcome()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult JsVars()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Unauthorized([FromUri]string unauthPage="")
        {
            ViewBag.unauthPage = unauthPage;
            return View();
        }

        public ActionResult ShowClaims()
        {
            List<string> claims = GetAllClaims(User.Identity);
            return View(claims);
        }
        public ActionResult ShowKeys()
        {
            var keys = ConfigurationManager.AppSettings.AllKeys;
            List<string> keyValues= keys.Select(key => string.Format("Key: {0}, Value: {1}", 
                key, ConfigurationManager.AppSettings[key])).ToList();

            List<string> serviceKeyValues = new List<string>();
            using (var facade = new LinkServiceFacade())
            {
                serviceKeyValues = facade.GetApiServiceKeys();
            }
            keyValues.Add("=========== Service layer Keys ==============");
            keyValues.AddRange(serviceKeyValues);

            return View("ShowClaims", keyValues);
        }
    }
}