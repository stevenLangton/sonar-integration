using Effort;
using JsPlc.Ssc.Link.Repository;
using JsPlc.Ssc.Link.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JsPlc.Ssc.Link.Interfaces.Services;
using JsPlc.Ssc.Link.Models.Entities;
using JsPlc.Ssc.Link.Service.Services;

namespace JsPlc.Ssc.Link.Service.Tests
{
    [TestClass]
    public class RepositoryMock
    {
        protected static ILinkRepository _repository;
        protected static IMeetingService _meeting;
        protected static IObjectivesService _objective;

        protected static RepositoryContext _context;

        public static ILinkRepository Repository
        {
            get { return _repository ?? (_repository = new LinkRepository(_context)); }
        }

        public static IMeetingService Meeting
        {
            get { return _meeting ?? (_meeting = new MeetingService(_context)); }
        }

        public static IObjectivesService Objective
        {
            get { return _objective ?? (_objective = new ObjectivesService(_context)); }
        }

        public static RepositoryContext Context
        {
            get
            {
                if (_context != null) return _context;

                _context = new RepositoryContext(DbConnectionFactory.CreateTransient());
                _context = ContextSetup.MockContext(_context);

                return _context;
            }
        }

        [AssemblyInitialize]
        public static void InitializeContext(TestContext context)
        {
            _context = Context;
            _repository = Repository;
            _meeting = Meeting;
            _objective = Objective;

        }
    }
}
