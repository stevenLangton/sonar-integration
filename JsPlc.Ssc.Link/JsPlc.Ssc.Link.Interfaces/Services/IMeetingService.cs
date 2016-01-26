using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Interfaces.Services
{
    public interface IMeetingService
    {
        IEnumerable<Question> GetQuestions();

        MeetingView GetMeeting(int meetingId);

        MeetingView GetNextMeeting(string colleagueId);

        MeetingView CreateMeeting(string colleagueId);

        int SaveMeeting(MeetingView meeting);

        void UpdateMeeting(MeetingView meeting);

        TeamView GetMeetings(string colleagueId);

        IEnumerable<TeamView> GetTeam(string managerId);

        void Dispose();
    }
}
