using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Repository
{
    public interface ILinkRepository
    {
        IEnumerable<Question> GetQuestions(int periodId);
        //IEnumerable<AnswerView> GetAnswers(AnswerParams parmas);
        IEnumerable<Answer> GetAnswers(int meetingId);
        EmployeeView GetEmployee(int employeeId);
        void SaveMeeting(LinkMeeting meeting);

        void Dispose();
    }
}
