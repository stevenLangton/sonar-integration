using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Repository
{
    public interface ILinkRepository
    {
        IEnumerable<Period> GetPeriods();
        IEnumerable<Question> GetQuestions(int periodId);
        MeetingView GetMeeting(int meetingId);
        void GetMeetings(int employeeId);
        EmployeeView GetEmployee(int employeeId);
        IEnumerable<Employee> GetEmployees(int managerId);
        int SaveMeeting(LinkMeeting meeting);
        

        void Dispose();
    }
}
