using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;
using JsPlc.Ssc.Link.Portal.Controllers.Base;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public ActionResult GetAllColleagueObjectives()
        {
            return GetObjectives(CurrentUser.Colleague.ColleagueId);
        }

        [HttpGet]
        public ActionResult GetObjectives(string ColleagueId)
        {
            using (var facade = new LinkServiceFacade())
            {
                var ObjectivesList = facade.GetObjectivesList(ColleagueId);

                var jsonResult = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = ObjectivesList
                };

                return jsonResult;
            }
        }

        [HttpGet]
        public ActionResult New()
        {
            LinkObjective item = new LinkObjective();
            item.LastAmendedBy = item.ColleagueId = CurrentUser.Colleague.ColleagueId;
            item.LastAmendedDate = item.CreatedDate = DateTime.Now;
            item.ManagerId = CurrentUser.Colleague.ManagerId;

            return View("Show", item);
        }

        [HttpGet]
        public ActionResult Show(int Id)
        {
            using (var facade = new LinkServiceFacade())
            {
                LinkObjective item = facade.GetObjective(CurrentUser.Colleague.ColleagueId, Id);

                return View(item);
            }
        }

        [HttpPost]
        public ActionResult Create(LinkObjective modifiedObjective)
        {
            bool Success = false;

            if (ModelState.IsValid)
            {
                using (var facade = new LinkServiceFacade())
                {
                    modifiedObjective.LastAmendedBy = CurrentUser.Colleague.ColleagueId;
                    modifiedObjective.LastAmendedDate = DateTime.Now;

                    //Add new item
                    int NewObjectId = facade.CreateObjective(modifiedObjective).Result;
                    Success = NewObjectId != 0;
                    if (Success)
                    {
                        modifiedObjective.Id = NewObjectId;
                    }
                }
            }

            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { success = Success }
            };
        }//Create

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

            return new JsonResult {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { success = Success }
            };
        }//Update
    }
}