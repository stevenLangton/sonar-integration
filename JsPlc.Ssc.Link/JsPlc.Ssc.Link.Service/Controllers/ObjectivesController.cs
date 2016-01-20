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
        public IHttpActionResult PutObjective(int id, LinkObjective objective)
        {
            LinkObjective newObjective = new LinkObjective();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != objective.Id)
            {
                return BadRequest();
            }

            if (_dbObjectives.UpdateObjective(id, objective))
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
            LinkObjective newObjective = new LinkObjective();

            newObjective.EmployeeId = colleagueId;
            newObjective.Objective = objective.Objective;
            newObjective.CreatedDate = DateTime.Now.Date;
            newObjective.LastAmendedDate = DateTime.Now.Date;
            newObjective.LastAmendedBy = objective.LastAmendedByColleagueId;

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
            List<LinkObjective> ObjectivesList = _dbObjectives.GetAllObjectives(colleagueId).ToList<LinkObjective>();

            return Ok(ObjectivesList);
        }

        // GET: api/GetListOfObjectives
        [HttpGet]
        public IHttpActionResult GetListOfObjectives(string colleagueId, DateTime year)
        {
            var objectivesList = _dbObjectives.GetListOfObjectives(colleagueId, year);
            return Ok(objectivesList);
        }

        [Route("colleagues/{colleagueId}/objectives/{objectiveId}")]
        public HttpResponseMessage GetObjective(string colleagueId, int objectiveId)
        {
            LinkObjective item = _dbObjectives.GetObjective(objectiveId);

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
        [ResponseType(typeof(LinkObjective))]
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