using System;
using System.Data.Entity;
using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Repository
{
    public class RepositoryInitializer : DropCreateDatabaseIfModelChanges<RepositoryContext>
    {
        protected override void Seed(RepositoryContext context)
        {
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
            new Question{Id=1,Description="What contributions have I made and what was the resulting difference?", QuestionType = "b"},
            new Question{Id=2,Description="Where could I have made a greater difference?", QuestionType = "b"},
            new Question{Id=3,Description="What am I going to do make more of a difference?", QuestionType = "f"},
            new Question{Id=4,Description="How will I learn and grow to make even more of a difference?", QuestionType = "f"},
            new Question{Id=5,Description="My career goals: How can I learn and grow to be the best I can be?", QuestionType = "f"}
            };
            
            questions.ForEach(c => context.Questions.Add(c));            
            context.SaveChanges();

           
            var employees=new List<Employee>{
                new Employee{Id=1, FirstName="Vasundhara", LastName="Chimakurthi", ColleagueId= "E001", ManagerId="E0010", EmailAddress="Vasundhara.B@sainsburys.co.uk"},
                new Employee{Id=2, FirstName="Laxmi", LastName="Nagaraja", ColleagueId="E002", ManagerId="E009", EmailAddress="Laxmi.N@sainsburys.co.uk"},
                new Employee{Id=3, FirstName="Tim", LastName="Morrison", ColleagueId="E003", ManagerId="E009", EmailAddress="Tim.M@sainsburys.co.uk"},
                new Employee{Id=4, FirstName="Chris", LastName="Atkin", ColleagueId="E004", ManagerId="E009", EmailAddress="Chris.A@sainsburys.co.uk"},
                new Employee{Id=5, FirstName="Luan", LastName="Au", ColleagueId="E005", ManagerId="E0010", EmailAddress="Luan.A@sainsburys.co.uk"},
                new Employee{Id=6, FirstName="Praveen", LastName="Kumar", ColleagueId="E006", ManagerId="E0010", EmailAddress="Praveen.K@sainsburys.co.uk"},
                new Employee{Id=7, FirstName="Steven", LastName="A", ColleagueId="E007", ManagerId="E0010", EmailAddress="Steven.A@sainsburys.co.uk"},
                new Employee{Id=8, FirstName="Anis", LastName="Batilwala", ColleagueId="E008", ManagerId="E009", EmailAddress="Anis.B@sainsburys.co.uk"},
                new Employee{Id=9, FirstName="Mohammed", LastName="Tahir", ColleagueId="E009", ManagerId="E0011", EmailAddress="Mohammed.T@sainsburys.co.uk"},
                new Employee{Id=10, FirstName="Sandip", LastName="Vaidya", ColleagueId="E0010", ManagerId="E0011", EmailAddress="Sandip.V@sainsburys.co.uk"},
                new Employee{Id=11, FirstName="Mike", LastName="Gwyer",  ColleagueId="E0011", ManagerId=string.Empty, EmailAddress="Mike.G@sainsburys.co.uk"},
                new Employee{Id=12, FirstName="Ridley", LastName="Scott",  ColleagueId="E0012", ManagerId=string.Empty, EmailAddress="ridley.scott@sainsburys.co.uk"},
                new Employee{Id=13, FirstName="Joss", LastName="Whedon",  ColleagueId="E0013", ManagerId=string.Empty, EmailAddress="joss.whedon@sainsburys.co.uk"},
                new Employee{Id=14, FirstName="Rick", LastName="Deckard",  ColleagueId="E0014", ManagerId="E0012", EmailAddress="rick.deckard@sainsburys.co.uk"},
                new Employee{Id=15, FirstName="Ellen", LastName="Ripley",  ColleagueId="E0015", ManagerId="E0012", EmailAddress="ellen.ripley@sainsburys.co.uk"},
                new Employee{Id=16, FirstName="Malcolm", LastName="Reynolds",  ColleagueId="E0016", ManagerId="E0013", EmailAddress="malcolm.reynolds@sainsburys.co.uk"},
                new Employee{Id=17, FirstName="Zoe", LastName="Washburne",  ColleagueId="E0017", ManagerId="E0013", EmailAddress="zoe.washburne@sainsburys.co.uk"},

            };

            employees.ForEach(c=>context.Employees.Add(c));
            context.SaveChanges();
         

            var meetings=new List<LinkMeeting>{

                new LinkMeeting{Id=1, EmployeeId=1, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" ,MeetingDate = new DateTime(2014,04,01)},
                new LinkMeeting{Id=2, EmployeeId=1, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2014,08,10)},
                new LinkMeeting{Id=3, EmployeeId=1, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed , ManagerId ="E0010" , MeetingDate = new DateTime(2015,12,12)},
                new LinkMeeting{Id=4, EmployeeId=1, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2015,01,13)},
                new LinkMeeting{Id=5, EmployeeId=1, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" ,MeetingDate = new DateTime(2015,10,01)},
                new LinkMeeting{Id=6, EmployeeId=5, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,10)},
                new LinkMeeting{Id=7, EmployeeId=6, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed , ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,12)},
                new LinkMeeting{Id=8, EmployeeId=1, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,13)},
                new LinkMeeting{Id=9, EmployeeId=5, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,14)},
                new LinkMeeting{Id=10, EmployeeId=6, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,15)},
                new LinkMeeting{Id=11, EmployeeId=1, ColleagueSignOff  = MeetingStatus.InComplete , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,18)},
                new LinkMeeting{Id=12, EmployeeId=5, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,20)},
                new LinkMeeting{Id=13, EmployeeId=6, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,21)},
                new LinkMeeting{Id=14, EmployeeId=10, ColleagueSignOff  = MeetingStatus.InComplete , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0011" , MeetingDate = new DateTime(2015,10,23)},
                new LinkMeeting{Id=15, EmployeeId=10, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0011" , MeetingDate = new DateTime(2015,10,30)},
            };

            meetings.ForEach(c => context.Meeting.Add(c));
            context.SaveChanges();

            var answers = new List<Answer>{
                new Answer{Id=1, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1,LinkMeetingId = 1},
                new Answer{Id=2, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=2,LinkMeetingId = 1},
                new Answer{Id=3, ManagerComments="Hard Worker", ColleagueComments="I'm a hard worker with the experience to get things done efficiently.", QuestionId=3,LinkMeetingId = 1},
                new Answer{Id=4, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4 ,LinkMeetingId = 1},
                new Answer{Id=5, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5,LinkMeetingId = 1},
                new Answer{Id=6, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1, LinkMeetingId = 10},
                new Answer{Id=7, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=2, LinkMeetingId = 10},
                new Answer{Id=8, ManagerComments="More Organizational skills", ColleagueComments="I can contribute my organizational skills and my ability to work well in a group.", QuestionId=3, LinkMeetingId = 10},
                new Answer{Id=9, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4, LinkMeetingId = 10},
                new Answer{Id=10, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5, LinkMeetingId = 10}
             };

            answers.ForEach(c => context.Answers.Add(c));
            context.SaveChanges();
            
            base.Seed(context);
        }
    }
}
