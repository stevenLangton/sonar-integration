using System.Data.Entity;
using JsPlc.Ssc.Link.Models;


namespace JsPlc.Ssc.Link.Repository
{
    public interface IRepositoryContext
    {
        IDbSet<Question> Questions { get; }

        IDbSet<Answer> Answers { get; }

        IDbSet<Employee> Employees { get; }

        IDbSet<Period> Periods { get; }

        IDbSet<LinkMeeting> Meeting { get; set; }
    }
}
