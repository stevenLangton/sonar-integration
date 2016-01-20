using System;
using System.Collections.Generic;
using System.Linq;
using JsPlc.Ssc.Link.StubService.StubModels;
using JsPlc.Ssc.Link.StubService.StubInterfaces;

namespace JsPlc.Ssc.Link.StubService.StubRepository
{
    public class StubLinkRepository:IStubLinkRepository
    {
        private readonly StubRepositoryContext _db;

        public StubLinkRepository() { }

        public StubLinkRepository(StubRepositoryContext context) { _db = context; }

        StubColleague IStubLinkRepository.GetColleague(string colleagueId)
        {
            //return _db.Employees.FirstOrDefault(e => e.EmailAddress.Equals(emailAddres,StringComparison.OrdinalIgnoreCase));
            return _db.Colleagues.FirstOrDefault(e =>e.ColleagueId.ToLower().Equals(colleagueId.ToLower()));
        }

        StubColleague IStubLinkRepository.GetColleagueByEmail(string emailAddress)
        {
            //return _db.Employees.FirstOrDefault(e => e.EmailAddress.Equals(emailAddres,StringComparison.OrdinalIgnoreCase));
            return _db.Colleagues.FirstOrDefault(e => e.EmailAddress.ToLower().Equals(emailAddress.ToLower()));
        }
       
        List<StubColleague> IStubLinkRepository.GetDirectReports(string managerId)
        {
            throw new NotImplementedException();
        }

        List<StubColleague> IStubLinkRepository.GetDirectReportsByManagerEmail(string managerEmail)
        {
            throw new NotImplementedException();
        }

        void IStubLinkRepository.Dispose()
        {
            _db.Dispose();
        }
    }
}
