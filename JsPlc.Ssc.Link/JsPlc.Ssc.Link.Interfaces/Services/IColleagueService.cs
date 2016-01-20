using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Interfaces.Services
{
    public interface IColleagueService
    {
        ColleagueView GetColleague(string colleagueId);

        ColleagueView GetColleagueByEmail(string emailAddress);

        List<ColleagueView> GetDirectReports(string managerId);

        List<ColleagueView> GetDirectReportsByManagerEmail(string emailAddress);

        bool IsManager(string colleagueId);

        bool IsManagerByEmail(string email);

        void Dispose();
    }
}
