using System;
using System.Data.Entity;
using System.Collections.Generic;
using JsPlc.Ssc.Link.StubService.StubModels;

namespace JsPlc.Ssc.Link.StubService.StubRepository
{
    public class StubRepositoryInitializer : DropCreateDatabaseIfModelChanges<StubRepositoryContext>
    {
        protected override void Seed(StubRepositoryContext context)
        {
            // ColleagueIds should match with RepositoryInitializer in Service project
            var employees=new List<StubColleague>{
                // Users in Azure Directory, Part before @ should match.
                // (e.g. Luan.Au@LinkTool.onmicrosoft.com) in Azure Dir LinkTool.onmicrosoft.com
                new StubColleague{Id=1, FirstName="Luan", LastName="Au", ColleagueId="E001", ManagerId="E003", 
                    EmailAddress="Luan.Au@domain.com"},
                new StubColleague{Id=2, FirstName="Parveen", LastName="Kumar", ColleagueId="E002", ManagerId="E003", 
                    EmailAddress="Parveen.Kumar@domain.com"},
                new StubColleague{Id=3, FirstName="Steven", LastName="Farkas", ColleagueId="E003", ManagerId="E0010", 
                    EmailAddress="Steven.Farkas@domain.com"},
                new StubColleague{Id=4, FirstName="Vasundhara", LastName="Chimakurthi", ColleagueId= "E004", ManagerId="E0010", 
                    EmailAddress="Vasundhara.B@domain.com"},

                new StubColleague{Id=5, FirstName="Laxmi", LastName="Nagaraja", ColleagueId="E005", ManagerId="E009", 
                    EmailAddress="Lakshmi.Nagaraja@domain.com"},
                new StubColleague{Id=6, FirstName="Tim", LastName="Morrison", ColleagueId="E006", ManagerId="E009", 
                    EmailAddress="Tim.Morrison@domain.com"},
                new StubColleague{Id=7, FirstName="Chris", LastName="Atkin", ColleagueId="E007", ManagerId="E009", 
                    EmailAddress="Chris.Atkin@domain.com"},
                new StubColleague{Id=8, FirstName="Anis", LastName="Batilwala", ColleagueId="E008", ManagerId="E009", 
                    EmailAddress="Anis.Batliwala@domain.com"},

                new StubColleague{Id=10, FirstName="Amardip", LastName="Shoker", ColleagueId="E009", ManagerId="E0011", 
                    EmailAddress="Amardip.Shoker@domain.com"},
                new StubColleague{Id=10, FirstName="Sandip", LastName="Vaidya", ColleagueId="E0010", ManagerId="E0011", 
                    EmailAddress="Sandip.Vaidya@domain.com"},
                new StubColleague{Id=11, FirstName="Mike", LastName="Gwyer",  ColleagueId="E0011", ManagerId=string.Empty, 
                    EmailAddress="Mike.Gwyer@domain.com"},

                new StubColleague{Id=12, FirstName="Ridley", LastName="Scott",  ColleagueId="E0012", ManagerId=string.Empty, 
                    EmailAddress="ridley.scott@domain.com"},
                new StubColleague{Id=13, FirstName="Joss", LastName="Whedon",  ColleagueId="E0013", ManagerId=string.Empty, 
                    EmailAddress="joss.whedon@domain.com"},
                new StubColleague{Id=14, FirstName="Rick", LastName="Deckard",  ColleagueId="E0014", ManagerId="E0012", 
                    EmailAddress="rick.deckard@domain.com"},
                new StubColleague{Id=15, FirstName="Ellen", LastName="Ripley",  ColleagueId="E0015", ManagerId="E0012", 
                    EmailAddress="ellen.ripley@domain.com"},
                new StubColleague{Id=16, FirstName="Malcolm", LastName="Reynolds",  ColleagueId="E0016", ManagerId="E0013", 
                    EmailAddress="malcolm.reynolds@domain.com"},
            };

            employees.ForEach(c=>context.Colleagues.Add(c));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
