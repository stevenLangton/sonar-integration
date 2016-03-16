using System;
using System.Collections.Generic;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Service.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsPlc.Ssc.Link.Models.Entities;
using Moq;

namespace JsPlc.Ssc.Link.Service.Tests.Controllers
{
    // Test that the methods in ColleaguePdpServices..
    [TestClass]
    public class ColleaguePdpServiceTest : RepositoryMock
    {
        ColleaguePdpService _service;
        private Mock<IConfigurationDataService> _mockConfigurationDataService;

        [TestInitialize]
        public void ColleaguePdpServiceTestSetup()
        {
            // Test Initialize/Setup
            _mockConfigurationDataService = new Mock<IConfigurationDataService>();

            _service = new ColleaguePdpService(_context, _mockConfigurationDataService.Object);
        }

        [TestMethod]
        public void GetPdpPeriodicityReturnsNullWhenNonEnumValue()
        {
            // Arrange
            var mockConfigMethod = _mockConfigurationDataService.Setup(
                service => service.GetConfigSettingValue("PdpDefinedPeriodicity"))
                .Returns("ByCurrentDate");
            mockConfigMethod.Verifiable();

            // Act
            var result = _service.GetPdpPeriodicity();

            // Assert
            Assert.IsNull(result, "GetPdpPeriodicity should return null when config key 'PdpDefinedPeriodicity' has value which is not of enum PeriodType");
            _mockConfigurationDataService.Verify(service => service.GetConfigSettingValue("PdpDefinedPeriodicity"), Times.AtLeastOnce(), "Didnt call get config key value PdpDefinedPeriodicity");
        }

        [TestMethod]
        public void GetPdpPeriodicityReturnsValidEnum()
        {
            // Arrange
            var mockConfigMethod = _mockConfigurationDataService.Setup(
                service => service.GetConfigSettingValue("PdpDefinedPeriodicity"))
                .Returns("Year");
            mockConfigMethod.Verifiable();

            // Act
            var result = _service.GetPdpPeriodicity();

            // Assert
            Assert.IsNotNull(result, "GetPdpPeriodicity should return valid enum when config key 'PdpDefinedPeriodicity' has value which is of enum PeriodType");
            Assert.IsTrue(result == PeriodType.Year, "GetPdpPeriodicity expected to parse as valid enum of PeriodType");
            _mockConfigurationDataService.Verify(service => service.GetConfigSettingValue("PdpDefinedPeriodicity"), Times.AtLeastOnce(), "Didnt call get config key value PdpDefinedPeriodicity");
        }

        [TestMethod]
        public void GetPdPVersionReturnsV1()
        {
            // Act
            var colleaguePdp = _service.GetPdp("E001", null, DateTime.Parse("2015-01-03"));
            
            // Assert
            Assert.IsTrue(colleaguePdp.PdpVersion.VersionName.Equals("PDPV1"), "Expected PDPV1 got :" + colleaguePdp.PdpVersion.VersionName);
            Assert.IsTrue(colleaguePdp.PdpVersion.Sections.Count == 3, "Expected 3 sections got :" + colleaguePdp.PdpVersion.Sections.Count);
        }

        [TestMethod]
        public void GetPdPVersionReturnsV2()
        {
            // Act
            var colleaguePdp = _service.GetPdp("E001", null, DateTime.Parse("2030-02-01"));

            // Assert
            Assert.IsTrue(colleaguePdp.PdpVersion.VersionName.Equals("PDPV2"), "Expected PDPV2 got :" + colleaguePdp.PdpVersion.VersionName);
        }

        [TestMethod]
        public void GetPdPVersionReturnsNewPdpTemplate()
        {
            // Act
            var colleaguePdp = _service.GetPdp("E001", -1);

            // Assert
            Assert.IsTrue(colleaguePdp.Id.Equals(0), "Expected new pdp when not match, Got :" + colleaguePdp);
        }

        [TestMethod]
        public void GetPdPWithIdReturnsPdp()
        {
            // Act
            var colleaguePdp = _service.GetPdp(1);

            // Assert
            Assert.IsTrue(colleaguePdp.Id.Equals(1), "Expected correct CollPdp returned, Got :" + colleaguePdp);
        }

        [TestMethod]
        public void GetPdpVersionByPeriodReturnsV1()
        {
            // Act
            var pdpVer1 = _service.GetPdpVersionByPeriod(new Period
            {
                Start = DateTime.Parse("2015-02-01"),
                End = DateTime.Parse("2020-02-01")
            }, PeriodType.Year);

            // Assert
            Assert.IsTrue(pdpVer1.VersionName.Equals("PDPV1"), "Expected PDPV1 got :" + pdpVer1.VersionName);
        }

        [TestMethod]
        public void GetPdpVersionByPeriodReturnsV2()
        {
            // Act
            var pdpVer2 = _service.GetPdpVersionByPeriod(new Period
            {
                Start = DateTime.Parse("2025-01-01"),
                End = DateTime.Parse("2025-02-01")
            }, PeriodType.Year);

            // Assert
            Assert.IsTrue(pdpVer2.VersionName.Equals("PDPV2"), "Expected PDPV2 got :" + pdpVer2.VersionName);
        }

        [TestMethod]
        public void GetPdpVersionByDateReturnsV1()
        {
            // Act
            var pdpVer1 = _service.GetPdpVersionByDate(DateTime.Parse("2015-01-01"));

            // Assert
            Assert.IsTrue(pdpVer1.VersionName.Equals("PDPV1"), "Expected PDPV1 got :" + pdpVer1.VersionName);
        }

        [TestMethod]
        public void GetPdpVersionByDateReturnsV2()
        {
            // Act
            var pdpVer2 = _service.GetPdpVersionByDate(DateTime.Parse("2025-01-01"));

            // Assert
            Assert.IsTrue(pdpVer2.VersionName.Equals("PDPV2"), "Expected PDPV2 got :" + pdpVer2.VersionName);
        }

        [TestMethod]
        public void GetCurrentlyApplicablePdpPeriodReturnsQtr()
        {
            // Act
            var periodQuarter = _service.GetCurrentlyApplicablePdpPeriod(PeriodType.Quarter, DateTime.Parse("2016-02-01"));

            // Assert
            Assert.IsTrue(periodQuarter.Description.Equals("Q4"), "Expected Q4 got :" + periodQuarter.Description);
        }
        
    }
}
