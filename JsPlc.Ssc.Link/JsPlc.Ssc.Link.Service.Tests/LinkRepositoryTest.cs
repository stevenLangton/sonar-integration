using Effort;
using System.Collections.Generic;
using System.Linq;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Repository;
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
            Assert.AreEqual(5,result.Count(), "Wrong number of questions");
        }

        [TestMethod]
        public void GetMeeting() { }
        
        [TestMethod]
        public void CreateMeeting() { }

        [TestMethod]
        public void SaveMeeting() { }

        [TestMethod]
        public void UpdateMeeting() { }

        [TestMethod]
        public void GetEmployee() { }

        [TestMethod]
        public void GetMeetings() { }

        [TestMethod]
        public void IsManager() { }

        [TestMethod]
        public void GetTeam() { }
    }
}
