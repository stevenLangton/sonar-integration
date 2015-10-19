using System.Collections.Generic;
using System.Web.Http.Results;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Service.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsPlc.Ssc.Link.Service.Tests.Controllers
{
    [TestClass]
    public class EmployeesControllerTest:RepositoryMock
    {
        [TestMethod]
        public void GetEmployeeDetails()
        {
            var controller=new EmployeesController(Repository);
            var result = controller.GetMyDetails("vasundhara.b@sainsburys.co.uk") as OkNegotiatedContentResult<Employee>;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetMyMeetings()
        {

        }

        [TestMethod]
        public void IsManager()
        {

        }

        [TestMethod]
        public void GetMyTeam()
        {

        }
    }
}
