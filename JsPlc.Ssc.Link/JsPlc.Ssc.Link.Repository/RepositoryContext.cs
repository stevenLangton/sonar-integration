using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Common;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Repository
{
    public class RepositoryContext:DbContext,IRepositoryContext
    {
        public IDbSet<Question> Questions { get; set; }

        public IDbSet<Answer> Answers { get; set; }

        public IDbSet<Employee> Employees { get; set; }

        public IDbSet<Period> Periods { get; set; }

        public IDbSet<User> Users { get; set; }

        public IDbSet<LinkMeeting> Meeting { get; set; }

        
        public RepositoryContext() : base("name=LinkRepository") { }

        public RepositoryContext(DbConnection connection) : base(connection, true) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();   
        }
    }
}
