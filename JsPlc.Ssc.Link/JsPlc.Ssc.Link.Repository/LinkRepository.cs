using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Repository
{
    public class LinkRepository:ILinkRepository
    {
        private RepositoryContext db;

        public LinkRepository() { }

        public LinkRepository(RepositoryContext context) { db = context; }


        public IEnumerable<Question> GetQuestions(int periodId)
        {
           return db.Questions.Where(q => q.PeriodId == periodId);
        }

        public IEnumerable<Answer> GetAnswers(int meetingId)
        {
            return db.Answers.Where(a => a.LinkMeetingId == meetingId);
        }


        //public IEnumerable<AnswerView> GetAnswers(AnswerParams parmas)
        //{
        //    var result = from a in db.Answers
        //        join m in db.Meeting on a.LinkMeetingId equals m.Id
        //        join q in db.Questions on m.PeriodId equals q.PeriodId
        //        where m.EmployeeId == parmas.EmployeeId && m.PeriodId == parmas.PeriodId && a.QuestionId==q.Id
        //        select
        //            new  AnswerView()
        //            {
        //                Id = q.Id,
        //                CollegueComment=a.ColleagueComments,
        //                ManagerComment=a.ManagerComments,
        //            };
        //    return result;
        //}

        public EmployeeView GetEmployee(int id)
        {
            
            var employee = (from e in db.Employees where e.Id==id
                           join m in db.Employees on e.ManagerId equals m.Id
                           select new EmployeeView
                            {
                                Id = e.Id,
                                EmployeeId = e.EmployeeId,
                                FirstName = e.FirstName,
                                LastName = e.LastName,
                                ManagerId = m.Id,
                                ManagerName = m.FirstName+" "+m.LastName,
                                EmailAddress = e.EmailAddress
                    
                            }).FirstOrDefault();

            return employee;
        }

        public void SaveMeeting(LinkMeeting meeting)
        {

            var result= db.Meeting.Add(meeting);
            
            if (meeting.Answers != null)
            {
                foreach (var answer in meeting.Answers)
                {
                    answer.LinkMeetingId = result.Id;
                    db.Answers.Add(answer);
                }
            }

            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
