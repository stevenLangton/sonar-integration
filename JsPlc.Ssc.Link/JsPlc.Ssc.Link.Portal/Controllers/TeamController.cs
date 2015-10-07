using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class TeamController : Controller
    {
        // GET: Team/Create
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Title = "My Team";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage response =
                    client.GetAsync(String.Format("{0}/api/Employees/?managerId={1}",
                    ConfigurationManager.AppSettings["ServicesBaseUrl"], "E0010")).Result;

                if (response.IsSuccessStatusCode)
                {
                    var team  = response.Content.ReadAsAsync<IEnumerable<TeamView>>().Result;
                    return View(team.ToList());
                }

            }
            return View();
        }
    }
}
