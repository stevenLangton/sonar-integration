using System;
using System.Linq;
using System.Data.Entity;
using System.Web;
using Elmah;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models.Entities;
using JsPlc.Ssc.Link.Repository;

namespace JsPlc.Ssc.Link.Service.Services
{
    public class ColleaguePdpService : IColleaguePdpService
    {
        private readonly RepositoryContext _db;

        public ColleaguePdpService() { }

        public ColleaguePdpService(RepositoryContext context) { _db = context; }

        //create pdp if one does not already exist.
        //no manager id as kept in pdp as only one record.
        public ColleaguePdp GetPdp(string colleagueId)
        {
            if (_db.ColleaguePdps.Any(e => e.ColleagueId == colleagueId))
            {
                var pdp = _db.ColleaguePdps.First(e => e.ColleagueId == colleagueId);
                return pdp;
            }
            else  
            {
                var newPdP = new ColleaguePdp {ColleagueId = colleagueId};

                if(!InsertPdp(newPdP)) return null;

                var pdp = _db.ColleaguePdps.First(e => e.ColleagueId == colleagueId);
                return pdp;
            }
        }

        public ColleaguePdp UpdatePdp(ColleaguePdp colleaguePdp)
        {
                _db.Entry(colleaguePdp).State = EntityState.Modified;
                _db.SaveChanges();

                var pdp = _db.ColleaguePdps.First(e => e.ColleagueId == colleaguePdp.ColleagueId);
                return pdp;
        }

        private bool InsertPdp(ColleaguePdp colleaguePdp)
        {
            try
            {
                _db.ColleaguePdps.Add(colleaguePdp);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                ErrorLog.GetDefault(HttpContext.Current).Log(new Error(ex));
                return false;
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}