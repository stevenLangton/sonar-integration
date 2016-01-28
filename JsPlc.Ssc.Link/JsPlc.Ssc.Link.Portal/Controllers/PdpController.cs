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
    public class PdpController : LinkBaseController
    {
        // GET: LinkPdp
        public ActionResult Index()
        {
            LinkPdp Pdp;
            using (var facade = new LinkServiceFacade())
            {
                Pdp = facade.GetPdp(CurrentUser.Colleague.ColleagueId);
            }

            return View(Pdp);

        }


        [HttpPost]//Update
        public ActionResult Index(LinkPdp modifiedPdp)
        {


            LinkPdp Pdp;
                using (var facade = new LinkServiceFacade())
                {
                    modifiedPdp.ColleagueId = CurrentUser.Colleague.ColleagueId;
                    Pdp = facade.UpdatePdp(modifiedPdp).Result;
                }

                return View(Pdp);
           
        }


    }
}