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
        public IHttpActionResult PutObjective(int id, Objectives objective)
        {
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
        [ResponseType(typeof(Objectives))]
        public IHttpActionResult PostObjective(Objectives objective)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _dbObjectives.InsertObjective(objective);

            return CreatedAtRoute("DefaultApi", new { id = objective.Id }, objective);
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

        // GET: api/GetListOfObjectives
        [HttpGet] 
        public IHttpActionResult GetListOfObjectives(int userId, DateTime year)
        {
            var objectivesList = _dbObjectives.GetListOfObjectives(userId, year);
            return Ok(objectivesList);
        }


    }
}