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

        public Employee GetEmployee(string emailAddres)
        {
            //return _db.Employees.FirstOrDefault(e => e.EmailAddress.Equals(emailAddres,StringComparison.OrdinalIgnoreCase));
            return _db.Employees.FirstOrDefault(e =>e.EmailAddress.ToLower().Equals(emailAddres.ToLower()));
        }
        
        //check where login user is manager or not
        public bool IsManager(string userName)
        {
            var firstOrDefault = _db.Employees.FirstOrDefault(e => e.EmailAddress.ToLower().Equals(userName.ToLower()));

            //var firstOrDefault = _db.Employees.FirstOrDefault(e => e.EmailAddress.Equals(userName,StringComparison.InvariantCultureIgnoreCase));

            if (firstOrDefault == null) return false;

            var id = firstOrDefault.ColleagueId;

            var subEmployees = _db.Employees.Where(e => e.ManagerId == id);

            return subEmployees.Any();
        }

        // employees and their meeting history of a manager
        public IEnumerable<TeamView> GetTeam(string managerId)
        {
            var team = _db.Employees.Where(e => e.ManagerId == managerId);

            var teamView = new List<TeamView>();
            var teamMeeting = new Meeting();

            foreach (var employee in team)
            {
                teamView.Add(teamMeeting.GetMeetings(employee.ColleagueId));
            }
            return teamView;
        }
       
        public void Dispose()
        {
            _db.Dispose();
        }

        
    }
}
