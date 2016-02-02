using System;
using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Service.Services
{
    public interface IStubServiceFacade: IDisposable
    {
        ColleagueView GetColleague(string colleagueId);
        ColleagueView GetColleagueByEmail(string email);
        IEnumerable<ColleagueView> GetDirectReports(string managerId);
        IEnumerable<ColleagueView> GetDirectReportsByManagerEmail(string managerEmail);
    }
}