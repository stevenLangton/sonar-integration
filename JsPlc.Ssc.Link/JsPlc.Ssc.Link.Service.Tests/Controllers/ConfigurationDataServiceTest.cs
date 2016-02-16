using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using System.Web.Http.Results;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Service.Controllers;
using JsPlc.Ssc.Link.Service.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsPlc.Ssc.Link.Models.Entities;
using Moq;

namespace JsPlc.Ssc.Link.Service.Tests.Controllers
{
    [TestClass]
    public class ConfigurationDataServiceTest
    {
        [TestMethod]
        public void ConfigDataSvcTest()
        {
            IConfigurationDataService configurationDataService = new ConfigurationDataService();
            var sampleValue = configurationDataService.GetConfigSettingValue("SampleKey");
            Assert.IsTrue(!String.IsNullOrEmpty(sampleValue), "Unable to get appsetting key value");
        }
    }
}
