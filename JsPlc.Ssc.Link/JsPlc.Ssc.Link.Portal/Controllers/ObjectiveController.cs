using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;
using JsPlc.Ssc.Link.Portal.Controllers.Base;
using JsPlc.Ssc.Link.Portal.Properties;
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
        public ObjectiveController() { }
        public ObjectiveController(ILinkUserView CurrentUser, ILinkServiceFacade Facade)
            : base(CurrentUser, Facade)
        {
            
        }

        // GET: Objective
        public ActionResult Index()
        {
            TempData["tabName"] = "objective";
            ViewBag.Title = Resources.MyObjectives;
            ViewBag.ColleagueId = CurrentUser.Colleague.ColleagueId;
            return View();
        }

        public ActionResult GetAllColleagueObjectives(string ColleagueId)
        {
            //return GetObjectives(CurrentUser.Colleague.ColleagueId);
            return GetObjectives(ColleagueId);
        }

        [HttpGet]
        public ActionResult GetObjectives(string ColleagueId)
        {
            var ObjectivesList = ServiceFacade.GetObjectivesList(ColleagueId);

            var jsonResult = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = ObjectivesList
            };

            return jsonResult;
        }

        [HttpGet]
        public async Task<ActionResult> GetOneObjective(int ObjectiveId)
        {
            LinkObjective item = await ServiceFacade.GetObjective(CurrentUser.Colleague.ColleagueId, ObjectiveId);

            var jsonResult = new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = item
            };

            return jsonResult;
        }

        [HttpGet]
        public ActionResult New()
        {
            ViewBag.Title = "Add a new objective";
            LinkObjective item = new LinkObjective();
            item.LastAmendedBy = item.ColleagueId = CurrentUser.Colleague.ColleagueId;
            item.LastAmendedDate = item.CreatedDate = DateTime.Now;
            item.ManagerId = CurrentUser.Colleague.ManagerId;

            return View("Show", item);
        }

        [HttpGet]
        public async Task<ActionResult> Show(int Id)
        {
            ViewBag.Title = "View objective";
            LinkObjective item = await ServiceFacade.GetObjective(CurrentUser.Colleague.ColleagueId, Id);
            ViewBag.ReadOnly = item.ColleagueId != CurrentUser.Colleague.ColleagueId;
            return View(item);
        }

        [HttpPost]
        public async Task<ActionResult> Create(LinkObjective modifiedObjective)
        {
            bool Success = false;

            if (ModelState.IsValid)
            {
                modifiedObjective.LastAmendedBy = CurrentUser.Colleague.ColleagueId;
                modifiedObjective.LastAmendedDate = DateTime.Now;

                //Add new item
                int NewObjectId = await ServiceFacade.CreateObjective(modifiedObjective);
                Success = NewObjectId != 0;
                if (Success)
                {
                    modifiedObjective.Id = NewObjectId;
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                       .Where(y => y.Count > 0)
                       .ToList();
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
                modifiedObjective.LastAmendedBy = CurrentUser.Colleague.ColleagueId;
                modifiedObjective.LastAmendedDate = DateTime.Now;
                Success = ServiceFacade.UpdateObjective(modifiedObjective).Result;
            }

            return new JsonResult {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { success = Success }
            };
        }//Update
    }
}