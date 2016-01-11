using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Interfaces
{
    public interface IMeeting
    {
        IEnumerable<Question> GetQuestions();

        MeetingView GetMeeting(int meetingId);

        MeetingView CreateMeeting(string colleagueId);

        int SaveMeeting(MeetingView meeting);

        void UpdateMeeting(MeetingView meeting);

        TeamView GetMeetings(string colleagueId);

        IEnumerable<TeamView> GetTeam(string managerId);

        void Dispose();
    }
}
