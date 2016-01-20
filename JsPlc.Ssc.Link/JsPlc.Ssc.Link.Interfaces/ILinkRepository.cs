using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Interfaces
{
    public interface ILinkRepository
    {
        ColleagueView GetColleague(string emailAddres);

        bool IsManager(string userName);

        int AppUserId(string colleagueId);

        void Dispose();
    }
}
