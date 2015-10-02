namespace JsPlc.Ssc.Link.Portal.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using System.Net.Http;
    using System.Net.Http.Headers;

    using JsPlc.Ssc.Link.Portal.Models;

    public class LinkFormController : Controller
    {
   

        // GET: LinkForm/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LinkForm/Create
        public ActionResult Create()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:35915/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                HttpResponseMessage response = client.GetAsync("api/questions/1").Result;
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.result = response.Content.ReadAsAsync<IEnumerable<Question>>().Result;
                }
                else
                {
                    ViewBag.result = "Error, Unable to connect to service.";
                }
            }



            return View();
        }

        // POST: LinkForm/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("LinkForm");
            }
            catch
            {
                return View();
            }
        }

        // GET: LinkForm/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LinkForm/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("LinkForm");
            }
            catch
            {
                return View();
            }
        }

      

       
    }
}
