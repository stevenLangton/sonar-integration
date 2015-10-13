using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
            return db.Periods.ToList().OrderBy(p=>p.Id);
        }

        public IEnumerable<Question> GetQuestions(int periodId)
        {
           return db.Questions.Where(q => q.PeriodId == periodId).OrderBy(q=>q.Id);
        }

        public EmployeeView GetEmployee(string id)
        {

            var employee = (from e in db.Employees
                            where e.ColleagueId == id
                            join m in db.Employees on e.ManagerId equals m.ColleagueId into m_join
                            from m in m_join.DefaultIfEmpty()
                            select new EmployeeView
                            {
                                Id = e.Id,
                                ColleagueId = e.ColleagueId,
                                FirstName = e.FirstName,
                                LastName = e.LastName,
                                ManagerId = m.ColleagueId,
                                ManagerName = String.Concat(m.FirstName, m.LastName),
                                EmailAddress = e.EmailAddress

                            }).FirstOrDefault();

            return employee;
        }

        public IEnumerable<TeamView> GetTeam(string managerId)
        {
            IEnumerable<TeamView> employees = (from e in db.Employees
                             where e.ManagerId == managerId
                             orderby e.FirstName, e.LastName
                             select new TeamView()
                             {
                                 Id = e.Id,
                                 ColleagueId = e.ColleagueId,
                                 FirstName = e.FirstName,
                                 LastName = e.LastName,
                                 Meetings = (from m in db.Meeting
                                             where m.PeriodId==1
                                             orderby m.PeriodId
                                            where m.EmployeeId == e.Id
                                            select new LinkMeetingView()
                                            {
                                                Id = m.Id,
                                                MeetingDate = m.MeetingDate,
                                                PeriodId = m.PeriodId,
                                                Status = m.Status
    
                                            }).ToList(),
                                 EmailAddress = e.EmailAddress
                          });
           
            return employees.ToList();
        }

        public MeetingView GetMeeting(int meetingId)
        {
            // Get meeting details along with manager details
            var meeting = (from m in db.Meeting
                join p in db.Periods on m.PeriodId equals p.Id
                join e in db.Employees on m.EmployeeId equals e.Id into e_join
                from e in e_join.DefaultIfEmpty()
                join mm in db.Employees on e.ManagerId equals mm.ColleagueId into m_join
                from mm in m_join.DefaultIfEmpty()
                where m.Id== meetingId 
                select new MeetingView()
                {
                    MeetingId = m.Id,
                    MeetingDate = m.MeetingDate,
                    PeriodId = m.PeriodId,
                    PeriodDescription = p.Description,
                    EmployeeId = e.Id,
                    ColleagueId = e.ColleagueId,
                    ColleagueName = string.Concat(e.FirstName," "+ e.LastName),
                    ManagerId = mm.ColleagueId,
                    ManagerName = string.Concat(mm.FirstName," "+ mm.LastName),
                    Status = m.Status,
                    Start = p.Start,
                    End=p.End
                }).FirstOrDefault();

            //Get questions with answers for particular meeting
            var question = from q in db.Questions
                join a in db.Answers on new {q.Id, LinkMeetingId = meetingId} equals new {Id = a.QuestionId, a.LinkMeetingId} into a_join
                from a in a_join.DefaultIfEmpty()
                where q.PeriodId==meeting.PeriodId
                select new QuestionView()
                {
                    QuestionId = q.Id,
                    Question = q.Description,
                    AnswerId = a.Id,
                    ColleagueComment = a.ColleagueComments,
                    ManagerComment = a.ManagerComments
                };

            if(meeting!=null)
                meeting.Questions = question;

            return meeting;
        }

        public IEnumerable<MeetingView> GetMeetings(string employeeId)
        {
            return null;
        }

        public MeetingView CreateMeeting(string employeeId, int periodId)
        {
            // Get meeting details along with manager details
            var meeting = (from e in db.Employees
                            where e.ColleagueId == employeeId
                            join m in db.Employees on e.ManagerId equals m.ColleagueId into m_join
                            from m in m_join.DefaultIfEmpty()
                            select new MeetingView
                            {
                                MeetingId = 0,
                                MeetingDate = DateTime.Now,
                                PeriodId = periodId,
                                EmployeeId = e.Id,
                                ColleagueId = e.ColleagueId,
                                ColleagueName = string.Concat(e.FirstName, " " + e.LastName),
                                ManagerId = m.ColleagueId,
                                ManagerName = string.Concat(m.FirstName, " " + m.LastName),
                                Status = 0
                            }).FirstOrDefault();

            //Get questions with answers for particular meeting
            var question = from q in db.Questions 
                           where q.PeriodId==periodId
                           select new QuestionView()
                           {
                               QuestionId = q.Id,
                               Question = q.Description,
                           };

            if(meeting!=null)
                meeting.Questions = question;

            return meeting;
        }

        public int SaveMeeting(MeetingView view)
        {
            int empId = db.Employees.Where(e => e.ColleagueId == view.ColleagueId).Select(e => e.Id).FirstOrDefault();

            var meeting = new LinkMeeting()
            {
                EmployeeId = empId,
                PeriodId = view.PeriodId,
                MeetingDate = view.MeetingDate,
                Status = view.Status
            };

            var result= db.Meeting.Add(meeting);
            db.SaveChanges();

            foreach (var answer in view.Questions.Select(answer => new Answer()
            {
                ColleagueComments = answer.ColleagueComment,
                ManagerComments = answer.ManagerComment,
                QuestionId = answer.QuestionId,
                LinkMeetingId = result.Id
            }))
            {
                db.Answers.Add(answer);
                db.SaveChanges();
            }
            
            return result.Id;
        }

        public void UpdateMeeting(MeetingView view)
        {
            var meeting = db.Meeting.FirstOrDefault(m => m.Id == view.MeetingId);
           
            if (meeting != null)
            {
                var linkMeeting = new LinkMeeting()
                {
                    MeetingDate = view.MeetingDate,
                    Status = view.Status,
                    Id = view.MeetingId
                };
                db.Meeting.AddOrUpdate(linkMeeting);
                db.SaveChanges();
            }

            foreach (var answer in view.Questions.Select(answer => new Answer()
            {
                ColleagueComments = answer.ColleagueComment,
                ManagerComments = answer.ManagerComment,
                QuestionId = answer.QuestionId,
                LinkMeetingId = view.MeetingId
            }))
            {
                db.Answers.AddOrUpdate(answer);
                db.SaveChanges();
            } 
        }

        public void Dispose()
        {
            db.Dispose();
        }
       
    }
}
