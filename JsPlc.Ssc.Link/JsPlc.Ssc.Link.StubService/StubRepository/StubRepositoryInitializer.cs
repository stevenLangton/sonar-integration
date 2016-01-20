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
            var employees=new List<StubColleague>{
                new StubColleague{Id=1, FirstName="Vasundhara", LastName="Chimakurthi", ColleagueId= "E001", ManagerId="E0010", EmailAddress="Vasundhara.B@sainsburys.co.uk"},
                new StubColleague{Id=2, FirstName="Laxmi", LastName="Nagaraja", ColleagueId="E002", ManagerId="E0009", EmailAddress="Laxmi.N@sainsburys.co.uk"},
                new StubColleague{Id=3, FirstName="Tim", LastName="Morrison", ColleagueId="E003", ManagerId="E0009", EmailAddress="Tim.M@sainsburys.co.uk"},
                new StubColleague{Id=4, FirstName="Chris", LastName="Atkin", ColleagueId="E004", ManagerId="E0009", EmailAddress="Chris.A@sainsburys.co.uk"},
                new StubColleague{Id=5, FirstName="Luan", LastName="Au", ColleagueId="E005", ManagerId="E0009", EmailAddress="Luan.Au@sainsburys.co.uk"},
                new StubColleague{Id=6, FirstName="Praveen", LastName="Kumar", ColleagueId="E006", ManagerId="E0009", EmailAddress="Praveen.K@sainsburys.co.uk"},
                new StubColleague{Id=7, FirstName="Steven", LastName="A", ColleagueId="E007", ManagerId="E0010", EmailAddress="Steven.A@sainsburys.co.uk"},
                new StubColleague{Id=8, FirstName="Anis", LastName="Batilwala", ColleagueId="E008", ManagerId="E009", EmailAddress="Anis.B@sainsburys.co.uk"},
                new StubColleague{Id=9, FirstName="Steven", LastName="Farkas", ColleagueId="E0009", ManagerId="E010", EmailAddress="steven.farkas@sainsburys.co.uk"},
                new StubColleague{Id=10, FirstName="Sandip", LastName="Vaidya", ColleagueId="E0010", ManagerId="E0011", EmailAddress="Sandip.V@sainsburys.co.uk"},
                new StubColleague{Id=11, FirstName="Mike", LastName="Gwyer",  ColleagueId="E0011", ManagerId=string.Empty, EmailAddress="Mike.G@sainsburys.co.uk"},
                new StubColleague{Id=12, FirstName="Ridley", LastName="Scott",  ColleagueId="E0012", ManagerId=string.Empty, EmailAddress="ridley.scott@sainsburys.co.uk"},
                new StubColleague{Id=13, FirstName="Joss", LastName="Whedon",  ColleagueId="E0013", ManagerId=string.Empty, EmailAddress="joss.whedon@sainsburys.co.uk"},
                new StubColleague{Id=14, FirstName="Rick", LastName="Deckard",  ColleagueId="E0014", ManagerId="E0012", EmailAddress="rick.deckard@sainsburys.co.uk"},
                new StubColleague{Id=15, FirstName="Ellen", LastName="Ripley",  ColleagueId="E0015", ManagerId="E0012", EmailAddress="ellen.ripley@sainsburys.co.uk"},
                new StubColleague{Id=16, FirstName="Malcolm", LastName="Reynolds",  ColleagueId="E0016", ManagerId="E0013", EmailAddress="malcolm.reynolds@sainsburys.co.uk"},
                new StubColleague{Id=17, FirstName="Zoe", LastName="Washburne",  ColleagueId="E0017", ManagerId="E0013", EmailAddress="zoe.washburne@sainsburys.co.uk"},
                new StubColleague{Id=18, FirstName="Chris", LastName="Mullaney",  ColleagueId="E0018", ManagerId="E0013", EmailAddress="Chris.Mullaney@sainsburys.co.uk"},

            };

            employees.ForEach(c=>context.Colleagues.Add(c));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
