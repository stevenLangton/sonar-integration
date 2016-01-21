using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;
using JsPlc.Ssc.Link.Portal.Controllers.Base;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
//using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    [Authorize]
    public class ObjectiveController : LinkBaseController
    {
        // GET: Objective
        public ActionResult Index()
        {
            return View();
        }


        public async Task<JsonResult> GetAllColleagueObjectives(string ColleagueId)
        {
            Uri redirectUri = new Uri(postLogoutRedirectUri);
            Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext authContext = new AuthenticationContext(Authority);

            //AuthenticationResult authResult = authContext.AcquireToken(LinkApiResourceId, clientId, redirectUri);

            HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
            HttpResponseMessage response = await client.GetAsync(LinkApiBaseAddress + "/colleagues/" + ColleagueId + "/objectives");

            var ObjectivesList = await response.Content.ReadAsAsync<List<LinkObjective>>();

            var jsonResult = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = ObjectivesList
            };

            return jsonResult;
        }

        [HttpGet]
        public ActionResult Objective(int Id)
        {
            return View();
        }

    }
}