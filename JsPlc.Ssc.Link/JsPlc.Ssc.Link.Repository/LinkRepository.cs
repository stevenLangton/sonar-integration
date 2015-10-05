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

        public EmployeeView GetEmployee(int id)
        {

            var employee = (from e in db.Employees
                            where e.Id == id
                            join m in db.Employees on e.ManagerId equals m.Id into m_join
                            from m in m_join.DefaultIfEmpty()
                            select new EmployeeView
                            {
                                Id = e.Id,
                                EmployeeId = e.EmployeeId,
                                FirstName = e.FirstName,
                                LastName = e.LastName,
                                ManagerId = m.Id,
                                ManagerName = String.Concat(m.FirstName, m.LastName),
                                EmailAddress = e.EmailAddress

                            }).FirstOrDefault();

            return employee;
        }

        public IEnumerable<Employee> GetEmployees(int managerId)
        {
            return db.Employees.Where(e => e.ManagerId == managerId);
        }

        public MeetingView GetMeeting(int meetingId)
        {
            // Get meeting details along with manager details
            var meeting = (from m in db.Meeting
                join p in db.Periods on m.PeriodId equals p.Id
                join e in db.Employees on m.EmployeeId equals e.Id into e_join
                from e in e_join.DefaultIfEmpty()
                join mm in db.Employees on e.ManagerId equals mm.Id into m_join
                from mm in m_join.DefaultIfEmpty()
                where m.Id== meetingId
                select new MeetingView()
                {
                    MeetingId = m.Id,
                    MeetingDate = m.MeetingDate,
                    PeriodId = m.PeriodId,
                    PeriodDescription = p.Description,
                    EmployeeId = e.EmployeeId,
                    EmployeeName = string.Concat(e.FirstName," "+ e.LastName),
                    ManagerId = mm.EmployeeId,
                    ManagerName = string.Concat(mm.FirstName," "+ mm.LastName),
                    Status = m.Status,
                    Start = p.Start,
                    End=p.End
                }).FirstOrDefault();

            //Get questions with answers for particular meeting
            var question = from q in db.Questions
                           join a in db.Answers on new {q.Id, LinkMeetingId = meetingId }  equals new {Id = a.QuestionId, a.LinkMeetingId} into a_join
                from a in a_join.DefaultIfEmpty()
                select new AnswerView()
                {
                    AnswerId  = a.Id,
                    QuestionId = q.Id,
                    Question = q.Description,
                    CollegueComment = a.ColleagueComments,
                    ManagerComment = a.ManagerComments
                };

            if(meeting!=null)
                meeting.Questions = question;

            return meeting;
        }

        public MeetingView CreateMeeting(int employeeId, int periodId)
        {
            // Get meeting details along with manager details
            var meeting = (from e in db.Employees
                            where e.Id == employeeId
                            join m in db.Employees on e.ManagerId equals m.Id into m_join
                            from m in m_join.DefaultIfEmpty()
                            select new MeetingView
                            {
                                MeetingId = 0,
                                MeetingDate = DateTime.Now,
                                PeriodId = periodId,
                                EmployeeId = e.EmployeeId,
                                EmployeeName = string.Concat(e.FirstName, " " + e.LastName),
                                ManagerId = m.EmployeeId,
                                ManagerName = string.Concat(m.FirstName, " " + m.LastName),
                                Status = 0
                            }).FirstOrDefault();

            //Get questions with answers for particular meeting
            var question = from q in db.Questions 
                           where q.PeriodId==periodId
                           select new AnswerView()
                           {
                               QuestionId = q.Id,
                               Question = q.Description,
                           };

            if(meeting!=null)
                meeting.Questions = question;

            return meeting;
        }

        public IEnumerable<MeetingView> GetMeetings(int employeeId)
        {
            return null;
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
