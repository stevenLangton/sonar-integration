using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using JsPlc.Ssc.Link.Portal.Helpers;
using JsPlc.Ssc.Link.Portal.Helpers.Extensions;
using JsPlc.Ssc.Link.Portal.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelState = System.Web.Http.ModelBinding.ModelState;

namespace JsPlc.Ssc.Link.Portal.Helpers.Tests
{
    [TestClass()]
    public class ModelStateHelperTests
    {
        [TestMethod()]
        public void ModelStateHelperErrorsTest()
        {
            var modelState = new System.Web.Mvc.ModelState();
            modelState.Value = new ValueProviderResult("", "", new CultureInfo("en-gb"));
            modelState.Errors.Add(@"modelStateError");

            var dict = new ModelStateDictionary { new KeyValuePair<string, System.Web.Mvc.ModelState>("", modelState) };

            var outErrors = dict.Errors();

            var keyValuePairs = outErrors as KeyValuePair<string, string[]>[] ?? outErrors.ToArray();
            Assert.IsNotNull(outErrors);
            Assert.IsTrue(keyValuePairs.Any());
            Assert.IsTrue(keyValuePairs.First().Value[0].Equals(@"modelStateError"));
        }
    }
}
