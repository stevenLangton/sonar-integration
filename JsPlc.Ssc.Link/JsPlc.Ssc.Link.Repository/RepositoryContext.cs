using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Common;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Interfaces;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Repository
{
    public class RepositoryContext:DbContext,IRepositoryContext
    {
        //public IDbSet<LinkUser> LinkUsers { get; set; }

        public IDbSet<Question> Questions { get; set; }

        public IDbSet<Answer> Answers { get; set; }

        //public IDbSet<Models.Employee> Employees { get; set; } // NOW LinkUser
 
        public IDbSet<Period> Periods { get; set; }

        public IDbSet<LinkMeeting> Meeting { get; set; }

        public IDbSet<LinkObjective> Objectives { get; set; }

        //public IDbSet<Pdp> Pdps { get; set; }

        public RepositoryContext() : base("name=LinkRepository") { }

        public RepositoryContext(DbConnection connection) : base(connection, true) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();   
        }
    }
}
