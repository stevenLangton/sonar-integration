using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Interfaces.Services
{
    public interface IColleaguePdpService
    {
        ColleaguePdp GetPdp(string colleagueId);

        ColleaguePdp UpdatePdp(ColleaguePdp colleaguePdp);

        void Dispose();
    }
}
