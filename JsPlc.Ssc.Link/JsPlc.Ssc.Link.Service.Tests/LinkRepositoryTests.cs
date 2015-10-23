using System;
using System.Collections.Generic;
using System.Linq;
using JsPlc.Ssc.Link.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsPlc.Ssc.Link.Service.Tests
{
    [TestClass]
    public class LinkRepositoryTest : RepositoryMock
    {

        [TestMethod]
        public void GetQuestions()
        {
            var result = _repository.GetQuestions();
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count(), "Wrong number of questions");
        }

        [TestMethod]
        public void GetMeeting()
        {
            var result = _repository.GetMeeting(1);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result,typeof(MeetingView),"Not expected type");
        }

        [TestMethod]
        public void CreateMeeting()
        {
            var result = _repository.CreateMeeting("E001");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MeetingView), "Not expected type");
        }

        [TestMethod]
        public void SaveMeeting()
        {
            var meeting = new MeetingView()
            {
                MeetingId = 0,
                EmployeeId = 1,
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

            int meetingId = _repository.SaveMeeting(meeting);
            Assert.AreEqual(meetingId,5,"Invalid number of meetings");
        }

        [TestMethod]
        public void UpdateMeeting()
        {
            var meeting = new MeetingView()
            {
                MeetingId = 1,
                EmployeeId = 1,
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

            _repository.UpdateMeeting(meeting);
            //var meeting = _repository.GetMeeting(1);

            //Assert.AreEqual(meeting.co);
        }

        [TestMethod]
        public void GetEmployee()
        {

            var result = _repository.CreateMeeting("E001");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MeetingView), "Not expected type");
        }

        [TestMethod]
        public void GetMeetings()
        {

            var result = _repository.GetMeetings("E001");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(TeamView), "Not meetings found");
        }

        [TestMethod]
        public void IsManager()
        {
            bool result = _repository.IsManager("vasundhara.b@sainsburys.co.uk");
            Assert.IsFalse(result);

            result = _repository.IsManager("sandip.v@sainsburys.co.uk");
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetTeam()
        {
            var result = _repository.GetTeam("E0010");
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<TeamView>), "Not meetings found");
        }
    }
}
