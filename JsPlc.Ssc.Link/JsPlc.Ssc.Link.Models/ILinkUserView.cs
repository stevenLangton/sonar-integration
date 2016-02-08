using System;
namespace JsPlc.Ssc.Link.Models
{
    public interface ILinkUserView
    {
        ColleagueView Colleague { get; set; }
        bool IsLineManager { get; set; }
        string UserId { get; set; }
    }
}
