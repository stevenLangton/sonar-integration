using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Interfaces
{
    public interface ILinkRepository
    {
        Employee GetEmployee(string emailAddres);

        bool IsManager(string userName);

        int appUserID(string StaffHrId);

        void Dispose();
    }
}
