using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Portal.Helpers;
using JsPlc.Ssc.Link.Portal.Helpers.Api;
using JsPlc.Ssc.Link.Portal.Helpers.Extensions;
using JsPlc.Ssc.Link.Portal.Security;
using Newtonsoft.Json;
using WebGrease.Css.Extensions;
using WebGrease.Extensions;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class LinkFormController : Controller
    {

        public ActionResult MyTeam()
        {
            return View();
        }

        [ScriptMethod(UseHttpGet = true)]
        public JsonResult GetLinkForm(string colleagueId)
        {
            var facade = new LinkServiceFacade();

            object jsonData;
            var newMeeting = facade.GetNewMeetingView(colleagueId);
            
            if (newMeeting != null)
            {
                jsonData = newMeeting;
            }
            else
            {
                jsonData = "Error";
            }

            var jsonResult = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = jsonData
            };
            return jsonResult;
        }

        /// <summary>
        /// For a given MeetingId - useful for VIEW a Meeting or Edit a Meeting
        /// </summary>
        /// <param name="meetingId"></param>
        /// <returns></returns>
        [ScriptMethod(UseHttpGet = true)]
        public JsonResult GetMeetingView(int meetingId)
        {
            var facade = new LinkServiceFacade();

            object jsonData;
            var meeting = facade.GetMeeting(meetingId);

            if (meeting != null)
            {
                jsonData = meeting;
            }
            else
            {
                jsonData = "Error";
            }
            var jsonResult = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = jsonData // MeetingView
            };
            return jsonResult;
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
        }

        // GET: LinkForm/Create
        [System.Web.Mvc.Authorize]
        //[LinkAuthorizeManager] // might be needed for ManagerApproval method. Not needed here yet as Create can be called by Mgr or Employee.
        public ActionResult Create(string colleagueId)
        {
            ViewBag.Title = "Create Link Form";
            if (String.IsNullOrEmpty(colleagueId))
            {
                return RedirectToAction("Welcome", "Home");
            }
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
        [System.Web.Mvc.Authorize]
        public ActionResult ViewMeeting(int? id)
        {
            ViewBag.Title = "My Team";

            return View();
        }

    }
}
