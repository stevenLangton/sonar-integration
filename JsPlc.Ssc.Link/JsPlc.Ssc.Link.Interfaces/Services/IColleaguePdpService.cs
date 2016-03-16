using System;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Interfaces.Services
{
    public interface IColleaguePdpService
    {
        ColleaguePdp GetPdp(string colleagueId, int? selectedPeriodId = null, DateTime? now = null);
        ColleaguePdp GetPdp(int pdpId);

        ColleaguePdp UpdatePdp(ColleaguePdp colleaguePdp);

        void Dispose();
    }
}
