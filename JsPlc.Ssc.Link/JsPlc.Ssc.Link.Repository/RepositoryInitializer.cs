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
                new Period{Id=1, Description="Q1", Start=new DateTime(2015,01,01),End=new DateTime(2015,03,31)},
                new Period{Id=2, Description="Q2", Start=new DateTime(2015,04,01),End=new DateTime(2015,06,30)},
                new Period{Id=3, Description="Q3", Start=new DateTime(2015,07,01),End=new DateTime(2015,09,30)},
                new Period{Id=4, Description="Q4", Start=new DateTime(2015,10,01),End=new DateTime(2015,12,31)}
            };

            periods.ForEach(c => context.Periods.Add(c));
            context.SaveChanges();

            var questions = new List<Question> { 
            new Question{Id=1,Description="What contributions have I made and what was the resulting difference?" , PeriodId = 1},
            new Question{Id=2,Description="Where could I have made a greater difference?" , PeriodId = 1},
            new Question{Id=3,Description="What am I going to do make more of a difference?", PeriodId = 1},
            new Question{Id=4,Description="How will I learn and grow to make even more of a difference?" , PeriodId = 1},
            new Question{Id=5,Description="My career goals: How can I learn and grow to be the best I can be?" , PeriodId = 1}
            };
            
            questions.ForEach(c => context.Questions.Add(c));            
            context.SaveChanges();

           
            var employees=new List<Employee>{
                new Employee{Id=1, FirstName="Vasundhara", LastName="Chimakurthi", EmployeeId= "E001", ManagerId=10, EmailAddress="Vasundhara.B@sainsburys.co.uk"},
                new Employee{Id=2, FirstName="Laxmi", LastName="Nagaraja", EmployeeId="E002", ManagerId=9, EmailAddress="Laxmi.N@sainsburys.co.uk"},
                new Employee{Id=3, FirstName="Tim", LastName="Morrison", EmployeeId="E003", ManagerId=9, EmailAddress="Tim.M@sainsburys.co.uk"},
                new Employee{Id=4, FirstName="Chris", LastName="Atkin", EmployeeId="E004", ManagerId=9, EmailAddress="Chris.A@sainsburys.co.uk"},
                new Employee{Id=5, FirstName="Luan", LastName="Au", EmployeeId="E005", ManagerId=10, EmailAddress="Luan.A@sainsburys.co.uk"},
                new Employee{Id=6, FirstName="Praveen", LastName="Kumar", EmployeeId="E006", ManagerId=10, EmailAddress="Praveen.K@sainsburys.co.uk"},
                new Employee{Id=7, FirstName="Steven", LastName="A", EmployeeId="E007", ManagerId=10, EmailAddress="Steven.A@sainsburys.co.uk"},
                new Employee{Id=8, FirstName="Anis", LastName="Batilwala", EmployeeId="E008", ManagerId=9, EmailAddress="Anis.B@sainsburys.co.uk"},
                new Employee{Id=9, FirstName="Mohammed", LastName="Tahir", EmployeeId="E009", ManagerId=0, EmailAddress="Mohammed.T@sainsburys.co.uk"},
                new Employee{Id=10, FirstName="Sandip", LastName="Vaidya", EmployeeId="E0010", ManagerId=0, EmailAddress="Sandip.V@sainsburys.co.uk"}
            };

            employees.ForEach(c=>context.Employees.Add(c));
            context.SaveChanges();

            var users = new List<User>{
                new User{Id=1, UserName= "Sandip.V@sainsburys.co.uk", EmployeeId=10, Password="testing" },
                new User{Id=2, UserName="Mohammed.T@sainsburys.co.uk", EmployeeId=9, Password="testing" },
                new User{Id=3, UserName="Vasundhara.B@sainsburys.co.uk", EmployeeId=1, Password="testing" },
                new User{Id=4, UserName="Luan.A@sainsburys.co.uk", EmployeeId=5, Password="testing" },
                new User{Id=5, UserName="Praveen.K@sainsburys.co.uk", EmployeeId=6, Password="testing" },
                new User{Id=6, UserName="Steven.A@sainsburys.co.uk", EmployeeId=7, Password="testing" },
            };

            users.ForEach(c=>context.Users.Add(c));
            context.SaveChanges();

            var performance=new List<LinkMeeting>{

                new LinkMeeting{Id=1, EmployeeId=1, PeriodId=1, Status=MeetingStatus.Completed , MeetingDate = new DateTime(2015,10,01)},
                new LinkMeeting{Id=2, EmployeeId=5, PeriodId=1, Status=MeetingStatus.Completed, MeetingDate = new DateTime(2015,10,10)},
                new LinkMeeting{Id=3, EmployeeId=6, PeriodId=1, Status=MeetingStatus.Completed , MeetingDate = new DateTime(2015,10,12)},
                new LinkMeeting{Id=4, EmployeeId=1, PeriodId=2, Status=MeetingStatus.Completed , MeetingDate = new DateTime(2015,10,13)},
                new LinkMeeting{Id=5, EmployeeId=5, PeriodId=2, Status=MeetingStatus.Completed, MeetingDate = new DateTime(2015,10,14)},
                new LinkMeeting{Id=6, EmployeeId=6, PeriodId=2, Status=MeetingStatus.Completed, MeetingDate = new DateTime(2015,10,15)},
                new LinkMeeting{Id=7, EmployeeId=1, PeriodId=3, Status=MeetingStatus.InComplete, MeetingDate = new DateTime(2015,10,18)},
                new LinkMeeting{Id=8, EmployeeId=5, PeriodId=3, Status=MeetingStatus.InComplete, MeetingDate = new DateTime(2015,10,20)},
                new LinkMeeting{Id=9, EmployeeId=6, PeriodId=3, Status=MeetingStatus.InComplete, MeetingDate = new DateTime(2015,10,21)},
                new LinkMeeting{Id=10, EmployeeId=10, PeriodId=1, Status=MeetingStatus.Completed, MeetingDate = new DateTime(2015,10,23)},
                new LinkMeeting{Id=11, EmployeeId=10, PeriodId=2, Status=MeetingStatus.InComplete, MeetingDate = new DateTime(2015,10,30)},
            };

            performance.ForEach(c=>context.Meeting.Add(c));
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
