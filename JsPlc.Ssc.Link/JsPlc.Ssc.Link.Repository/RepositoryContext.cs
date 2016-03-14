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

        public IDbSet<LinkPdp> Pdp { get; set; }

        public IDbSet<Section> Sections { get; set; }
        public IDbSet<PdpVersion> PdpVersions { get; set; }
        public IDbSet<PdpSection> PdpSections { get; set; }
        public IDbSet<PdpSectionQuestion> PdpSectionQuestions { get; set; }

        public IDbSet<ColleaguePdp> ColleaguePdps { get; set; }
        public IDbSet<ColleaguePdpSectionInstance> ColleaguePdpSectionInstances { get; set; }
        public IDbSet<ColleaguePdpAnswer> ColleaguePdpAnswers { get; set; }
        
        public RepositoryContext() : base("name=LinkRepository") { }

        public RepositoryContext(DbConnection connection) : base(connection, true) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<PdpSection>()
            //    .HasRequired(p => p.PdpVersion)
            //    .WithRequiredDependent()
            //    .Map(m => m.MapKey("PdpVersionId"));
        }
    }
}
