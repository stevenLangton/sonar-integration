using System.Collections.Generic;
using JsPlc.Ssc.Link.StubService.StubModels;

namespace JsPlc.Ssc.Link.StubService.StubInterfaces
{
    public interface IColleagueServices
    {
        ColleagueDto GetColleague(string colleagueId);
        ColleagueDto GetColleagueByEmail(string emailAddress);

        List<ColleagueDto> GetDirectReports(string managerId);
        List<ColleagueDto> GetDirectReportsByManagerEmail(string managerId);

        //bool IsManager(string colleagueId);
        //bool IsManagerByEmail(string email);

        void Dispose();
    }
}
