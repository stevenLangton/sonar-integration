using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;
using JsPlc.Ssc.Link.Service.Models;
using JsPlc.Ssc.Link.Repository;
using JsPlc.Ssc.Link.Interfaces;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class ObjectivesController : BaseController
    {
        //public ObjectivesController() { }

        //public ObjectivesController(ILinkRepository repository) : base(repository) { }

        // PUT/Update: api/Objectives/5
        [Route("Objectives")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutObjective(string EmployeeId, Objectives objective)
        {
            int UserId = _db.AppUserId(EmployeeId).GetValueOrDefault();

            Objectives newObjective = new Objectives();


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (UserId != objective.Id)
            {
                return BadRequest();
            }

            if (_dbObjectives.UpdateObjective(UserId, objective))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return BadRequest();
            }

        }

        // POST/Insert: api/Objectives
        [ResponseType(typeof(ObjectiveAdd))]
        [Route("colleagues/{colleagueId}/objectives", Name="NewObjective")]
        public IHttpActionResult PostObjective(ObjectiveAdd objective, string colleagueId)
        {
            int UserId = _db.AppUserId(colleagueId).GetValueOrDefault();

            Objectives newObjective = new Objectives();

            newObjective.EmployeeId = UserId;
            newObjective.Objective = objective.Objective;
            newObjective.CreatedDate = DateTime.Now.Date;
            newObjective.LastAmendedDate = DateTime.Now.Date;
            newObjective.LastAmendedBy = _db.AppUserId(objective.LastAmendedByColleagueId).GetValueOrDefault();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbObjectives.InsertObjective(newObjective);

            return CreatedAtRoute("NewObjective", new { id = newObjective.Id }, newObjective);
        }

        /// <summary>
        /// Return all objectives for a colleague
        /// </summary>
        /// <param name="colleagueId">The real life colleague id of a sainsburys employee</param>
        /// <returns>List of Objectives objects</returns>
        [Route("colleagues/{colleagueId}/objectives")]
        public IHttpActionResult GetAllObjectives(string colleagueId)
        {
            int UserId = _db.AppUserId(colleagueId).GetValueOrDefault();

            List<Objectives> ObjectivesList = _dbObjectives.GetAllObjectives(UserId).ToList<Objectives>();

            return Ok(ObjectivesList);
        }

        // GET: api/GetListOfObjectives
        [HttpGet]
        public IHttpActionResult GetListOfObjectives(int userId, DateTime year)
        {
            var objectivesList = _dbObjectives.GetListOfObjectives(userId, year);
            return Ok(objectivesList);
        }

        [Route("colleagues/{colleagueId}/objectives/{objectiveId}")]
        public HttpResponseMessage GetObjective(string colleagueId, int objectiveId)
        {
            Objectives item = _dbObjectives.GetObjective(objectiveId);

            if (item == null)
            {
                var message = string.Format("No objective with id = {0} found", objectiveId);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
        }

        // DELETE: api/Objectives/5
        [ResponseType(typeof(Objectives))]
        public bool DeleteObjective(int id)
        {
            if(_dbObjectives.DeleteObjective(id))
            {
                return true;
            }
            else
            {
                return false;
            } 
        }




    }
}