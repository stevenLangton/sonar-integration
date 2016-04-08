using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;
using JsPlc.Ssc.Link.Portal.Controllers.Base;
using JsPlc.Ssc.Link.Portal.Properties;
using JsPlc.Ssc.Link.Portal.Security;
using log4net;
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

		[HttpGet]
		[TeamAccess]
		public async Task<ActionResult> GetObjectives(string ColleagueId)
		{
			var ObjectivesList = await ServiceFacade.GetObjectivesList(ColleagueId);

			return MakeJsonObject(ObjectivesList);
		}

		[HttpGet]
		[TeamAccess]
		public async Task<ActionResult> GetSharedObjectives(string ColleagueId)
		{
			var ObjectivesList = await ServiceFacade.GetSharedObjectives(ColleagueId);

			var jsonResult = new JsonResult
			{
				JsonRequestBehavior = JsonRequestBehavior.AllowGet,
				Data = ObjectivesList
			};

			return jsonResult;
		}

		[HttpGet]
		[TeamAccess]
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

        //[HttpGet]
        //public async Task<ActionResult> Show(int Id)
        //{
        //    //temporary solution to find out where this action has been requested from
        //    ILog _logger = LogManager.GetLogger("GlobalActionExecutedEx");
        //    _logger.Info(Request.RawUrl);
        //    _logger.Info(Request.UrlReferrer);
        //    _logger.Info(Request.UserHostAddress);

        //    ViewBag.Title = "View objective";
        //    LinkObjective item = await ServiceFacade.GetObjective(CurrentUser.Colleague.ColleagueId, Id);
        //    ViewBag.ReadOnly = item.ColleagueId != CurrentUser.Colleague.ColleagueId;
        //    return View(item);
        //}

		[HttpPost]
		public async Task<ActionResult> Create(LinkObjective modifiedObjective)
		{
			bool Success = false;

			if (ModelState.IsValid)
			{
                //Check authorization. User is only allowed to create his own objectives
                if (!AuthorizationService.IsUserData(CurrentUser.Colleague.ColleagueId, modifiedObjective)) {
				    return MakeJsonObject(null, false, @"You are only authorized to create your own objective");
                }

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

				return MakeJsonObject(errors, false, @"Validation errors found");
			}

			return MakeJsonObject(modifiedObjective, true, @"Success");

		}//Create

		[HttpPost]
		public async Task<ActionResult> Update(LinkObjective modifiedObjective)
		{
			bool Success = false;

			if (ModelState.IsValid)
			{
                //Check authorization. User is only allowed to create his own objectives
                if (!AuthorizationService.IsUserData(CurrentUser.Colleague.ColleagueId, modifiedObjective))
                {
                    return MakeJsonObject(null, false, @"You are only authorized to update your own objective");
                }

				modifiedObjective.LastAmendedBy = CurrentUser.Colleague.ColleagueId;
				modifiedObjective.LastAmendedDate = DateTime.Now;
				Success = await ServiceFacade.UpdateObjective(modifiedObjective);
			}

            return MakeJsonObject(modifiedObjective, true, @"Success");
		}
		//Update
	}
}