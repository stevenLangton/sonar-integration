using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Interfaces;
using JsPlc.Ssc.Link.Models.Entities;
using JsPlc.Ssc.Link.Repository;

namespace JsPlc.Ssc.Link.Service.Services
{
    public class ObjectivesService : IObjectivesService
    {
        private readonly RepositoryContext _db;

        public ObjectivesService() { }

        public ObjectivesService(RepositoryContext context) { _db = context; }

        public LinkObjective GetObjective(int id)
        {
            LinkObjective objectives = _db.Objectives.Find(id);

            return objectives;
        }

        public bool UpdateObjective(int id, LinkObjective objectives)
        {

            try
            {   
                _db.Entry(objectives).State = EntityState.Modified;
                _db.SaveChanges();
                return true;
            }
            catch
            {                
                return false;
            }

           
        }

        public bool InsertObjective(LinkObjective objectives)
        {
            try
            {   _db.Objectives.Add(objectives);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteObjective(int id)
        {
            try
            {
                LinkObjective objectives = _db.Objectives.Find(id);
                _db.Objectives.Remove(objectives);
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<LinkObjective> GetListOfObjectives(string colleagueId, DateTime year)
        {
            return _db.Objectives.Where(e => e.ColleagueId == colleagueId && e.CreatedDate.Year == year.Year);
        }

        /// <summary>
        /// Get all objectives for a colleague
        /// </summary>
        /// <param name="colleagueId"></param>
        /// <returns>A list of objectives</returns>
        public async Task<List<LinkObjective>> GetAllObjectives(string colleagueId)
        {
            return await _db.Objectives.Where(e => e.ColleagueId == colleagueId).ToListAsync<LinkObjective>();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
   
        private bool ObjectivesExists(int id)
        {
            return _db.Objectives.Count(e => e.Id == id) > 0;
        }
    }
}
