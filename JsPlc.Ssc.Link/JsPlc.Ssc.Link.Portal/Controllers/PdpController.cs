using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    public class PdpController : Controller
    {
        // GET: LinkPdp
        public ActionResult Index()
        {
            return View();
        }


        public async Task<JsonResult> GetAllColleagueObjectives()
        {
            Uri redirectUri = new Uri(postLogoutRedirectUri);
            Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext authContext = new AuthenticationContext(Authority);

            //AuthenticationResult authResult = authContext.AcquireToken(LinkApiResourceId, clientId, redirectUri);

            HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
            HttpResponseMessage response = await client.GetAsync(LinkApiBaseAddress + "/colleagues/" + CurrentUser.Colleague.ColleagueId + "/objectives");

            var ObjectivesList = await response.Content.ReadAsAsync<List<LinkObjective>>();

            var jsonResult = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = ObjectivesList
            };

            return jsonResult;
        }

        [HttpPost]
        public ActionResult Update(LinkObjective modifiedObjective)
        {
            bool Success = false;

            if (ModelState.IsValid)
            {
                using (var facade = new LinkServiceFacade())
                {
                    modifiedObjective.LastAmendedBy = CurrentUser.Colleague.ColleagueId;
                    modifiedObjective.LastAmendedDate = DateTime.Now;
                    Success = facade.UpdateObjective(modifiedObjective).Result;
                }
            }

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { success = Success }
            };
        }//Update
    }
}