using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Interfaces.Services
{
    public interface IMeetingService
    {
        IEnumerable<Question> GetQuestions();

        MeetingView GetMeeting(int meetingId);

        MeetingView CreateMeeting(string colleagueId);

        int SaveMeeting(MeetingView meeting);

        void UpdateMeeting(MeetingView meeting);

        ColleagueTeamView GetColleagueAndMeetings(string colleagueId);

        IEnumerable<ColleagueTeamView> GetTeam(string managerId);

        void Dispose();
    }
}
