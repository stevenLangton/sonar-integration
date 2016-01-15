using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Interfaces;

namespace JsPlc.Ssc.Link.Repository
{
    public class Meeting : IMeeting
    {
        private readonly RepositoryContext _db;

        public Meeting() { }

        public Meeting(RepositoryContext context) { _db = context; }

        public IEnumerable<Question> GetQuestions()
        {
            return _db.Questions.OrderBy(q => q.Id);
        }

        // meetings history of an employee
        public TeamView GetMeetings(string colleagueId)
        {
            var firstOrDefault = _db.Employees.FirstOrDefault(e => e.ColleagueId == colleagueId);

            if (firstOrDefault == null) return null;

            var empId = firstOrDefault.Id;

            var myReport = (from e in _db.Employees
                .Where(e => e.Id == empId)
                            select new TeamView
                            {
                                EmployeeId = e.Id,
                                ColleagueId = e.ColleagueId,
                                FirstName = e.FirstName,
                                LastName = e.LastName,
                                Meetings = (from m in _db.Meeting
                                            orderby m.MeetingDate descending
                                            where m.EmployeeId == e.Id
                                            select new LinkMeetingView
                                            {
                                                MeetingId = m.Id,
                                                MeetingDate = m.MeetingDate,
                                                ColleagueSignOff = m.ColleagueSignOff,
                                                ManagerSignOff = m.ManagerSignOff
                                            }).ToList(),
                                EmailAddress = e.EmailAddress
                            }).FirstOrDefault();

            foreach (var meeting in myReport.Meetings)
            {
                var mDate = meeting.MeetingDate;

                var period = (from p in _db.Periods
                              where mDate >= p.Start && mDate <= p.End
                              select p).FirstOrDefault();

                if (period == null) continue; // should not occur since each meeting should fall within a period

                meeting.Period = period.Description;
                meeting.Year = period.Year;
            }
            return myReport;
        }

        // view particular meeting
        public MeetingView GetMeeting(int meetingId)
        {
            // Get meeting details along with manager details
            var meeting = (from m in _db.Meeting
                           join e in _db.Employees on m.EmployeeId equals e.Id into e_join
                           from e in e_join.DefaultIfEmpty()
                           join mm in _db.Employees on m.ManagerId equals mm.ColleagueId into m_join
                           from mm in m_join.DefaultIfEmpty()
                           where m.Id == meetingId
                           select new MeetingView
                           {
                               MeetingId = m.Id,
                               MeetingDate = m.MeetingDate,
                               EmployeeId = e.Id,
                               ColleagueId = e.ColleagueId,
                               ColleagueName = string.Concat(e.FirstName, " " + e.LastName),
                               ManagerId = mm.ColleagueId,
                               ManagerName = string.Concat(mm.FirstName, " " + mm.LastName),
                               ColleagueSignOff = m.ColleagueSignOff,
                               ManagerSignOff = m.ManagerSignOff,
                           }).FirstOrDefault();

            //Get questions with answers for particular meeting
            var question = from q in _db.Questions
                           join a in _db.Answers on new { q.Id, LinkMeetingId = meetingId } equals new { Id = a.QuestionId, a.LinkMeetingId } into a_join
                           from a in a_join.DefaultIfEmpty()
                           select new QuestionView
                           {
                               QuestionId = q.Id,
                               Question = q.Description,
                               QuestionType = q.QuestionType,
                               AnswerId = a.Id,
                               ColleagueComment = a.ColleagueComments,
                               ManagerComment = a.ManagerComments,
                               Discussed = a.Discussed
                           };

            if (meeting != null)
                meeting.Questions = question;

            return meeting;
        }

        // create new meeting
        public MeetingView CreateMeeting(string colleagueId)
        {
            // Get meeting details along with manager details
            var meeting = (from e in _db.Employees
                           where e.ColleagueId == colleagueId
                           join m in _db.Employees on e.ManagerId equals m.ColleagueId into m_join
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
            var question = from q in _db.Questions
                           select new QuestionView
                           {
                               QuestionId = q.Id,
                               Question = q.Description,
                               QuestionType = q.QuestionType
                           };

            if (meeting != null)
                meeting.Questions = question;

            return meeting;
        }

        // save new meeting
        public int SaveMeeting(MeetingView view)
        {
            int empId = _db.Employees.Where(e => e.ColleagueId == view.ColleagueId).Select(e => e.Id).FirstOrDefault();

            var meeting = new LinkMeeting
            {
                EmployeeId = empId,
                MeetingDate = view.MeetingDate,
                ManagerId = view.ManagerId,
                ColleagueSignOff = view.ColleagueSignOff,
                ManagerSignOff = view.ManagerSignOff
            };

            var result = _db.Meeting.Add(meeting);
            _db.SaveChanges();

            foreach (var answer in view.Questions.Select(answer => new Answer
            {
                ColleagueComments = answer.ColleagueComment,
                ManagerComments = answer.ManagerComment,
                QuestionId = answer.QuestionId,
                LinkMeetingId = result.Id,
                Discussed = answer.Discussed
            }))
            {
                _db.Answers.Add(answer);
                _db.SaveChanges();
            }

            return result.Id;
        }

        // update the meeting
        public void UpdateMeeting(MeetingView view)
        {
            var meeting = _db.Meeting.FirstOrDefault(m => m.Id == view.MeetingId);

            if (meeting != null)
            {
                var linkMeeting = new LinkMeeting
                {
                    MeetingDate = view.MeetingDate,
                    ColleagueSignOff = view.ColleagueSignOff,
                    ManagerSignOff = view.ManagerSignOff,
                    EmployeeId = meeting.EmployeeId,
                    ManagerId = meeting.ManagerId,
                    Id = view.MeetingId
                };
                _db.Meeting.AddOrUpdate(linkMeeting);
                _db.SaveChanges();
            }

            //foreach (var answer in view.Questions.Select(question => question.AnswerId != null ? new Answer
            //{
            //    Id = (int)question.AnswerId,
            //    QuestionId = question.QuestionId,
            //    ColleagueComments = question.ColleagueComment,
            //    ManagerComments = question.ManagerComment,
            //    LinkMeetingId = view.MeetingId,
            //    Discussed = question.Discussed
            //} : null))
            //{
            //    if (answer != null)
            //    {
            //        _db.Answers.AddOrUpdate(answer);
            //        _db.SaveChanges();
            //    }
            //}

            //Update questions if modified
            foreach (var question in view.Questions)
            {
                if (question.Discussed != null || !String.IsNullOrEmpty(question.ManagerComment) || !String.IsNullOrEmpty(question.ColleagueComment))
                {
                    var answer = new Answer();
                        answer.QuestionId = question.QuestionId;
                        answer.ColleagueComments = question.ColleagueComment??"";
                        answer.ManagerComments = question.ManagerComment??"";
                        answer.LinkMeetingId = view.MeetingId;
                        answer.Discussed = question.Discussed.HasValue ? question.Discussed.Value : false;

                        if (question.AnswerId == null)
                        {
                            _db.Answers.Add(answer);
                        }
                        else
                        {
                            answer.Id = (int)question.AnswerId;
                            _db.Answers.AddOrUpdate(answer);

                        }
                }
            }

            _db.SaveChanges();

        }

        // employees and their meeting history of a manager
        public IEnumerable<TeamView> GetTeam(string managerId)
        {
            var team = _db.Employees.Where(e => e.ManagerId == managerId);

            var teamView = new List<TeamView>();

            foreach (var employee in team)
            {
                teamView.Add(GetMeetings(employee.ColleagueId));
            }
            return teamView;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }


}
