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
            return _db.Colleagues.FirstOrDefault(e =>e.ColleagueId.ToLower().Equals(colleagueId.ToLower()));
        }

        StubColleague IStubLinkRepository.GetColleagueByEmail(string emailAddress)
        {
            return _db.Colleagues.FirstOrDefault(e => e.EmailAddress.ToLower().Equals(emailAddress.ToLower()));
        }
       
        List<StubColleague> IStubLinkRepository.GetDirectReports(string managerId)
        {
            return GetDirectReports(managerId);
        }

        List<StubColleague> IStubLinkRepository.GetDirectReportsByManagerEmail(string managerEmail)
        {
            return GetDirectReportsByManagerEmail(managerEmail);
        }

        bool IStubLinkRepository.IsManager(string colleagueId)
        {
            var dr = GetDirectReports(colleagueId);
            return dr.Any();
        }

        bool IStubLinkRepository.IsManagerByEmail(string email)
        {
            var dr = GetDirectReportsByManagerEmail(email);
            return dr != null && dr.Any();
        }

        private List<StubColleague> GetDirectReports(string colleagueId)
        {
            var dr = _db.Colleagues.Where(x => x.ManagerId.Equals(colleagueId));
            return dr.ToList();
        }

        private List<StubColleague> GetDirectReportsByManagerEmail(string managerEmail)
        {
            var mgr = _db.Colleagues.FirstOrDefault(x => x.EmailAddress.Equals(managerEmail));
            if (mgr == null) return null;

            var dr = _db.Colleagues.Where(x => x.ManagerId.Equals(mgr.ColleagueId));
            return dr.ToList();
        }

        void IStubLinkRepository.Dispose()
        {
            _db.Dispose();
        }
    }
}
