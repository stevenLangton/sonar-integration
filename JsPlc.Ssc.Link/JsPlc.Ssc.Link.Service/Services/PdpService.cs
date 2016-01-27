using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
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
            var Pdp = _db.Pdp.First(e => e.ColleagueId == colleagueId);

            if (Pdp == null)
            {
                LinkPdp newPdP = new LinkPdp();
                newPdP.ColleagueId = colleagueId;
                InsertObjective(newPdP);

                Pdp = _db.Pdp.First(e => e.ColleagueId == colleagueId);
               
            }

            return Pdp;
        }

        public bool UpdatePdp(LinkPdp linkPdp)
        {
            try
            {
                _db.Entry(linkPdp).State = EntityState.Modified;
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }


        }

        private bool InsertObjective(LinkPdp linkPdp)
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