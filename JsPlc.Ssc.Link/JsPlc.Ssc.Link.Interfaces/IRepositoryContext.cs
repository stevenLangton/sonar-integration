using System.Data.Entity;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Interfaces
{
    public interface IRepositoryContext
    {
        IDbSet<Question> Questions { get; }

        IDbSet<Answer> Answers { get; }

        //IDbSet<Models.Employee> Employees { get; }

        IDbSet<Period> Periods { get; }

        IDbSet<LinkMeeting> Meeting { get; set; }

        IDbSet<LinkObjective> Objectives { get; set; }
    }
}
