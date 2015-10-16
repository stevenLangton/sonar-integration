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

        public IEnumerable<Question> GetQuestions()
        {
           return db.Questions.OrderBy(q=>q.Id);
        }

        public IEnumerable<TeamView> GetTeam(string managerId)
        {
            IList<TeamView> team = (from e in db.Employees
                             where e.ManagerId == managerId
                             orderby e.FirstName, e.LastName
                             select new TeamView
                             {
                                EmployeeId = e.Id,
                                 ColleagueId = e.ColleagueId,
                                 FirstName = e.FirstName,
                                 LastName = e.LastName,
                                 Meetings = (from m in db.Meeting
                                             orderby m.MeetingDate
                                             where m.EmployeeId == e.Id
                                             
                                             select new LinkMeetingView
                                             {
                                                MeetingId = m.Id,
                                                MeetingDate = m.MeetingDate,
                                                ColleagueSignOff   = m.ColleagueSignOff,
                                                ManagerSignOff = m.ManagerSignOff
                                            }).ToList(),
                                 EmailAddress = e.EmailAddress
                          }).ToList();

            foreach (var member in team)
            {
                foreach (var meeting in member.Meetings)
                {
                    var mDate = meeting.MeetingDate;

                    var period = (from p in db.Periods
                        where mDate >= p.Start && mDate <= p.End
                        select p).FirstOrDefault() ;

                    if (period == null) continue; // should not occur since each meeting should fall within a period

                    meeting.Period = period.Description;
                    meeting.Year = period.Year;
                }
            }
           
            return team;
        }

        public Employee GetEmployee(string emailAddres)
        {
           return db.Employees.FirstOrDefault(e => e.EmailAddress == emailAddres);
        }

        public bool IsManager(string userName)
        {
            var firstOrDefault = db.Employees.FirstOrDefault(e => e.EmailAddress == userName);

            if (firstOrDefault == null) return false;

            var id = firstOrDefault.ColleagueId;

            var subEmployees = db.Employees.Where(e => e.ManagerId == id);

            return subEmployees.Any();
        }

        // view particular meeting
        public MeetingView GetMeeting(int meetingId)
        {
            // Get meeting details along with manager details
            var meeting = (from m in db.Meeting
                join e in db.Employees on m.EmployeeId equals e.Id into e_join
                from e in e_join.DefaultIfEmpty()
                join mm in db.Employees on m.ManagerId equals mm.ColleagueId into m_join
                from mm in m_join.DefaultIfEmpty()
                where m.Id== meetingId 
                select new MeetingView
                {
                    MeetingId = m.Id,
                    MeetingDate = m.MeetingDate,
                    EmployeeId = e.Id,
                    ColleagueId = e.ColleagueId,
                    ColleagueName = string.Concat(e.FirstName," "+ e.LastName),
                    ManagerId = mm.ColleagueId,
                    ManagerName = string.Concat(mm.FirstName," "+ mm.LastName),
                    ColleagueSignOff = m.ColleagueSignOff,
                    ManagerSignOff = m.ManagerSignOff,
                }).FirstOrDefault();

            //Get questions with answers for particular meeting
            var question = from q in db.Questions
                join a in db.Answers on new {q.Id, LinkMeetingId = meetingId} equals new {Id = a.QuestionId, a.LinkMeetingId} into a_join
                from a in a_join.DefaultIfEmpty()
                select new QuestionView
                {
                    QuestionId = q.Id,
                    Question = q.Description,
                    QuestionType = q.QuestionType,
                    AnswerId = a.Id,
                    ColleagueComment = a.ColleagueComments,
                    ManagerComment = a.ManagerComments
                };

            if(meeting!=null)
                meeting.Questions = question;

            return meeting;
        }

        // all meetings of an employee
        public IEnumerable<MeetingView> GetMeetings(string employeeId)
        {
            return null;
        }

        // create new meeting
        public MeetingView CreateMeeting(string colleagueId)
        {
            // Get meeting details along with manager details
            var meeting = (from e in db.Employees
                           where e.ColleagueId == colleagueId
                            join m in db.Employees on e.ManagerId equals m.ColleagueId into m_join
                            from m in m_join.DefaultIfEmpty()
                            select new MeetingView
                            {
                                MeetingId = 0,
                                MeetingDate = DateTime.Now,
                                EmployeeId = e.Id,
                                ColleagueId = e.ColleagueId,
                                ColleagueName = string.Concat(e.FirstName, " " + e.LastName),
                                ManagerId = m.ColleagueId,
                                ManagerName = string.Concat(m.FirstName, " " + m.LastName),
                                ColleagueSignOff = 0,
                                ManagerSignOff = 0,
                            }).FirstOrDefault();

            //Get questions with answers for particular meeting
            var question = from q in db.Questions 
                           select new QuestionView
                           {
                               QuestionId = q.Id,
                               Question = q.Description,
                               QuestionType = q.QuestionType
                           };

            if(meeting!=null)
                meeting.Questions = question;

            return meeting;
        }

        // save new meeting
        public int SaveMeeting(MeetingView view)
        {
            int empId = db.Employees.Where(e => e.ColleagueId == view.ColleagueId).Select(e => e.Id).FirstOrDefault();

            var meeting = new LinkMeeting
            {
                EmployeeId = empId,
                MeetingDate = view.MeetingDate,
                ManagerId = view.ManagerId,
                ColleagueSignOff = view.ColleagueSignOff,
                ManagerSignOff = view.ManagerSignOff
            };

            var result= db.Meeting.Add(meeting);
            db.SaveChanges();

            foreach (var answer in view.Questions.Select(answer => new Answer
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

        // update the meeting
        public void UpdateMeeting(int id,MeetingView view)
        {
            var meeting = db.Meeting.FirstOrDefault(m => m.Id == id);
           
            if (meeting != null)
            {
                var linkMeeting = new LinkMeeting
                {
                    MeetingDate = view.MeetingDate,
                    ColleagueSignOff = view.ColleagueSignOff,
                    ManagerSignOff = view.ManagerSignOff,
                    Id = view.MeetingId
                };
                db.Meeting.AddOrUpdate(linkMeeting);
                db.SaveChanges();
            }

            foreach (var answer in view.Questions.Select(answer => new Answer
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
