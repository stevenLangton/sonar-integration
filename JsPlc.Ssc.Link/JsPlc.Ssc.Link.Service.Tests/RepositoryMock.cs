using Effort;
using JsPlc.Ssc.Link.Repository;
using JsPlc.Ssc.Link.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsPlc.Ssc.Link.Service.Tests
{
    [TestClass]
    public class RepositoryMock
    {
        protected static ILinkRepository _repository;
        protected static IMeeting _meeting;
        protected static IObjectives _objectives;

        protected static RepositoryContext _context;

        public static ILinkRepository Repository
        {
            get { return _repository ?? (_repository = new LinkRepository(_context)); }
        }

        public static IMeeting Meeting
        {
            get { return _meeting ?? (_meeting = new Meeting(_context)); }
        }

        public static IObjectives Objectives

        {
            get { return _objectives ?? (_objectives = new LinkObjectives(_context)); }
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
            _objectives = Objectives;

        }
    }
}
