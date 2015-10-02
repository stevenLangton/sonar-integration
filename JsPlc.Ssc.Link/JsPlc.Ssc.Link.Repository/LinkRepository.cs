using System;
using System.Collections.Generic;
using System.Linq;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Repository
{
    public class LinkRepository:ILinkRepository
    {
        private RepositoryContext db;

        public LinkRepository() { }

        public LinkRepository(RepositoryContext context) { db = context; }

        public IEnumerable<Period> GetPeriods()
        {
            return db.Periods.ToList();
        }

        public IEnumerable<Question> GetQuestions(int periodId)
        {
           return db.Questions.Where(q => q.PeriodId == periodId);
        }

        public MeetingView GetMeeting(int meetingId)
        {
            var result = (from m in db.Meeting
                join p in db.Periods on m.PeriodId equals p.Id
                join q in db.Questions on m.PeriodId equals q.PeriodId into q_join
                from q in q_join.DefaultIfEmpty()
                join a in db.Answers
                    on new {m.Id, Column1 = q.Id}
                    equals new {Id = a.LinkMeetingId, Column1 = a.QuestionId} into a_join
                from a in a_join.DefaultIfEmpty()
                join e in db.Employees on new {EmployeeId = m.EmployeeId} equals new {EmployeeId = e.Id} into e_join
                from e in e_join.DefaultIfEmpty()
                join mm in db.Employees on new {ManagerId = e.ManagerId} equals new {ManagerId = mm.Id} into mm_join
                from mm in mm_join.DefaultIfEmpty()
                where m.Id == meetingId
                select new MeetingView()
                {
                    MeetingId = m.Id,
                    MeetingDate = m.MeetingDate,
                    PeriodId = m.PeriodId,
                    PeriodDescription = p.Description,
                    EmployeeId = e.EmployeeId,
                    EmployeeName = string.Concat(e.FirstName, e.LastName),
                    ManagerId = mm.EmployeeId,
                    ManagerName = string.Concat(mm.FirstName, mm.LastName),
                    Status = m.Status,
                    QuestionId = q.Id,
                    ColleagueComments = a.ColleagueComments,
                    ManagerComments = a.ManagerComments
                }).FirstOrDefault();

            return result;
        }

        public IEnumerable<MeetingView> GetMeetings(int employeeId)
        {
            return null;
        }

        public EmployeeView GetEmployee(int id)
        {
            
            var employee = (from e in db.Employees where e.Id==id
                           join m in db.Employees on e.ManagerId equals m.Id into m_join
                           from m in m_join.DefaultIfEmpty()
                           select new EmployeeView
                            {
                                Id = e.Id,
                                EmployeeId = e.EmployeeId,
                                FirstName = e.FirstName,
                                LastName = e.LastName,
                                ManagerId = m.Id,
                                ManagerName =String.Concat(m.FirstName,m.LastName),
                                EmailAddress = e.EmailAddress
                    
                            }).FirstOrDefault();

            return employee;
        }

        public IEnumerable<Employee> GetEmployees(int managerId)
        {
            return db.Employees.Where(e=>e.ManagerId==managerId);
        }

        public int SaveMeeting(LinkMeeting meeting)
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
            return result.Id;
        }

        public void Dispose()
        {
            db.Dispose();
        }
       
    }
}
