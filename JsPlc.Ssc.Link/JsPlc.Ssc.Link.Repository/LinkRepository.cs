using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Interfaces;

namespace JsPlc.Ssc.Link.Repository
{
    //public class LinkRepository : ILinkRepository
    //{
    //    private readonly RepositoryContext _db;
    //    private readonly IColleagueService _svc;

    //    public LinkRepository() { }

    //    public LinkRepository(RepositoryContext context) { _db = context; }

    //    public LinkRepository(IColleagueService svc) { _svc = svc; }

    //    //// We will eventually call a Service to get Colleague data.. 
    //    //public int? AppUserId(string colleagueId)
    //    //{
    //    //    var colleague = _svc.GetColleague(colleagueId);
    //    //    return linkUser;
    //    //}

    //    public void Dispose()
    //    {
    //        _db.Dispose();
    //    }

        
    //}
}
