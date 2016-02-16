using System;
using System.Collections.Generic;
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
    public class DomainTranslationTest
    {
        private Mock<IConfigurationDataService> _mockConfigurationDataService;
        //private Mock<IDomainTranslationService> _mockDomainTranslationService;
        private IDomainTranslationService _domainTranslationService;

        [TestInitialize]
        public void DomainTranslationTestSetup()
        {
            // Test Initialize/Setup
            _mockConfigurationDataService = new Mock<IConfigurationDataService>();
            //_mockDomainTranslationService = new Mock<IDomainTranslationService>();

            //IConfigurationDataService configurationDataService = new ConfigurationDataService();
            _domainTranslationService = new DomainTranslationService(_mockConfigurationDataService.Object);
        }

        [TestMethod]
        public void DomainTranslationTestTranlation()
        {
            // Arrange
            _mockConfigurationDataService.Setup(service => service.GetConfigSettingValue("AzureLinkDomain")).Returns("@jsmocktest1.onmicrosoft.com");
            _mockConfigurationDataService.Setup(service => service.GetConfigSettingValue("DbLinkDomain")).Returns("@linktool.onmicrosoft.com");
            // Act
            var colleagueEmail = _domainTranslationService.AdDomainToDbDomain("Luan.Au@jsmocktest1.onmicrosoft.com");
            //Assert
            Assert.IsTrue(colleagueEmail.Equals("Luan.Au@linktool.onmicrosoft.com"));
        }

        [TestMethod]
        public void DomainTranslationTestDefaultTranlation()
        {
            // Arrange
            _mockConfigurationDataService.Setup(service => service.GetConfigSettingValue("AzureLinkDomain")).Returns("@jsmocktest1.onmicrosoft.com");
            _mockConfigurationDataService.Setup(service => service.GetConfigSettingValue("DbLinkDomain")).Returns("");
            // Act
            var colleagueEmail = _domainTranslationService.AdDomainToDbDomain("Luan.Au@jsmocktest1.onmicrosoft.com");
            //Assert
            Assert.IsTrue(colleagueEmail.Equals("Luan.Au@domain.com"));
        }
        [TestMethod]
        public void DomainTranslationTestNoTranslation()
        {
            // Arrange
            _mockConfigurationDataService.Setup(service => service.GetConfigSettingValue("AzureLinkDomain")).Returns("@jsmocktest1.onmicrosoft.com");
            _mockConfigurationDataService.Setup(service => service.GetConfigSettingValue("DbLinkDomain")).Returns("@jsmocktest1.onmicrosoft.com");
            // Act
            var colleagueEmail = _domainTranslationService.AdDomainToDbDomain("Luan.Au@jsmocktest1.onmicrosoft.com");
            //Assert
            Assert.IsTrue(colleagueEmail.Equals("Luan.Au@jsmocktest1.onmicrosoft.com"));
        }

        [TestMethod]
        [ExpectedException(typeof(Elmah.ApplicationException))]
        public void DomainTranslationTestInvalidEmail()
        {
            // Arrange
            _mockConfigurationDataService.Setup(service => service.GetConfigSettingValue("AzureLinkDomain")).Returns("@jsmocktest1.onmicrosoft.com");
            _mockConfigurationDataService.Setup(service => service.GetConfigSettingValue("DbLinkDomain")).Returns("@domain.com");
            // Act
            _domainTranslationService.AdDomainToDbDomain("Luan.Au");

            //Assert that exception is thrown as per attrib
        }

        [TestMethod]
        public void DefaultCtor()
        {
            // Arrange

            // Act
            var dts = new DomainTranslationService();

            //Assert 
            Assert.IsNotNull(dts, "Expected dts to be built, couldnt new DomainTransSvc");
        }
    }
}
