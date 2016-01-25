using System;
using System.Data.Entity;
using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Repository
{
    public class RepositoryInitializer : DropCreateDatabaseIfModelChanges<RepositoryContext>
    {
        protected override void Seed(RepositoryContext context)
        {
            //var users=new List<LinkUser>{
            //    new LinkUser{Id=1,  ColleagueId="E001",     EmailAddress="Luan.Au@linktool.onmicrosoft.com"},
            //    new LinkUser{Id=2,  ColleagueId="E002",     EmailAddress="Parveen.Kumar@linktool.onmicrosoft.com"},
            //    new LinkUser{Id=3,  ColleagueId="E003",     EmailAddress="Steven.Farkas@linktool.onmicrosoft.com"},
            //    new LinkUser{Id=4,  ColleagueId="E004",     EmailAddress="Vasundhara.B@linktool.onmicrosoft.com"},

            //    new LinkUser{Id=10, ColleagueId="E0010",    EmailAddress="Sandip.Vaidya@linktool.onmicrosoft.com"},
            //    new LinkUser{Id=11, ColleagueId="E0011",    EmailAddress="Mike.Gwyer@linktool.onmicrosoft.com"},

            //    new LinkUser{Id=12, ColleagueId="E0012",    EmailAddress="ridley.scott@linktool.onmicrosoft.com"},
            //    new LinkUser{Id=13, ColleagueId="E0013",    EmailAddress="joss.whedon@linktool.onmicrosoft.com"},
            //    new LinkUser{Id=14, ColleagueId="E0014",    EmailAddress="rick.deckard@linktool.onmicrosoft.com"},
            //    new LinkUser{Id=15, ColleagueId="E0015",    EmailAddress="ellen.ripley@linktool.onmicrosoft.com"},
            //    new LinkUser{Id=16, ColleagueId="E0016",    EmailAddress="malcolm.reynolds@linktool.onmicrosoft.com"},
            //};

            //users.ForEach(c=>context.LinkUsers.Add(c));
            //context.SaveChanges();

         var periods = new List<Period>{
                new Period{Id=1, Description="Q1", Start=new DateTime(2014,04,01),End=new DateTime(2014,06,30), Year = "2014/15"},
                new Period{Id=2, Description="Q2", Start=new DateTime(2014,07,01),End=new DateTime(2014,09,30), Year = "2014/15"},
                new Period{Id=3, Description="Q3", Start=new DateTime(2014,10,01),End=new DateTime(2014,12,31), Year = "2014/15"},
                new Period{Id=4, Description="Q4", Start=new DateTime(2015,01,01),End=new DateTime(2015,03,31), Year = "2014/15"},
                new Period{Id=5, Description="Q1", Start=new DateTime(2015,04,01),End=new DateTime(2015,06,30), Year = "2015/16"},
                new Period{Id=6, Description="Q2", Start=new DateTime(2015,07,01),End=new DateTime(2015,09,30), Year = "2015/16"},
                new Period{Id=7, Description="Q3", Start=new DateTime(2015,10,01),End=new DateTime(2015,12,31), Year = "2015/16"},
                new Period{Id=8, Description="Q4", Start=new DateTime(2016,01,01),End=new DateTime(2016,03,31), Year = "2015/16"},
                new Period{Id=9, Description="Q1", Start=new DateTime(2016,04,01),End=new DateTime(2016,06,30), Year = "2016/17"},
                new Period{Id=10, Description="Q2", Start=new DateTime(2016,07,01),End=new DateTime(2016,09,30), Year = "2016/17"},
                new Period{Id=11, Description="Q3", Start=new DateTime(2016,10,01),End=new DateTime(2016,12,31), Year = "2016/17"},
                new Period{Id=12, Description="Q4", Start=new DateTime(2017,01,01),End=new DateTime(2017,03,31), Year = "2016/17"}
            };

            periods.ForEach(c => context.Periods.Add(c));
            context.SaveChanges();

            var questions = new List<Question> { 
            new Question{Id=1,Description="Reflecting on your objectives and the personal development you have undertaken, what is the difference you’ve made?", QuestionType = "LOOKING BACK"},
            new Question{Id=3,Description="What will you do going forward to make a bigger difference?", QuestionType = "LOOKING FORWARD"},
            new Question{Id=4,Description="What personal development will you undertake to enable the delivery of your objectives and support any career aspirations?", QuestionType = "DRIVING MY DEVELOPMENT"},
            new Question{Id=5,Description="What are the key takeaways from your conversation?", QuestionType = "IN A NUTSHELL"}
            };
            
            questions.ForEach(c => context.Questions.Add(c));            
            context.SaveChanges();

            //var employees=new List<Employee>{
            //    new Employee{Id=1, FirstName="Vasundhara", LastName="Chimakurthi", ColleagueId= "E001", ManagerId="E0010", EmailAddress="Vasundhara.B@sainsburys.co.uk"},
            //    new Employee{Id=2, FirstName="Laxmi", LastName="Nagaraja", ColleagueId="E002", ManagerId="E009", EmailAddress="Laxmi.N@sainsburys.co.uk"},
            //    new Employee{Id=3, FirstName="Tim", LastName="Morrison", ColleagueId="E003", ManagerId="E009", EmailAddress="Tim.M@sainsburys.co.uk"},
            //    new Employee{Id=4, FirstName="Chris", LastName="Atkin", ColleagueId="E004", ManagerId="E009", EmailAddress="Chris.A@sainsburys.co.uk"},
            //    new Employee{Id=5, FirstName="Luan", LastName="Au", ColleagueId="E005", ManagerId="E009", EmailAddress="Luan.Au@sainsburys.co.uk"},
            //    new Employee{Id=6, FirstName="Praveen", LastName="Kumar", ColleagueId="E006", ManagerId="E009", EmailAddress="Praveen.K@sainsburys.co.uk"},
            //    new Employee{Id=7, FirstName="Steven", LastName="A", ColleagueId="E007", ManagerId="E0010", EmailAddress="Steven.A@sainsburys.co.uk"},
            //    new Employee{Id=8, FirstName="Anis", LastName="Batilwala", ColleagueId="E008", ManagerId="E009", EmailAddress="Anis.B@sainsburys.co.uk"},
            //    new Employee{Id=9, FirstName="Steven", LastName="Farkas", ColleagueId="E009", ManagerId="E010", EmailAddress="steven.farkas@sainsburys.co.uk"},
            //    new Employee{Id=10, FirstName="Sandip", LastName="Vaidya", ColleagueId="E0010", ManagerId="E0011", EmailAddress="Sandip.V@sainsburys.co.uk"},
            //    new Employee{Id=11, FirstName="Mike", LastName="Gwyer",  ColleagueId="E0011", ManagerId=string.Empty, EmailAddress="Mike.G@sainsburys.co.uk"},
            //    new Employee{Id=12, FirstName="Ridley", LastName="Scott",  ColleagueId="E0012", ManagerId=string.Empty, EmailAddress="ridley.scott@sainsburys.co.uk"},
            //    new Employee{Id=13, FirstName="Joss", LastName="Whedon",  ColleagueId="E0013", ManagerId=string.Empty, EmailAddress="joss.whedon@sainsburys.co.uk"},
            //    new Employee{Id=14, FirstName="Rick", LastName="Deckard",  ColleagueId="E0014", ManagerId="E0012", EmailAddress="rick.deckard@sainsburys.co.uk"},
            //    new Employee{Id=15, FirstName="Ellen", LastName="Ripley",  ColleagueId="E0015", ManagerId="E0012", EmailAddress="ellen.ripley@sainsburys.co.uk"},
            //    new Employee{Id=16, FirstName="Malcolm", LastName="Reynolds",  ColleagueId="E0016", ManagerId="E0013", EmailAddress="malcolm.reynolds@sainsburys.co.uk"},
            //    new Employee{Id=17, FirstName="Zoe", LastName="Washburne",  ColleagueId="E0017", ManagerId="E0013", EmailAddress="zoe.washburne@sainsburys.co.uk"},
            //    new Employee{Id=18, FirstName="Chris", LastName="Mullaney",  ColleagueId="E0018", ManagerId="E0013", EmailAddress="Chris.Mullaney@sainsburys.co.uk"},
           
            //};
         
            //employees.ForEach(c=>context.Employees.Add(c));
            //context.SaveChanges();

            var meetings=new List<LinkMeeting>{

                new LinkMeeting{Id=1, ColleagueId= "E001", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E003" ,MeetingDate = new DateTime(2014,04,01)},
                new LinkMeeting{Id=2, ColleagueId="E001", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E003" , MeetingDate = new DateTime(2014,08,10)},
                new LinkMeeting{Id=3, ColleagueId="E001", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed , ManagerId ="E003" , MeetingDate = new DateTime(2014,12,12)},
                new LinkMeeting{Id=4, ColleagueId="E001", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E003" , MeetingDate = new DateTime(2015,01,13)},
                new LinkMeeting{Id=5, ColleagueId="E002", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E003" ,MeetingDate = new DateTime(2015,10,01)},
                new LinkMeeting{Id=6, ColleagueId="E003", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,10)},
                new LinkMeeting{Id=7, ColleagueId="E006", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed , ManagerId ="E009" , MeetingDate = new DateTime(2015,10,12)},
                new LinkMeeting{Id=8, ColleagueId="E001", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E003" , MeetingDate = new DateTime(2015,10,13)},
                new LinkMeeting{Id=9, ColleagueId="E003", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,14)},
                new LinkMeeting{Id=10, ColleagueId="E004", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,15)},
                new LinkMeeting{Id=11, ColleagueId="E001", ColleagueSignOff  = MeetingStatus.InComplete , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E003" , MeetingDate = new DateTime(2015,10,18)},
                new LinkMeeting{Id=12, ColleagueId="E003", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,20)},
                new LinkMeeting{Id=13, ColleagueId="E004", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,21)},
                new LinkMeeting{Id=14, ColleagueId="E0010", ColleagueSignOff  = MeetingStatus.InComplete , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0011" , MeetingDate = new DateTime(2015,10,23)},
                new LinkMeeting{Id=15, ColleagueId="E0010", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0011" , MeetingDate = new DateTime(2015,10,30)},
            };

            meetings.ForEach(c => context.Meeting.Add(c));
            context.SaveChanges();

            var answers = new List<Answer>{
                new Answer{Id=1, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1,LinkMeetingId = 1, Discussed=false},
                new Answer{Id=2, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=1,LinkMeetingId = 1,  Discussed=false},
                new Answer{Id=3, ManagerComments="Hard Worker", ColleagueComments="I'm a hard worker with the experience to get things done efficiently.", QuestionId=3,LinkMeetingId = 1,  Discussed=false},
                new Answer{Id=4, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4 ,LinkMeetingId = 1,  Discussed=false},
                new Answer{Id=5, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5,LinkMeetingId = 1,  Discussed=false},
                new Answer{Id=6, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1, LinkMeetingId = 10,  Discussed=false},
                new Answer{Id=7, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=1, LinkMeetingId = 10,  Discussed=false},
                new Answer{Id=8, ManagerComments="More Organizational skills", ColleagueComments="I can contribute my organizational skills and my ability to work well in a group.", QuestionId=3, LinkMeetingId = 10,  Discussed=false},
                new Answer{Id=9, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4, LinkMeetingId = 10,  Discussed=false},
                new Answer{Id=10, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5, LinkMeetingId = 10,  Discussed=false},
                new Answer{Id=11, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1,LinkMeetingId = 2,  Discussed=false},
                new Answer{Id=12, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=1,LinkMeetingId = 2,  Discussed=false},
                new Answer{Id=13, ManagerComments="Hard Worker", ColleagueComments="I'm a hard worker with the experience to get things done efficiently.", QuestionId=3,LinkMeetingId = 2,  Discussed=false},
                new Answer{Id=14, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4 ,LinkMeetingId = 2,  Discussed=false},
                new Answer{Id=15, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5,LinkMeetingId = 2,  Discussed=false},

                new Answer{Id=16, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1,LinkMeetingId = 3,  Discussed=false},
                new Answer{Id=17, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=1,LinkMeetingId = 3,  Discussed=false},
                new Answer{Id=18, ManagerComments="Hard Worker", ColleagueComments="I'm a hard worker with the experience to get things done efficiently.", QuestionId=3,LinkMeetingId = 3,  Discussed=false},
                new Answer{Id=19, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4 ,LinkMeetingId = 3,  Discussed=false},
                new Answer{Id=20, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5,LinkMeetingId = 3,  Discussed=false},

                new Answer{Id=21, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1,LinkMeetingId = 4,  Discussed=false},
                new Answer{Id=22, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=1,LinkMeetingId = 4,  Discussed=false},
                new Answer{Id=23, ManagerComments="Hard Worker", ColleagueComments="I'm a hard worker with the experience to get things done efficiently.", QuestionId=3,LinkMeetingId = 4,  Discussed=false},
                new Answer{Id=24, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4 ,LinkMeetingId = 4,  Discussed=false},
                new Answer{Id=25, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5,LinkMeetingId = 4,  Discussed=false},

                new Answer{Id=26, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1,LinkMeetingId = 5,  Discussed=false},
                new Answer{Id=27, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=1,LinkMeetingId = 5,  Discussed=false},
                new Answer{Id=28, ManagerComments="Hard Worker", ColleagueComments="I'm a hard worker with the experience to get things done efficiently.", QuestionId=3,LinkMeetingId = 5,  Discussed=false},
                new Answer{Id=29, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4 ,LinkMeetingId = 5,  Discussed=false},
                new Answer{Id=30, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5,LinkMeetingId = 5,  Discussed=false},

                new Answer{Id=31, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1,LinkMeetingId = 8,  Discussed=false},
                new Answer{Id=32, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=1,LinkMeetingId = 8,  Discussed=false},
                new Answer{Id=33, ManagerComments="Hard Worker", ColleagueComments="I'm a hard worker with the experience to get things done efficiently.", QuestionId=3,LinkMeetingId = 8,  Discussed=false},
                new Answer{Id=34, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4 ,LinkMeetingId = 8,  Discussed=false},
                new Answer{Id=35, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5,LinkMeetingId = 8,  Discussed=false},

                new Answer{Id=36, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1,LinkMeetingId = 11,  Discussed=false},
                new Answer{Id=37, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=1,LinkMeetingId = 11,  Discussed=false},
                new Answer{Id=38, ManagerComments="Hard Worker", ColleagueComments="I'm a hard worker with the experience to get things done efficiently.", QuestionId=3,LinkMeetingId = 11,  Discussed=false},
                new Answer{Id=39, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4 ,LinkMeetingId = 11,  Discussed=false},
                new Answer{Id=40, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5,LinkMeetingId = 11,  Discussed=false},
             };

            answers.ForEach(c => context.Answers.Add(c));
            context.SaveChanges();

            var objectives = new List<LinkObjective> { 
            new LinkObjective{Id=1,ColleagueId="E002", ManagerId="E003", ColleagueSignOff=ObjectiveStatus.Approved,ManagerSignOff=ObjectiveStatus.Approved,SignOffDate=new DateTime(2015,03,31),CreatedDate=new DateTime(2015,03,31),LastAmendedBy="E002",LastAmendedDate=new DateTime(2015,03,31),Objective="this is my objective 111", Title="Be superman"},
            new LinkObjective{Id=1,ColleagueId="E002", ManagerId="E003", ColleagueSignOff=ObjectiveStatus.Approved,ManagerSignOff=ObjectiveStatus.Approved,SignOffDate=new DateTime(2015,03,31),CreatedDate=new DateTime(2015,03,31),LastAmendedBy="E002",LastAmendedDate=new DateTime(2015,03,31),Objective="this is my objective 222", Title="All comes from one, one comes from where?"},
            new LinkObjective{Id=1,ColleagueId="E001", ManagerId="E003", ColleagueSignOff=ObjectiveStatus.Approved,ManagerSignOff=ObjectiveStatus.Approved,SignOffDate=new DateTime(2015,03,31),CreatedDate=new DateTime(2015,03,31),LastAmendedBy="E001",LastAmendedDate=new DateTime(2015,03,31),Objective="this is my objective 333", Title="Good and bad is a disease of the mine - Trang Tu"},
            new LinkObjective{Id=1,ColleagueId="E001", ManagerId="E003", ColleagueSignOff=ObjectiveStatus.Draft,ManagerSignOff=ObjectiveStatus.Draft,SignOffDate=null,CreatedDate=new DateTime(2015,03,31),LastAmendedBy="E001",LastAmendedDate=new DateTime(2015,03,31),Objective="this is my objective 444", Title="Show me your mind"},
            new LinkObjective{Id=1,ColleagueId="E003", ManagerId="E0010", ColleagueSignOff=ObjectiveStatus.Draft,ManagerSignOff=ObjectiveStatus.Draft,SignOffDate=null,CreatedDate=new DateTime(2015,03,31),LastAmendedBy="E003",LastAmendedDate=new DateTime(2015,03,31),Objective="this is my objective 555",Title="Live happily"},
            };

            objectives.ForEach(c => context.Objectives.Add(c));
            context.SaveChanges();
            
            base.Seed(context);
        }
    }
}
