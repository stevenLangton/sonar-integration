﻿using System.Configuration;
using System.Drawing;
using System.Web.Helpers;
using System.Web.Script.Services;
using System.Web.Services.Description;
using JsPlc.Ssc.Link.Portal.Models.MockData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net.Http;
using System.Net.Http.Headers;

using JsPlc.Ssc.Link.Portal.Models;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class LinkFormController : Controller
    {
        [ScriptMethod(UseHttpGet = true)]
        public JsonResult GetLinkForm(int periodId)
        {
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ServicesBaseUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                HttpResponseMessage response =
                    client.GetAsync(String.Format("{0}/api/questions/?periodId={1}", 
                    ConfigurationManager.AppSettings["ServicesBaseUrl"],periodId)).Result;
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.result = response.Content.ReadAsAsync<IEnumerable<Question>>().Result;
                }
                else
                {
                    return new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = "Error"
                    };
                }
            }
            var linkForm = MockData.MockLinkForm();
            linkForm.Questions = ViewBag.result;

            var jsonData = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = linkForm
            };
            return jsonData;
        }

        // GET: LinkForm/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LinkForm/Create
        public ActionResult Create()
        {
            // assuming we're creating a new Link Meeting for now
            //LinkForm model = MockData.MockLinkForm();
            return View();//"LinkMeeting", model);
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
