using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http.Results;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Repository;
using JsPlc.Ssc.Link.Service.Controllers;
using JsPlc.Ssc.Link.Service.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsPlc.Ssc.Link.Models.Entities;
using Moq;
using NMemory;
using NMemory.Linq;

namespace JsPlc.Ssc.Link.Service.Tests.Controllers
{
    [TestClass]
    public class RepositoryContextTests : RepositoryMock
    {

        [TestInitialize]
        public void RepositoryContextTestsSetup()
        {
            // Test Initialize/Setup
        }

        [TestMethod]
        public void RepositoryContextTestSeed()
        {
            // Arrange
            RepositoryInitializer ri = new RepositoryInitializer();

            // Act
            foreach (var period in _context.Periods)
            {
                _context.Periods.Remove(period);
            }
            //foreach (var objective in _context.Objectives)
            //{
            //    _context.Objectives.Remove(objective);
            //}
            ri.SeedTheDb(_context);
            
            //Assert
            Assert.IsTrue(_context.Periods.Count() == 12);
            Assert.IsTrue(_context.Objectives.Count() == 5);
            var firstObjective = _context.Objectives.FirstOrDefault();
            Assert.IsTrue(firstObjective != null && firstObjective.Title.Equals("Be superman"));
        }

    }
}
