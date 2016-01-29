using System.Collections.Generic;
using System.Web.Http.Results;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Service.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using JsPlc.Ssc.Link.Models.Entities;

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
            var response = controller.GetAllObjectives(ColleagueId) as OkNegotiatedContentResult<List<LinkObjective>>;
            List<LinkObjective> ObjectivesList = response.Content as List<LinkObjective>;
            Assert.IsNotNull(response);
        }
    }
}
