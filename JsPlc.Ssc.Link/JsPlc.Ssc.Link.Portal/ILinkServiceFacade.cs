using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Portal
{
    public interface ILinkServiceFacade
    {
        List<string> GetApiServiceKeys();

        Task<int> CreateObjective(LinkObjective modified);
        ColleagueView GetColleague(string ColleagueId);
        ColleagueView GetColleagueByUsername(string email);
        MeetingView GetMeeting(int Id);
        ColleagueTeamView GetMyMeetingsView(string colleagueId);
        MeetingView GetNewMeetingView(string colleagueId);
        LinkMeeting GetNextMeeting(string colleagueId);
        Task<LinkObjective> GetObjective(string colleagueId, int objectiveId);
        List<LinkObjective> GetObjectivesList(string colleagueId);
        Task<List<LinkObjective>> GetSharedObjectives(string colleagueId);
        LinkPdp GetPdp(string ColleagueId);
        IEnumerable<ColleagueTeamView> GetTeamView(string managerId);
        ColleagueView GetUserDetails(string Email);
        bool HasColleagueAccess(string colleagueId, string otherColleagueId);
        bool HasMeetingAccess(int meetingId, string colleagueId);
        //bool IsManager(string username);
        bool IsManagerByEmail(string email);
        Task<bool> UpdateObjective(LinkObjective modified);
        Task<LinkPdp> UpdatePdp(LinkPdp modified);
        Task<MeetingView> UnshareMeeting(int meetingId);
        Task<MeetingView> ApproveMeeting(int meetingId);
    }
}
