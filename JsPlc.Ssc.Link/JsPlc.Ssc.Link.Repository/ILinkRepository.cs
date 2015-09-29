using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Repository
{
    public interface ILinkRepository
    {
        IEnumerable<Question> GetQuestions();
        Employee GetEmployee(int employeeId);
        void SaveAnswers(IEnumerable<Answer> answers);
    }
}
