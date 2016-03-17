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
using JsPlc.Ssc.Link.Interfaces.Services;
using System.Threading.Tasks;

namespace JsPlc.Ssc.Link.Service.Controllers
{
    public class ObjectivesController : BaseController
    {
        public ObjectivesController() { }

        public ObjectivesController(IObjectivesService service) : base(service) { }//For testing

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
            return BadRequest();
        }

        // POST/Insert: api/Objectives
        //[ResponseType(typeof(ObjectiveAdd))]
        /// <summary>
        /// Add a new objective. Invoke by a POST verb
        /// </summary>
        /// <param name="newObjective"></param>
        /// <param name="colleagueId"></param>
        /// <returns>The url to the successfully created objective or error if not so.</returns>
        [Route("colleagues/{colleagueId}/objectives", Name="NewObjective")]
        public IHttpActionResult PostObjective(LinkObjective newObjective, string colleagueId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbObjectives.InsertObjective(newObjective);

            return CreatedAtRoute("ObjectiveUrl", new { id = newObjective.Id }, newObjective);
        }

        /// <summary>
        /// Updates an objective as we already have an id and url for it
        /// </summary>
        /// <param name="modObjective"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("colleagues/{colleagueId}/objectives/{id}", Name = "ObjectiveUrl")]
        public IHttpActionResult PutObjective(LinkObjective modObjective, int id)
        {
            if (ModelState.IsValid)
            {
                if (id != modObjective.Id)
                    return NotFound();

                var status = _dbObjectives.UpdateObjective(id, modObjective);

                if (status)
                {
                    return Ok();
                }
                return NotFound();
            }
            return BadRequest();
        }

        /// <summary>
        /// Return all objectives for a colleague
        /// </summary>
        /// <param name="colleagueId">The real life colleague id of a sainsburys employee</param>
        /// <returns>List of Objectives objects</returns>
        [Route("colleagues/{colleagueId}/objectives")]
        public async Task<IHttpActionResult> GetAllObjectives(string colleagueId)
        {
            List<LinkObjective> ObjectivesList = await _dbObjectives.GetAllObjectives(colleagueId);

            return Ok(ObjectivesList);
        }

        [Route("colleagues/{colleagueId}/objectives/shared")]
        public async Task<IHttpActionResult> GetSharedObjectives(string colleagueId)
        {
            List<LinkObjective> ObjectivesList = await _dbObjectives.GetSharedObjectives(colleagueId);

            return Ok(ObjectivesList);
        }

        // GET: api/GetListOfObjectives
        [HttpGet]
        public IHttpActionResult GetListOfObjectives(string colleagueId, DateTime year)
        {
            var objectivesList = _dbObjectives.GetListOfObjectives(colleagueId, year);
            return Ok(objectivesList);
        }

        /// <summary>
        /// Get a particular objective (objectiveId) belonging to a particular colleague (colleagueId)
        /// </summary>
        /// <param name="colleagueId"></param>
        /// <param name="objectiveId"></param>
        /// <returns>an object of type LinkObjective</returns>
        [Route("colleagues/{colleagueId}/objectives/{objectiveId}")]
        public IHttpActionResult GetObjective(string colleagueId, int objectiveId)
        {
            LinkObjective item = _dbObjectives.GetObjective(objectiveId);

            if (item == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(item);
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
            return false;
        }
    }
}