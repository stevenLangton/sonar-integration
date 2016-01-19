using Effort;
using JsPlc.Ssc.Link.Interfaces;
using JsPlc.Ssc.Link.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JsPlc.Ssc.Link.Service.Tests
{
    [TestClass]
    public class RepositoryMock
    {
        protected static ILinkRepository _repository;
        protected static RepositoryContext _context;

        public static ILinkRepository Repository
        {
            get { return _repository ?? (_repository = new LinkRepository(_context)); }
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

        }
    }
}
