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

        public void Dispose()
        {
            _db.Dispose();
        }

        
    }
}
