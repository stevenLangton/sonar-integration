using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.Helpers;
using System;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

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

        [System.Web.Mvc.HttpPost]
        public async Task<HttpResponseMessage> PostLinkForm([FromBody]MeetingView meetingView)
        {
            #region 'Old approach = Receiving FormCollection' 
            // Get Json data in FormCollection formdata
            //var jsonData = formData["linkForm"];

            // Convert Json to LinkForm object
            // var newform = JsonConvert.DeserializeObject(jsonData, Type.GetType("JsPlc.Ssc.Link.Portal.Models.LinkForm")); // key set in .ajax POST on Create
            #endregion

         
            // POSTING Data further to ServiceApi
            // http://www.asp.net/web-api/overview/advanced/calling-a-web-api-from-a-net-client

            // validate the linkForm MeetingView and then post it back to Service Api
            var meetingViewJson = JsonConvert.SerializeObject(meetingView);

            if (ModelState.IsValid)
            {
                var response = await LinkServiceCaller.RunAsync(meetingViewJson);
                if (response.IsSuccessStatusCode)
                {
                    Uri meetingUrl = response.Headers.Location;
                    response.Content = new StringContent("Meeting saved at:" + meetingUrl);
                } // else may want to redirect to diff Url or set an error message etc
                return response;
            }
     
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(meetingViewJson)
            };
        }

        // GET: LinkForm/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LinkForm/Create
        public ActionResult Create()
        {
            ViewBag.Title = "Create Link Form"; 
            // assuming we're creating a new Link Meeting for now
            //LinkForm model = MockData.MockLinkForm();
            return View();//"LinkMeeting", model);
        }

        // POST: LinkForm/Create
        [System.Web.Mvc.HttpPost]
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
                return View();
            }
        }
    }
}
