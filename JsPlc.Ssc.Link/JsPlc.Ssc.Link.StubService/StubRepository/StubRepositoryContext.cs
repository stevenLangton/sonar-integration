using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Common;
using JsPlc.Ssc.Link.StubService.StubInterfaces;
using JsPlc.Ssc.Link.StubService.StubModels;

namespace JsPlc.Ssc.Link.StubService.StubRepository
{
    public class StubRepositoryContext: DbContext, IStubRepositoryContext
    {
        public IDbSet<StubColleague> Colleagues { get; set; }

        public StubRepositoryContext() : base("name=StubLinkRepository") { }

        public StubRepositoryContext(DbConnection connection) : base(connection, true) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();   
        }
    }
}
