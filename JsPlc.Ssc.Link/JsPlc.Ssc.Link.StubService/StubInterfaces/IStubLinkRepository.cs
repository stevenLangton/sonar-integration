using System.Collections.Generic;
using JsPlc.Ssc.Link.StubService.StubModels;

namespace JsPlc.Ssc.Link.StubService.StubInterfaces
{
    public interface IStubLinkRepository
    {
        StubColleague GetColleague(string colleagueId);
        StubColleague GetColleagueByEmail(string emailAddress);

        List<StubColleague> GetDirectReports(string managerId);
        List<StubColleague> GetDirectReportsByManagerEmail(string managerId);

        bool IsManager(string colleagueId);
        bool IsManagerByEmail(string email);

        void Dispose();
    }
}
