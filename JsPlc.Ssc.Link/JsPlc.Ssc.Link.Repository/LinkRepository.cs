using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Interfaces;

namespace JsPlc.Ssc.Link.Repository
{
    public class LinkRepository:ILinkRepository
    {
        private readonly RepositoryContext _db;

        public LinkRepository() { }

        public LinkRepository(RepositoryContext context) { _db = context; }

        public ColleagueView GetColleague(string emailAddres)
        {
            throw new NotImplementedException();
            // TODO - Call Stub API to get this.. Keep it transparent for Portal
            ////return _db.Employees.FirstOrDefault(e => e.EmailAddress.Equals(emailAddres,StringComparison.OrdinalIgnoreCase));
            //return _db.Employees.FirstOrDefault(e =>e.EmailAddress.ToLower().Equals(emailAddres.ToLower()));
        }

        // We will eventually call a Service to get Colleague data.. 
        public int AppUserId(string colleagueId)
        {
            //return _db.Employees.FirstOrDefault(e => e.EmailAddress.Equals(emailAddres,StringComparison.OrdinalIgnoreCase));
            return _db.LinkUsers.FirstOrDefault(e => e.ColleagueId.ToLower().Equals(colleagueId.ToLower())).Id;
        }
        
        //check where login user is manager or not
        public bool IsManager(string userName)
        {
            throw new NotImplementedException();
            // TODO call the StubApi for this.
            //var firstOrDefault = _db.Employees.FirstOrDefault(e => e.EmailAddress.ToLower().Equals(userName.ToLower()));

            ////var firstOrDefault = _db.Employees.FirstOrDefault(e => e.EmailAddress.Equals(userName,StringComparison.InvariantCultureIgnoreCase));

            //if (firstOrDefault == null) return false;

            //var id = firstOrDefault.ColleagueId;

            //var subEmployees = _db.Employees.Where(e => e.ManagerId == id);

            //return subEmployees.Any();
        }

       
       
        public void Dispose()
        {
            _db.Dispose();
        }

        
    }
}
