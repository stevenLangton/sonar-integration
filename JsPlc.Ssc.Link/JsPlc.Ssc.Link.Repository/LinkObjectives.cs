using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Interfaces;

namespace JsPlc.Ssc.Link.Repository
{
    public class LinkObjectives : IObjectives
    {
        private readonly RepositoryContext _db;

        public LinkObjectives() { }

        public LinkObjectives(RepositoryContext context) { _db = context; }

        public Objectives GetObjective(int id)
        {
            Objectives objectives = _db.Objectives.Find(id);

            return objectives;
        }

        public bool UpdateObjective(int id, Objectives objectives)
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

        public bool InsertObjective(Objectives objectives)
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
                Objectives objectives = _db.Objectives.Find(id);
                _db.Objectives.Remove(objectives);
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Objectives> GetListOfObjectives(int userId, DateTime year)
        {
            return _db.Objectives.Where(e => e.EmployeeId == userId && e.CreatedDate.Year == year.Year);
        }

        /// <summary>
        /// Get all objectives for a colleague
        /// </summary>
        /// <param name="userId">The internal db id of a colleague (not the real life HR Colleague Id) </param>
        /// <returns>A list of objectives</returns>
        public IEnumerable<Objectives> GetAllObjectives(int userId)
        {
            return _db.Objectives.Where(e => e.EmployeeId == userId);
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
