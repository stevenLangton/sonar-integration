using System;
using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Repository;

namespace JsPlc.Ssc.Link.Service.Tests
{
    public class ContextSetup
    {
        public static RepositoryContext  MockContext(RepositoryContext context)
        {
            
            var periods = new List<Period>{
                new Period{Id=1, Description="Q1", Start=new DateTime(2015,04,01),End=new DateTime(2015,06,30), Year = "2015/16"},
                new Period{Id=2, Description="Q2", Start=new DateTime(2015,07,01),End=new DateTime(2015,09,30), Year = "2015/16"},
                new Period{Id=3, Description="Q3", Start=new DateTime(2015,10,01),End=new DateTime(2015,12,31), Year = "2015/16"},
                new Period{Id=4, Description="Q4", Start=new DateTime(2016,01,01),End=new DateTime(2016,03,31), Year = "2015/16"},
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


            var employees = new List<Employee>{
                new Employee{Id=1, FirstName="Vasundhara", LastName="Chimakurthi", ColleagueId= "E001", ManagerId="E0010", EmailAddress="Vasundhara.B@sainsburys.co.uk"},
                new Employee{Id=2, FirstName="Luan", LastName="Au", ColleagueId="E005", ManagerId="E0010", EmailAddress="Luan.A@sainsburys.co.uk"},
                new Employee{Id=3, FirstName="Praveen", LastName="Kumar", ColleagueId="E006", ManagerId="E0010", EmailAddress="Praveen.K@sainsburys.co.uk"},
                new Employee{Id=4, FirstName="Steven", LastName="A", ColleagueId="E007", ManagerId="E0010", EmailAddress="Steven.A@sainsburys.co.uk"},
                new Employee{Id=5, FirstName="Sandip", LastName="Vaidya", ColleagueId="E0010", ManagerId="E0011", EmailAddress="Sandip.V@sainsburys.co.uk"},
                
            };

            employees.ForEach(c => context.Employees.Add(c));
            context.SaveChanges();


            var meetings = new List<LinkMeeting>{

                new LinkMeeting{Id=1, EmployeeId=1, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" ,MeetingDate = new DateTime(2015,04,01)},
                new LinkMeeting{Id=2, EmployeeId=1, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2015,08,10)},
                new LinkMeeting{Id=3, EmployeeId=1, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed , ManagerId ="E0010" , MeetingDate = new DateTime(2016,12,12)},
                new LinkMeeting{Id=4, EmployeeId=1, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2016,01,13)},
            };

            meetings.ForEach(c => context.Meeting.Add(c));
            context.SaveChanges();

            var answers = new List<Answer>{
                new Answer{Id=1, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1,LinkMeetingId = 1},
                new Answer{Id=2, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=2,LinkMeetingId = 1},
                new Answer{Id=3, ManagerComments="Hard Worker", ColleagueComments="I'm a hard worker with the experience to get things done efficiently.", QuestionId=3,LinkMeetingId = 1},
                new Answer{Id=4, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4 ,LinkMeetingId = 1},
                new Answer{Id=5, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5,LinkMeetingId = 1},
             };

            answers.ForEach(c => context.Answers.Add(c));
            context.SaveChanges();

            return context;
        }
    }
}
