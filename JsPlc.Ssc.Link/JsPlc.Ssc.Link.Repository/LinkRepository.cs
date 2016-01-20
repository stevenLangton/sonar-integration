using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Interfaces;

namespace JsPlc.Ssc.Link.Repository
{
    public class LinkRepository : ILinkRepository
    {
        private readonly RepositoryContext _db;

        public LinkRepository() { }

        public LinkRepository(RepositoryContext context) { _db = context; }

        public ColleagueView GetColleague(string emailAddress)
        {
            throw new NotImplementedException();
            // TODO - Call Stub API to get this.. Keep it transparent for Portal
        }

        // We will eventually call a Service to get Colleague data.. 
        public int? AppUserId(string colleagueId)
        {
            var linkUser = _db.LinkUsers.FirstOrDefault(e => e.ColleagueId.ToLower().Equals(colleagueId.ToLower()));
            if (linkUser != null)
            {
                return linkUser.Id;
            }
            return null;
        }

        //check where login user is manager or not
        public bool IsManager(string userName)
        {
            throw new NotImplementedException();
            // TODO call the StubApi for this. GetDirectReports and then return true if any..
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
