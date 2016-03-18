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
                new StubColleague{Id=11, FirstName="Mike", LastName="Gwyer",  ColleagueId="E0011", ManagerId="E0017", 
                    EmailAddress="Mike.Gwyer@domain.com"},

                //stakeholders
                new StubColleague{Id=12, FirstName="Ridley", LastName="Scott",  ColleagueId="E0012", ManagerId="E0017", 
                    EmailAddress="ridley.scott@domain.com"},
                new StubColleague{Id=13, FirstName="Joss", LastName="Whedon",  ColleagueId="E0013", ManagerId="E0017", 
                    EmailAddress="joss.whedon@domain.com"},
                new StubColleague{Id=14, FirstName="Rick", LastName="Deckard",  ColleagueId="E0014", ManagerId="E0012", 
                    EmailAddress="rick.deckard@domain.com"},
                new StubColleague{Id=15, FirstName="Ellen", LastName="Ripley",  ColleagueId="E0015", ManagerId="E0012", 
                    EmailAddress="ellen.ripley@domain.com"},
                new StubColleague{Id=16, FirstName="Malcolm", LastName="Reynolds",  ColleagueId="E0016", ManagerId="E0013", 
                    EmailAddress="malcolm.reynolds@domain.com"},

                    //Devops
                     new StubColleague{Id=17, FirstName="Simon", LastName="Parsons",  ColleagueId="E0017", ManagerId="E0011", 
                    EmailAddress="simon.parsons@domain.com"},
                     new StubColleague{Id=18, FirstName="TR", LastName="Thambi",  ColleagueId="E0018", ManagerId="E0011", 
                    EmailAddress="tr.thambi@domain.com"},
                     new StubColleague{Id=19, FirstName="Arkadiusz", LastName="Goral",  ColleagueId="E0019", ManagerId="E0011", 
                    EmailAddress="Arkadiusz.Goral@domain.com"},

                    //PEN test users
                    new StubColleague{Id=24, FirstName="PenTest", LastName="Consultant1",  ColleagueId="E0400", ManagerId="E0011", 
                    EmailAddress="PenTest.Consultant1@domain.com"},

                    new StubColleague{Id=20, FirstName="PenTest", LastName="Consultant2",  ColleagueId="E0100", ManagerId="E0400", 
                    EmailAddress="PenTest.Consultant2@domain.com"},

                    new StubColleague{Id=21, FirstName="PenTest", LastName="Consultant3",  ColleagueId="E0200", ManagerId="E0300", 
                    EmailAddress="PenTest.Consultant3@domain.com"},

                     new StubColleague{Id=23, FirstName="PenTest", LastName="Consultant4",  ColleagueId="E0300", ManagerId="E0011", 
                    EmailAddress="PenTest.Consultant4@domain.com"},

                    //UX Test Users
                    new StubColleague{Id=24, FirstName="colleague", LastName="1",  ColleagueId="U0001", ManagerId="U0013", 
                    EmailAddress="colleague.1@domain.com"},
                    new StubColleague{Id=25, FirstName="colleague", LastName="2",  ColleagueId="U0002", ManagerId="U0014", 
                    EmailAddress="colleague.2@domain.com"},
                    new StubColleague{Id=26, FirstName="colleague", LastName="3",  ColleagueId="U0003", ManagerId="U0015", 
                    EmailAddress="colleague.3@domain.com"},
                    new StubColleague{Id=27, FirstName="colleague", LastName="4",  ColleagueId="U0004", ManagerId="U0016", 
                    EmailAddress="colleague.4@domain.com"},
                    new StubColleague{Id=28, FirstName="colleague", LastName="5",  ColleagueId="U0005", ManagerId="U0017", 
                    EmailAddress="colleague.5@domain.com"},
                    new StubColleague{Id=29, FirstName="colleague", LastName="6",  ColleagueId="U0006", ManagerId="U0018", 
                    EmailAddress="colleague.6@domain.com"},
                    new StubColleague{Id=30, FirstName="colleague", LastName="7",  ColleagueId="U0007", ManagerId="U0019", 
                    EmailAddress="colleague.7@domain.com"},
                    new StubColleague{Id=31, FirstName="colleague", LastName="8",  ColleagueId="U0008", ManagerId="U0020", 
                    EmailAddress="colleague.8@domain.com"},
                    new StubColleague{Id=32, FirstName="colleague", LastName="9",  ColleagueId="U0009", ManagerId="U0021", 
                    EmailAddress="colleague.9@domain.com"},
                    new StubColleague{Id=33, FirstName="colleague", LastName="10",  ColleagueId="U0010", ManagerId="U0012", 
                    EmailAddress="colleague.10@domain.com"},
                    new StubColleague{Id=34, FirstName="colleague", LastName="11",  ColleagueId="U0011", ManagerId="U0012", 
                    EmailAddress="colleague.11@domain.com"},

                    new StubColleague{Id=24, FirstName="manager", LastName="1",  ColleagueId="U0012", ManagerId="E003", 
                    EmailAddress="manager.1@domain.com"},
                    new StubColleague{Id=25, FirstName="manager", LastName="2",  ColleagueId="U0013", ManagerId="E003", 
                    EmailAddress="manager.2@domain.com"},
                    new StubColleague{Id=26, FirstName="manager", LastName="3",  ColleagueId="U0014", ManagerId="E003", 
                    EmailAddress="manager.3@domain.com"},
                    new StubColleague{Id=27, FirstName="manager", LastName="4",  ColleagueId="U0015", ManagerId="E003", 
                    EmailAddress="manager.4@domain.com"},
                    new StubColleague{Id=28, FirstName="manager", LastName="5",  ColleagueId="U0016", ManagerId="E003", 
                    EmailAddress="manager.5@domain.com"},
                    new StubColleague{Id=29, FirstName="manager", LastName="6",  ColleagueId="U0016", ManagerId="E003", 
                    EmailAddress="manager.6@domain.com"},
                    new StubColleague{Id=30, FirstName="manager", LastName="7",  ColleagueId="U0018", ManagerId="E003", 
                    EmailAddress="manager.7@domain.com"},
                    new StubColleague{Id=31, FirstName="manager", LastName="8",  ColleagueId="U0019", ManagerId="E003", 
                    EmailAddress="manager.8@domain.com"},
                    new StubColleague{Id=32, FirstName="manager", LastName="9",  ColleagueId="U0020", ManagerId="E003", 
                    EmailAddress="manager.9@domain.com"},
                    new StubColleague{Id=33, FirstName="manager", LastName="10",  ColleagueId="U0021", ManagerId="E003", 
                    EmailAddress="manager.10@domain.com"},



                    
            };

            employees.ForEach(c=>context.Colleagues.Add(c));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
