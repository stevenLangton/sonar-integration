using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Data.Entity;
using Elmah;
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
            _colleagueService = new ColleagueService(new StubServiceFacade());
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

        /// <summary>
        /// Get the next meeting in the future.
        /// </summary>
        /// <param name="colleagueId"></param>
        /// <returns></returns>
        public LinkMeeting GetNextMeeting(string colleagueId)
        {
            //var meeting = _db.Meeting.Where(x => x.MeetingDate > DateTime.Now).OrderBy(x => x.MeetingDate).First();
            var query = from meeting in _db.Meeting
                        where (meeting.ColleagueId == colleagueId && meeting.MeetingDate > DateTime.Now)
                        orderby (meeting.MeetingDate)
                        select meeting;

            //var FoundMeeting = query.FirstOrDefault();
            //if (FoundMeeting == null) return null;
            return (query.FirstOrDefault());
        }

        // meetings history of an employee
        public ColleagueTeamView GetColleagueAndMeetings(string colleagueId)
        {
            ColleagueView colleague = _colleagueService.GetColleague(colleagueId);
            ColleagueTeamView myReport;

            // TODO Ideally limit past meetings to last 12 months, but no limit on future meetings (not many expected in future)
            myReport = (from m in _db.Meeting
                            .Where(m => m.ColleagueId.Equals(colleagueId))
                            select new ColleagueTeamView
                            {
                                //Colleague = new ColleagueView{ColleagueId = cv},
                                Meetings = (from m1 in _db.Meeting
                                            orderby m1.MeetingDate descending
                                            where m1.ColleagueId == colleague.ColleagueId 
                                            && (
                                                DbFunctions.DiffMonths(m1.MeetingDate, DateTime.Now) <= 12 // Only Past ones within last 12 months
                                                ||
                                                DbFunctions.DiffDays(DateTime.Now, m1.MeetingDate) >= 1 // All Future meetings
                                                )
                                            select new LinkMeetingView
                                            {
                                                MeetingId = m1.Id,
                                                MeetingDate = m1.MeetingDate,
                                                ColleagueSignOff = m1.ColleagueSignOff,
                                                ManagerSignOff = m1.ManagerSignOff,
                                                ColleagueId = m1.ColleagueId,
                                                ManagerAtTimeId = m1.ManagerId,
                                                ColleagueSignedOffDate = m1.ColleagueSignedOffDate,
                                                ManagerSignedOffDate = m1.ManagerSignedOffDate,
                                                SharingStatus = m1.SharingStatus,
                                                SharingDate = m1.SharingDate
                                            }).ToList(),
                            }).FirstOrDefault();

            if (myReport == null)
            {
                return new ColleagueTeamView() {Colleague = colleague};
            }
            myReport.Colleague = colleague;
            foreach (var meeting in myReport.Meetings)
            {
                var mDate = meeting.MeetingDate;

                var period = (from p in _db.Periods
                    where mDate >= p.Start && mDate <= p.End
                    select p).FirstOrDefault();

                if (period == null) continue; // should not occur since each meeting should fall within a period
                meeting.Colleague = _colleagueService.GetColleague(meeting.ColleagueId);
                meeting.ManagerAtTime = _colleagueService.GetColleague(meeting.ManagerAtTimeId);
                meeting.Period = period.Description;
                meeting.Year = period.Year;
            }
            return myReport;
        }

        // view particular meeting
        public MeetingView GetMeeting(int meetingId)
        {
            // Get meeting details along with manager details
            var meeting = _db.Meeting.FirstOrDefault(x => x.Id == meetingId);
            if (meeting == null) return null;

            return GetMeetingView(meeting);

            //var coll = _colleagueService.GetColleague(meeting.ColleagueId);
            //ColleagueView mgr = null;
            //if (coll.HasManager)
            //{
            //    mgr = _colleagueService.GetColleague(meeting.ManagerId);
            //}

            //MeetingView meetingView = meeting.ToMeetingView();
            //meetingView.ColleagueName = coll.FirstName + " " + coll.LastName;
            //meetingView.ManagerName = (mgr == null) ? "-" : mgr.FirstName + " " + mgr.LastName;

            ////Get questions with answers for particular meeting
            //var question = from q in _db.Questions
            //               join a in _db.Answers on new { q.Id, LinkMeetingId = meetingId } equals new { Id = a.QuestionId, a.LinkMeetingId } into a_join
            //               from a in a_join.DefaultIfEmpty()
            //               select new QuestionView
            //               {
            //                   QuestionId = q.Id,
            //                   Question = q.Description,
            //                   QuestionType = q.QuestionType,
            //                   AnswerId = a.Id,
            //                   ColleagueComment = a.ColleagueComments,
            //                   ManagerComment = a.ManagerComments,
            //                   Discussed = a.Discussed
            //               };

            //meetingView.Questions = question;

            //return meetingView;
        }

        // create new meeting object for view, NOT persisted yet.
        public MeetingView CreateMeeting(string colleagueId)
        {
            // Get meeting details along with manager details
            var coll = _colleagueService.GetColleague(colleagueId);
            ColleagueView mgr=null;
            if (coll.HasManager)
            {
                mgr = _colleagueService.GetColleague(coll.ManagerId);
            }
            var meetingView = new MeetingView
            {
                MeetingId = 0,
                MeetingDate = DateTime.Now,
                ColleagueId = coll.ColleagueId,
                ColleagueName = string.Concat(coll.FirstName, " " + coll.LastName),
                Colleague = coll,
                Manager = mgr,
                ManagerId = (mgr == null) ? "" : mgr.ColleagueId,
                ManagerName = (mgr == null) ? "" : string.Concat(mgr.FirstName, " " + mgr.LastName),
                ColleagueSignOff = MeetingStatus.InComplete,
                ManagerSignOff = MeetingStatus.InComplete,
                SharingStatus = MeetingSharingStatus.NotShared,
                SharingDate = null
            };
            //Get questions with answers for particular meeting
            var question = from q in _db.Questions
                           select new QuestionView
                           {
                               QuestionId = q.Id,
                               Question = q.Description,
                               QuestionType = q.QuestionType
                           };

            meetingView.Questions = question;

            return meetingView;
        }

        public MeetingView UnshareMeeting(int id)
        {
            var meeting = _db.Meeting.FirstOrDefault(m => m.Id == id);

            if (meeting != null && meeting.ManagerSignOff != MeetingStatus.Completed)
            {
                meeting.SharingStatus = MeetingSharingStatus.NotShared;
                _db.Meeting.AddOrUpdate(meeting);
                _db.SaveChanges();
            }
            else
            {
                throw new Elmah.ApplicationException("Cannot unshare a meeting which has been approved.");
            }
            return meeting.ToMeetingView();
        }


        public MeetingView ApproveMeeting(int id)
        {
            var meeting = _db.Meeting.FirstOrDefault(m => m.Id == id);

            if (meeting != null 
                && meeting.SharingStatus == MeetingSharingStatus.Shared
                && meeting.ManagerSignOff != MeetingStatus.Completed)
            {
                meeting.ManagerSignOff = MeetingStatus.Completed;
                meeting.ManagerSignedOffDate = DateTime.Now;
                _db.Meeting.AddOrUpdate(meeting);
                _db.SaveChanges();
            }
            else
            {
                throw new Elmah.ApplicationException("Cannot approve a meeting if not shared or already approved.");
            }
            return meeting.ToMeetingView();
        }

        // save new meeting, returns 0 if not saved.
        public int SaveNewMeeting(MeetingView view)
        {
            var colleague = _colleagueService.GetColleague(view.ColleagueId);
            if (colleague == null) return 0;

            var meeting = new LinkMeeting
            {
                ColleagueId = colleague.ColleagueId,
                ManagerId = colleague.ManagerId,
                MeetingDate = view.MeetingDate,
                ColleagueSignOff = view.ColleagueSignOff,
                ManagerSignOff = view.ManagerSignOff,
                SharingStatus = view.SharingStatus,
            };
            if (meeting.ColleagueSignOff == MeetingStatus.Completed)
            {
                meeting.ColleagueSignedOffDate = DateTime.Now;
            }
            if (meeting.ManagerSignOff == MeetingStatus.Completed)
            {
                meeting.ManagerSignedOffDate = DateTime.Now;
            }
            if (meeting.SharingStatus == MeetingSharingStatus.Shared)
            {
                meeting.SharingDate = DateTime.Now;
            }
            var result = _db.Meeting.Add(meeting);
            int saveCount = _db.SaveChanges();

            if (saveCount <= 0) return 0; // cannot proceed to save answers
 
            foreach (var answer in view.Questions.Select(answer => new Answer
            {
                ColleagueComments = answer.ColleagueComment,
                //ManagerComments = answer.ManagerComment, // descoped.. Now only one comments box.
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
                    ColleagueId = meeting.ColleagueId,
                    ManagerId = meeting.ManagerId,
                    SharingStatus = view.SharingStatus,
                    SharingDate = null,
                    Id = view.MeetingId
                };
                if (linkMeeting.ColleagueSignOff == MeetingStatus.Completed)
                {
                    linkMeeting.ColleagueSignedOffDate = DateTime.Now;
                }
                if (linkMeeting.ManagerSignOff == MeetingStatus.Completed)
                {
                    linkMeeting.ManagerSignedOffDate = DateTime.Now;
                }
                if (linkMeeting.SharingStatus == MeetingSharingStatus.Shared)
                {
                    linkMeeting.SharingDate = DateTime.Now;
                }
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
        public IEnumerable<ColleagueTeamView> GetTeam(string managerId)
        {
            var team = _colleagueService.GetDirectReports(managerId);

            var teamView = new List<ColleagueTeamView>();

            foreach (ColleagueView colleague in team)
            {
                teamView.Add(GetColleagueAndMeetings(colleague.ColleagueId));
            }
            return teamView;
        }

        //UNDER TESTING
        public IEnumerable<ColleagueTeamView> GetTeamLastMeetings(string managerId)
        {
            var team = _colleagueService.GetDirectReports(managerId);

            var teamView = new List<ColleagueTeamView>();

            //Currently incorrect. Need to do outer join
            var query = from c in team
                        from m1 in _db.Meeting
                        where m1.ColleagueId.Equals(c.ColleagueId)
                                                && ((DateTime.Now.Month - m1.MeetingDate.Month) <= 12
                                                    || (m1.MeetingDate.Day - DateTime.Now.Day) >= 1)
                        group new { meeting=m1, colleague=c } by new { ColleagueId = m1.ColleagueId } into grp1
                        select grp1;

            foreach (var reportee in query)
            {
                var newestGrp = reportee.OrderByDescending(x => x.meeting.MeetingDate).FirstOrDefault();

                var m1 = newestGrp.meeting;
                var colleague = newestGrp.colleague;

                teamView.Add(new ColleagueTeamView {
                    Colleague = colleague,
                    Meetings = new List<LinkMeetingView>() { new LinkMeetingView {
                                    MeetingId = m1.Id,
                                    MeetingDate = m1.MeetingDate,
                                    ColleagueSignOff = m1.ColleagueSignOff,
                                    ManagerSignOff = m1.ManagerSignOff,
                                    ColleagueId = m1.ColleagueId,
                                    ManagerAtTimeId = m1.ManagerId,
                                    ColleagueSignedOffDate = m1.ColleagueSignedOffDate,
                                    ManagerSignedOffDate = m1.ManagerSignedOffDate,
                                    SharingStatus = m1.SharingStatus,
                                    SharingDate = m1.SharingDate,
                                }}
                });
            }

            return teamView;
        }
        //END UNDER TESTING

        public void Dispose()
        {
            _db.Dispose();
        }

        #region Private methods
        private MeetingView GetMeetingView(LinkMeeting meeting)
        {
            var coll = _colleagueService.GetColleague(meeting.ColleagueId);
            ColleagueView mgr = null;
            if (coll.HasManager)
            {
                mgr = _colleagueService.GetColleague(meeting.ManagerId);
            }

            MeetingView meetingView = meeting.ToMeetingView();
            meetingView.ColleagueName = coll.FirstName + " " + coll.LastName;
            meetingView.Colleague = coll;
            meetingView.Manager = mgr;
            meetingView.ManagerName = (mgr == null) ? "-" : mgr.FirstName + " " + mgr.LastName;

            //Get questions with answers for particular meeting
            var question = from q in _db.Questions
                           join a in _db.Answers on new { q.Id, LinkMeetingId = meeting.Id } equals new { Id = a.QuestionId, a.LinkMeetingId } into a_join
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
        #endregion
    }


}
