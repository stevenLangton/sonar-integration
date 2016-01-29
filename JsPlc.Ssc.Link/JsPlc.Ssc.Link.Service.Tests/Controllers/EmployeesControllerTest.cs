using System.Collections.Generic;
using System.Web.Http.Results;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Service.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Service.Tests.Controllers
{
    [TestClass]
    public class EmployeesControllerTest:RepositoryMock
    {
        [TestMethod]
        public void GetEmployeeDetails()
        {
            var controller=new ColleagueController(Repository);
            var result = controller.GetMyDetailsByEmail("vasundhara.b@sainsburys.co.uk") as OkNegotiatedContentResult<LinkUserView>;
            Assert.IsNotNull(result,"Invalid user no record found or email address is wrong");
        }

        [TestMethod]
        public void GetMyMeetings()
        {
            //var controller = new ColleagueController();
            //var result = controller.GetMyMeetings("E001") as OkNegotiatedContentResult<ColleagueTeamView>;
            //Assert.IsNotNull(result,"failed as  no meetigns found");
        }

        [TestMethod]
        public void IsManager()
        {
            var controller = new ColleagueController(Repository);
            var result = controller.IsManagerByEmail("vasundhara.b@sainsburys.co.uk");
            Assert.IsFalse(result,"failed as he / she is Manager");

            result = controller.IsManagerByEmail("sandip.v@sainsburys.co.uk");
            Assert.IsTrue(result,"failed as email is wrong or not a manager");
        }

        [TestMethod]
        public void GetMyTeam()
        {
            //var controller = new ColleagueController(Repository);
            //var result = controller.GetMyTeam("E0010") as OkNegotiatedContentResult<IList<ColleagueTeamView>>;
            //Assert.IsNotNull(result, "No team members found");
        }
    }
}
