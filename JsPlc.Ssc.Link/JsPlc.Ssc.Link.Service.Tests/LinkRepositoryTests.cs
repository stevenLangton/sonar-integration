using System;
using System.Collections.Generic;
using System.Linq;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Service.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsPlc.Ssc.Link.Models.Entities;
using Moq;

namespace JsPlc.Ssc.Link.Service.Tests
{
    [TestClass]
    public class LinkRepositoryTest : RepositoryMock
    {
        private IMeetingService _meeting;
        private Mock<IColleagueService> _mockColleagueService;

        [TestInitialize]
        public void LinkRepositoryTestSetup()
        {
            // Test Initialize/Setup
            _mockColleagueService = new Mock<IColleagueService>();

            _meeting = new MeetingService(_context, _mockColleagueService.Object);
        }

        [TestMethod]
        public void GetQuestions()
        {
            var result = _meeting.GetQuestions();
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count(), "Wrong number of questions");
        }

        [TestMethod]
        public void GetMeeting()
        {
            _mockColleagueService.Setup(service => service.GetColleague("E001")).Returns(new ColleagueView());
            var result = _meeting.GetMeeting(1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(MeetingView),"Not expected type");
        }

        [TestMethod]
        public void CreateMeeting()
        {
            _mockColleagueService.Setup(service => service.GetColleague("E001")).Returns(new ColleagueView());

            var result = _meeting.CreateMeeting("E001");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MeetingView), "Not expected type");
        }

        [TestMethod]
        public void SaveMeeting()
        {
            _mockColleagueService.Setup(service => service.GetColleague("E001")).Returns(new ColleagueView
            {
                ColleagueId = "E001", ManagerId = "E0011"
            });

            var meeting = new MeetingView()
            {
                MeetingId = 0,
                ColleagueId = "E001",
                ColleagueName = "vasu",
                ManagerId = "E0010",
                ManagerName = "Sandip.V",
                ColleagueSignOff = MeetingStatus.Completed,
                ManagerSignOff = MeetingStatus.Completed,
                MeetingDate = DateTime.Now,
                Questions = new List<QuestionView>()
                   {
                       new QuestionView(){ QuestionId  = 1, QuestionType = "b", Question = "Test1", AnswerId = 1, ColleagueComment = "testing1", ManagerComment = "testing1"},
                       new QuestionView(){ QuestionId  = 2, QuestionType = "b", Question = "Test2", AnswerId = 2, ColleagueComment = "testing2", ManagerComment = "testing2"},
                       new QuestionView(){ QuestionId  = 3, QuestionType = "f", Question = "Test3", AnswerId = 3, ColleagueComment = "testing3", ManagerComment = "testing3"},
                       new QuestionView(){ QuestionId  = 4, QuestionType = "f", Question = "Test4", AnswerId = 4, ColleagueComment = "testing4", ManagerComment = ""},
                       new QuestionView(){ QuestionId  = 5, QuestionType = "f", Question = "Test5", AnswerId = 5, ColleagueComment = "", ManagerComment = ""}
                   }
            };

            var beforeSaveCount = _context.Meeting.Count();

            int meetingId = _meeting.SaveNewMeeting(meeting);
            Assert.AreEqual(meetingId, beforeSaveCount+1,"Invalid number of meetings");
        }

        [TestMethod]
        public void UpdateMeeting()
        {
            var meeting = new MeetingView()
            {
                MeetingId = 1,
                ColleagueId = "E001",
                ColleagueName = "vasu",
                ManagerId = "E0010",
                ManagerName = "Sandip.V",
                ColleagueSignOff = MeetingStatus.Completed,
                ManagerSignOff = MeetingStatus.Completed,
                MeetingDate = DateTime.Now,
                Questions = new List<QuestionView>()
                   {
                       new QuestionView(){ QuestionId  = 1, QuestionType = "b", Question = "Test1", AnswerId = 1, ColleagueComment = "testing update", ManagerComment = "testing1"},
                       new QuestionView(){ QuestionId  = 2, QuestionType = "b", Question = "Test2", AnswerId = 2, ColleagueComment = "testing2", ManagerComment = "testing2"},
                       new QuestionView(){ QuestionId  = 3, QuestionType = "f", Question = "Test3", AnswerId = 3, ColleagueComment = "testing3", ManagerComment = "testing3"},
                       new QuestionView(){ QuestionId  = 4, QuestionType = "f", Question = "Test4", AnswerId = 4, ColleagueComment = "testing4", ManagerComment = ""},
                       new QuestionView(){ QuestionId  = 5, QuestionType = "f", Question = "Test5", AnswerId = 5, ColleagueComment = "", ManagerComment = ""}
                   }
            };

            _meeting.UpdateMeeting(meeting);
            //var meeting = _repository.GetMeeting(1);

            //Assert.AreEqual(meeting.co);
        }

        [TestMethod]
        public void GetNextMeeting()
        {
            var result = _meeting.GetNextMeeting("E0010");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(LinkMeeting), "Not meetings found");
        }

        [TestMethod]
        public void IsManager()
        {
            //bool result = _repository.IsManager("vasundhara.b@sainsburys.co.uk");
            //Assert.IsFalse(result);

            //result = _repository.IsManager("sandip.v@sainsburys.co.uk");
            //Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetTeam()
        {
            _mockColleagueService.Setup(service => service.GetDirectReports("E0010")).Returns(new List<ColleagueView>
            {
                new ColleagueView
                {
                    ColleagueId = "E001",
                },
                new ColleagueView
                {
                    ColleagueId = "E002",
                }
            });
            _mockColleagueService.Setup(service => service.GetColleague("E001")).Returns(new ColleagueView
            {
                ColleagueId = "E001",
                ManagerId = "E0010"
            });

            _mockColleagueService.Setup(service => service.GetColleague("E002")).Returns(new ColleagueView
            {
                ColleagueId = "E002",
                ManagerId = "E0010"
            });

            var result = _meeting.GetTeam("E0010");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<ColleagueTeamView>), "Not meetings found");
        }
    }
}
