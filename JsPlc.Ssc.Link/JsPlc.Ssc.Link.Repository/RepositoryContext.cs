using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Common;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Repository
{
    public class RepositoryContext:DbContext,IRepositoryContext
    {
        public IDbSet<Models.Question> Questions { get; set; }

        public IDbSet<Models.Answer> Answers { get; set; }

        public IDbSet<Models.Employee> Employees { get; set; }

        public IDbSet<Models.Period> Periods { get; set; }

        public IDbSet<Models.LinkMeeting> Meeting { get; set; }

        public IDbSet<Models.Objectives> Objectives { get; set; }

        public RepositoryContext() : base("name=LinkRepository") { }

        public RepositoryContext(DbConnection connection) : base(connection, true) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();   
        }
    }
}
