using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Repository
{
    public interface ILinkRepository
    {
        IEnumerable<Period> GetPeriods();

        IEnumerable<Question> GetQuestions(int periodId);
        
        MeetingView GetMeeting(int meetingId);
        IEnumerable<MeetingView> GetMeetings(string employeeId);
        MeetingView CreateMeeting(string employeeId, int periodId);
        int SaveMeeting(MeetingView meeting);

        EmployeeView GetEmployee(string employeeId);
        IEnumerable<TeamView> GetTeam(string managerId);
        
        void Dispose();
    }
}
