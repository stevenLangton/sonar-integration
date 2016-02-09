using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;

using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;
using JsPlc.Ssc.Link.Portal.Controllers.Base;

namespace JsPlc.Ssc.Link.Portal.Controllers
{
    [Authorize]
    public class PdpController : LinkBaseController
    {
        public PdpController(){}
        public PdpController(ILinkUserView CurrentUser, ILinkServiceFacade Facade) : base(CurrentUser, Facade) {}

        // GET: LinkPdp
        public ActionResult Index()
        {
            LinkPdp Pdp;

            Pdp = ServiceFacade.GetPdp(CurrentUser.Colleague.ColleagueId);

            return View(Pdp);
        }


        [HttpPost]//Update
        public ActionResult Index(LinkPdp modifiedPdp)
        {
            ViewBag.PdpUpdated = false;

            try
            {
                LinkPdp Pdp;

                modifiedPdp.ColleagueId = CurrentUser.Colleague.ColleagueId;
                Pdp = ServiceFacade.UpdatePdp(modifiedPdp).Result;

                ViewBag.PdpUpdated = true;
                return View(Pdp);
            }
            catch (Exception ex)
            {
                return new EmptyResult();
            }
        }

        [HttpGet]
        public JsonResult GetPdp(string ColleagueId)
        {
            var Pdp = ServiceFacade.GetPdp(ColleagueId);

            var jsonResult = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = Pdp
            };

            return jsonResult;
        }

    }
}