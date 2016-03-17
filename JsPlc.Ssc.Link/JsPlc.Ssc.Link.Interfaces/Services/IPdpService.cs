using System;
using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Interfaces
{
    public interface IPdpService
    {
        LinkPdp GetPdp(string colleagueId);

        LinkPdp UpdatePdp(LinkPdp linkPdp);

        //LinkPdp UnsharePdp(string colleagueId);
        //LinkPdp ApprovePdp(string colleagueId, string managerId);

        void Dispose();
    }
}
