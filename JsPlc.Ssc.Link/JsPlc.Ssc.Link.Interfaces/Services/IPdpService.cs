using System;
using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Interfaces
{
    public interface IPdpService
    {
        LinkPdp GetPdp(string colleagueId);

        bool UpdatePdp(LinkPdp linkPdp);

        void Dispose();
    }
}
