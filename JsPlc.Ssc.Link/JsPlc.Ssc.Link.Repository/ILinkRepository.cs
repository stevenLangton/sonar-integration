using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Repository
{
    public interface ILinkRepository
    {
        
        IEnumerable<Question> GetQuestions();
        
        MeetingView GetMeeting(int meetingId);
        IEnumerable<MeetingView> GetMeetings(string employeeId);
        MeetingView CreateMeeting(string employeeId);
        int SaveMeeting(MeetingView meeting);
        void UpdateMeeting(int id,MeetingView meeting);

    
        IEnumerable<TeamView> GetTeam(string managerId);
        bool IsManager(string userName);
        
        void Dispose();
    }
}
