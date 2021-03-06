﻿using System.Collections.Generic;
using System.Web.Http.Results;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Service.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsPlc.Ssc.Link.Service.Tests.Controllers
{
    [TestClass]
    public class QuestionsControllerTests:RepositoryMock
    {
        [TestMethod]
        public void GetQuestions()
        {
            var controller= new QuestionsController(Repository);
            var response = controller.GetQuestions() as OkNegotiatedContentResult<IEnumerable<Question>>;
            Assert.IsNotNull(response);
        }
    }
}
