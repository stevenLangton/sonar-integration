using System;
using System.Linq;
using System.Data.Entity;
using JsPlc.Ssc.Link.Interfaces;
using JsPlc.Ssc.Link.Models.Entities;
using JsPlc.Ssc.Link.Repository;

namespace JsPlc.Ssc.Link.Service.Services
{
    public class PdpService : IPdpService
    {
        private readonly RepositoryContext _db;

        public PdpService() { }

        public PdpService(RepositoryContext context) { _db = context; }

        //Only one pdf can be created for each colleague
        //create pdp if one does not already exist.
        //no manager id as kept in pdp as only one record.
        public LinkPdp GetPdp(string colleagueId)
        {
            if (_db.Pdp.Any(e => e.ColleagueId == colleagueId))
            {
                var Pdp = _db.Pdp.First(e => e.ColleagueId == colleagueId);
                return Pdp;
            }
            else  
            {
                LinkPdp newPdP = new LinkPdp();
                newPdP.ColleagueId = colleagueId;
                InsertPDP(newPdP);

                var Pdp = _db.Pdp.First(e => e.ColleagueId == colleagueId);
                return Pdp;
            }
        }

        public LinkPdp UpdatePdp(LinkPdp linkPdp)
        {
                _db.Entry(linkPdp).State = EntityState.Modified;
                _db.SaveChanges();

                var Pdp = _db.Pdp.First(e => e.ColleagueId == linkPdp.ColleagueId);
                return Pdp;
        }

        private bool InsertPDP(LinkPdp linkPdp)
        {
            try
            {
                _db.Pdp.Add(linkPdp);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}