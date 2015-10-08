using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.Helpers;
using JsPlc.Ssc.Link.Portal.Helpers.Extensions;
using Newtonsoft.Json;
using WebGrease.Css.Extensions;
using WebGrease.Extensions;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class LinkFormController : Controller
    {
        [ScriptMethod(UseHttpGet = true)]
        public JsonResult GetLinkForm(string employeeId, int periodId)
        {
            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ServicesBaseUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                HttpResponseMessage response =
                    client.GetAsync(String.Format("{0}/api/Meetings/?employeeId={1}&periodId={2}",
                    ConfigurationManager.AppSettings["ServicesBaseUrl"], employeeId, periodId)).Result;
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.result = response.Content.ReadAsAsync<MeetingView>().Result;
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
            //var linkForm = MockData.MockLinkForm();
            //linkForm.Questions = ViewBag.result;

            var jsonData = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = ViewBag.result
            };
            return jsonData;
        }


        [ScriptMethod(UseHttpGet = true)]
        public JsonResult GetMeetingView(int meetingId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response =
                    client.GetAsync(String.Format("{0}/api/meetings/{1}",
                        ConfigurationManager.AppSettings["ServicesBaseUrl"], meetingId)).Result;

                // Build a MeetingView Json response.
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.result = response.Content.ReadAsAsync<MeetingView>().Result;
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
            var jsonData = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = ViewBag.result // MeetingView
            };
            return jsonData;
        }

        [System.Web.Mvc.HttpPost]
        public async Task<JsonResult> PostLinkForm([FromBody] MeetingView meetingView)
        {
            #region 'Old approach = Receiving FormCollection' 

            // Get Json data in FormCollection formdata
            //var jsonData = formData["linkForm"];

            // Convert Json to LinkForm object
            // var newform = JsonConvert.DeserializeObject(jsonData, Type.GetType("JsPlc.Ssc.Link.Portal.Models.LinkForm")); // key set in .ajax POST on Create

            #endregion

            // POSTING Data further to ServiceApi
            // http://www.asp.net/web-api/overview/advanced/calling-a-web-api-from-a-net-client

            HttpResponseMessage response = null;
            // validate the linkForm MeetingView and then post it back to Service Api
            var meetingViewJson = JsonConvert.SerializeObject(meetingView);

            // Return ModelState errors in json 
            // http://stackoverflow.com/questions/2845852/asp-net-mvc-how-to-convert-modelstate-errors-to-json
            if (ModelState.IsValid)
            {
                response = await LinkServiceCaller.RunAsync(meetingViewJson);
                if (response.IsSuccessStatusCode)
                {
                    Uri meetingUrl = response.Headers.Location;
                    return response.ToJsonResult(null, null, "ApiSuccess");

                    // else may want to redirect to diff Url or set an error message etc
                }
                return response.ToJsonResult(null, null, "ApiFail");
            }

            var errorList = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            var errArray = new List<KeyValuePair<string, string[]>>();
            errorList.ForEach(kvp =>
            {
                if (kvp.Value.Any())
                {
                    errArray.Add(kvp);
                }
            });

            var badRequestResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };
            // key and string of arrays
            return badRequestResponse.ToJsonResult(null, errArray, "UIValidationErrors");

            //else
            //{
            //    return (ModelState);
            // or return Json(ModelState.Values.SelectMany(x => x.Errors));
            // or ModelState.Errors(); (extension method)
            //}

        }

        //// GET: LinkForm/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: LinkForm/Create
        public ActionResult Create(string employeeId, int? periodId)
        {
            ViewBag.Title = "Create Link Form"; 
            // assuming we're creating a new Link Meeting for now
            //LinkForm model = MockData.MockLinkForm();
            return View();//"LinkMeeting", model);
        }

        //// POST: LinkForm/Create
        //[System.Web.Mvc.HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("LinkForm");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: LinkForm/Edit/52
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: LinkForm/Edit/5
        [System.Web.Mvc.HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("LinkForm");
            }
            catch
            {
                return RedirectToAction("LinkForm");
                //return View();
            }
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult ViewMeeting(int? id)
        {
            ViewBag.Title = "My Team";

            return View();
        }

    }
}
