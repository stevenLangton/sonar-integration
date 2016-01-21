using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;
using JsPlc.Ssc.Link.Portal.Helpers.Extensions;
using JsPlc.Ssc.Link.Repository;

namespace JsPlc.Ssc.Link.Service.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly RepositoryContext _db;
        private readonly IColleagueService _colleagueService;

        public MeetingService()
        {
            _db = new RepositoryContext();
            _colleagueService = new ColleagueService(new ServiceFacade());
        }

        public MeetingService(RepositoryContext context) { _db = context; }

        public MeetingService(RepositoryContext context, IColleagueService svc)
        { 
            _db = context;
            _colleagueService = svc;
        }
        public MeetingService(IColleagueService svc) { _colleagueService = svc; }

        public IEnumerable<Question> GetQuestions()
        {
            return _db.Questions.OrderBy(q => q.Id);
        }

        // meetings history of an employee
        public TeamView GetMeetings(string colleagueId)
        {
            ColleagueView cv = _colleagueService.GetColleague(colleagueId);
            
            var myReport = (from m in _db.Meeting
                            .Where(m => m.ColleagueId.Equals(colleagueId))
                            select new TeamView
                            {
                                //Colleague = new ColleagueView{ColleagueId = cv},
                                Meetings = (from m1 in _db.Meeting
                                            orderby m1.MeetingDate descending
                                            where m1.ColleagueId == cv.ColleagueId
                                            select new LinkMeetingView
                                            {
                                                MeetingId = m1.Id,
                                                MeetingDate = m1.MeetingDate,
                                                ColleagueSignOff = m1.ColleagueSignOff,
                                                ManagerSignOff = m1.ManagerSignOff
                                            }).ToList(),
                            }).FirstOrDefault();

            if (myReport == null) return null;
            
            myReport.Colleague = cv;

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
            //var meeting = (from m in _db.Meeting
            //               join e in _db.Employees on m.EmployeeId equals e.Id into e_join

            //               from e in e_join.DefaultIfEmpty()
            //               join mm in _db.Employees on m.ManagerId equals mm.ColleagueId into m_join

            //               from mm in m_join.DefaultIfEmpty()
            //               where m.Id == meetingId
            //               select new MeetingView
            //               {
            //                   MeetingId = m.Id,
            //                   MeetingDate = m.MeetingDate,
            //                   EmployeeId = e.Id,
            //                   ColleagueId = e.ColleagueId,
            //                   ColleagueName = string.Concat(e.FirstName, " " + e.LastName),
            //                   ManagerId = mm.ColleagueId,
            //                   ManagerName = string.Concat(mm.FirstName, " " + mm.LastName),
            //                   ColleagueSignOff = m.ColleagueSignOff,
            //                   ManagerSignOff = m.ManagerSignOff,
            //               }).FirstOrDefault();

            var meeting = _db.Meeting.FirstOrDefault(x => x.Id == meetingId);
            if (meeting == null) return null;

            var coll = _colleagueService.GetColleague(meeting.ColleagueId);
            var mgr = _colleagueService.GetColleague(meeting.ManagerId);

            MeetingView meetingView = meeting.ToMeetingView();
            meetingView.ColleagueName = (coll == null) ? "-" : coll.FirstName + " " + coll.LastName;
            meetingView.ManagerName = (mgr == null) ? "-" : mgr.FirstName + " " + mgr.LastName;

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

            meetingView.Questions = question;

            return meetingView;
        }

        // create new meeting
        public MeetingView CreateMeeting(string colleagueId)
        {
            throw new NotImplementedException();
            // TODO Implement
            //// Get meeting details along with manager details
            //var meeting = (from e in _db.Employees
            //               where e.ColleagueId == colleagueId
            //               join m in _db.Employees on e.ManagerId equals m.ColleagueId into m_join
            //               from m in m_join.DefaultIfEmpty()
            //               select new MeetingView
            //               {
            //                   MeetingId = 0,
            //                   MeetingDate = DateTime.Now,
            //                   EmployeeId = e.Id,
            //                   ColleagueId = e.ColleagueId,
            //                   ColleagueName = string.Concat(e.FirstName, " " + e.LastName),
            //                   ManagerId = m.ColleagueId,
            //                   ManagerName = string.Concat(m.FirstName, " " + m.LastName),
            //                   ColleagueSignOff = 0,
            //                   ManagerSignOff = 0,
            //               }).FirstOrDefault();

            ////Get questions with answers for particular meeting
            //var question = from q in _db.Questions
            //               select new QuestionView
            //               {
            //                   QuestionId = q.Id,
            //                   Question = q.Description,
            //                   QuestionType = q.QuestionType
            //               };

            //if (meeting != null)
            //    meeting.Questions = question;

            //return meeting;
        }

        // save new meeting
        public int SaveMeeting(MeetingView view)
        {
            throw new NotImplementedException();
            // TODO Implement

            //int empId = _db.Employees.Where(e => e.ColleagueId == view.ColleagueId).Select(e => e.Id).FirstOrDefault();

            //var meeting = new LinkMeeting
            //{
            //    EmployeeId = empId,
            //    MeetingDate = view.MeetingDate,
            //    ManagerId = view.ManagerId,
            //    ColleagueSignOff = view.ColleagueSignOff,
            //    ManagerSignOff = view.ManagerSignOff
            //};

            //var result = _db.Meeting.Add(meeting);
            //_db.SaveChanges();

            //foreach (var answer in view.Questions.Select(answer => new Answer
            //{
            //    ColleagueComments = answer.ColleagueComment,
            //    ManagerComments = answer.ManagerComment,
            //    QuestionId = answer.QuestionId,
            //    LinkMeetingId = result.Id,
            //    Discussed = answer.Discussed
            //}))
            //{
            //    _db.Answers.Add(answer);
            //    _db.SaveChanges();
            //}

            //return result.Id;
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
                    ColleagueId = meeting.ColleagueId,
                    ManagerId = meeting.ManagerId,
                    Id = view.MeetingId
                };
                _db.Meeting.AddOrUpdate(linkMeeting);
                _db.SaveChanges();
            }

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
                        answer.Discussed = question.Discussed.HasValue && question.Discussed.Value;

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
            var team = _colleagueService.GetDirectReports(managerId);

            var teamView = new List<TeamView>();

            foreach (ColleagueView colleague in team)
            {
                teamView.Add(GetMeetings(colleague.ColleagueId));
            }
            return teamView;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }


}
