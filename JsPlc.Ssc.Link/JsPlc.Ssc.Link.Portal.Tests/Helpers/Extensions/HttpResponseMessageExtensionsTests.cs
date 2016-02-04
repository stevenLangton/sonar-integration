using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JsPlc.Ssc.Link.Portal.Helpers;
using JsPlc.Ssc.Link.Portal.Helpers.Extensions;
using JsPlc.Ssc.Link.Portal.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsPlc.Ssc.Link.Portal.Tests
{
    [TestClass()]
    public class HttpResponseMessageExtensionsTests
    {
        [TestMethod()]
        public void HttpResponseMessageExtensionsTest()
        {
            var msg = new HttpResponseMessage();
           
            object jsonData = new LinkForm() {Id = 0, MeetingDate = DateTime.Parse("2016-01-30"), Completed = true, Questions = null };
            object modelErrors = new List<string>(){"a", "b"};
            var outMsg = msg.ToJsonResult(jsonData, modelErrors, "testCustomStatusCode");

            var verifyData = outMsg.Data as LinkJsonData;
            Assert.IsNotNull(verifyData);
            Assert.IsNotNull(verifyData.JsonObject as LinkForm);
            Assert.IsNotNull(verifyData.ModelErrors as List<string>);
        }
    }
}
