using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Repository
{
    public interface ILinkRepository
    {
        IEnumerable<Period> GetPeriods();

        IEnumerable<Question> GetQuestions(int periodId);
        
        MeetingView GetMeeting(int meetingId);
        IEnumerable<MeetingView> GetMeetings(int employeeId);
        MeetingView CreateMeeting(int meetingId, int periodId);
        int SaveMeeting(MeetingView meeting);

        EmployeeView GetEmployee(int employeeId);
        IEnumerable<Employee> GetEmployees(int managerId);
        
        void Dispose();
    }
}
