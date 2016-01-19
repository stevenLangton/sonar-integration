using System.Collections.Generic;
using System.Web.Http.Results;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Service.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;

namespace JsPlc.Ssc.Link.Service.Tests.Controllers
{
    [TestClass]
    public class ObjectivesControllerTests
    {
        [TestMethod]
        public void GetAllObjectives()
        {
            var ColleagueId = "E0009";
            var controller = new ObjectivesController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
            var response = controller.GetAllObjectives(ColleagueId) as OkNegotiatedContentResult<List<Objectives>>;
            List<Objectives> ObjectivesList = response.Content as List<Objectives>;
            Assert.IsNotNull(response);
        }
    }
}
