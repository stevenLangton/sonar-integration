using System.Data.Entity;
using JsPlc.Ssc.Link.Models;


namespace JsPlc.Ssc.Link.Repository
{
    public interface IRepositoryContext
    {
        IDbSet<Models.Question> Questions { get; }

        IDbSet<Models.Answer> Answers { get; }

        IDbSet<Models.Employee> Employees { get; }

        IDbSet<Models.Period> Periods { get; }

        IDbSet<Models.LinkMeeting> Meeting { get; set; }

        IDbSet<Models.Objectives> Objectives { get; set; }

    }
}
