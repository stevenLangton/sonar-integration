using System.Collections.Generic;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsPlc.Ssc.Link.Service.Controllers;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Service.Tests.Controllers
{
    [TestClass]
    public class QuestionsControllerTests
    {
        [TestMethod]
        public void GetQuestions()
        {
            var controller= new QuestionsController();
            var response = controller.GetQuestions() as OkNegotiatedContentResult<IEnumerable<Question>>;
            Assert.IsNull(response);
        }
    }
}
