using System;
using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;
using JsPlc.Ssc.Link.Repository;
using JsPlc.Ssc.Link.Models.Entities;

namespace JsPlc.Ssc.Link.Service.Tests
{
    public class ContextSetup
    {
        public static RepositoryContext MockContext(RepositoryContext context)
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
            new Question{Id=1,Description="What contributions have I made and what was the resulting difference?", QuestionType = "LOOKING BACK"},
            new Question{Id=2,Description="Where could I have made a greater difference?", QuestionType = "LOOKING BACK"},
            new Question{Id=3,Description="What am I going to do make more of a difference?", QuestionType = "LOOKING FORWARD"},
            new Question{Id=4,Description="How will I learn and grow to make even more of a difference?", QuestionType = "DRIVING MY DEVELOPMENT"},
            new Question{Id=5,Description="My career goals: How can I learn and grow to be the best I can be?", QuestionType = "IN A NUTSHELL"}
            };

            questions.ForEach(c => context.Questions.Add(c));
            context.SaveChanges();

            //var users = new List<LinkUserView>{
            //    new LinkUserView{UserId=1},
            //    new LinkUserView{UserId=2},
            //    new LinkUserView{UserId=3},
            //    new LinkUserView{UserId=4},

            //    new LinkUserView{UserId=10},
            //    new LinkUserView{UserId=11},

            //    new LinkUserView{UserId=12},
            //    new LinkUserView{UserId=13},
            //    new LinkUserView{UserId=14},
            //    new LinkUserView{UserId=15},
            //    new LinkUserView{UserId=16},
            //};

            //users.ForEach(c => context.LinkUsers.Add(c));
            //context.SaveChanges();

            var meetings=new List<LinkMeeting>{

                new LinkMeeting{Id=1, ColleagueId= "E001", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E003" ,
                    MeetingDate = new DateTime(2014,04,01), 
                    ColleagueSignedOffDate = DateTime.Parse("2014-04-02 12:30pm"),
                    ManagerSignedOffDate = DateTime.Parse("2014-04-03 2:30pm")},
                new LinkMeeting{Id=2, ColleagueId="E001", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E003" , 
                    MeetingDate = new DateTime(2014,08,10),
                    ColleagueSignedOffDate = DateTime.Parse("2014-08-12 2:00pm"),
                    ManagerSignedOffDate = DateTime.Parse("2014-08-14 3:45pm")},
                new LinkMeeting{Id=3, ColleagueId="E001", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed , ManagerId ="E003" , 
                    MeetingDate = new DateTime(2014,12,12),
                    ColleagueSignedOffDate = DateTime.Parse("2014-12-12 10:00am"),
                    ManagerSignedOffDate = DateTime.Parse("2014-12-15 8:05pm")},
                new LinkMeeting{Id=4, ColleagueId="E001", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E003" , 
                    MeetingDate = new DateTime(2015,01,13),
                    ColleagueSignedOffDate = DateTime.Parse("2015-01-14 11:00am"),
                    ManagerSignedOffDate = DateTime.Parse("2015-01-17 8:05am")},
                new LinkMeeting{Id=5, ColleagueId="E002", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E003" ,
                    MeetingDate = new DateTime(2015,10,01),
                    ColleagueSignedOffDate = DateTime.Parse("2015-10-03 04:00pm"),
                    ManagerSignedOffDate = DateTime.Parse("2015-10-12 9:05am")},
                new LinkMeeting{Id=6, ColleagueId="E003", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0010" , 
                    MeetingDate = new DateTime(2015,10,10),
                    ColleagueSignedOffDate = DateTime.Parse("2015-10-15 04:00pm"),
                    ManagerSignedOffDate = null},                
                new LinkMeeting{Id=7, ColleagueId="E006", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.InComplete , ManagerId ="E009" , 
                    MeetingDate = new DateTime(2015,10,12),
                    ColleagueSignedOffDate = DateTime.Parse("2015-10-15 09:00am"),
                    ManagerSignedOffDate = null},     
                new LinkMeeting{Id=8, ColleagueId="E001", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E003" , 
                    MeetingDate = new DateTime(2015,10,13),
                    ColleagueSignedOffDate = DateTime.Parse("2015-10-23 04:20pm"),
                    ManagerSignedOffDate = DateTime.Parse("2015-10-24 9:15am")},
                new LinkMeeting{Id=9, ColleagueId="E003", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , 
                    MeetingDate = new DateTime(2015,10,14),
                    ColleagueSignedOffDate = DateTime.Parse("2015-10-16 08:45am"),
                    ManagerSignedOffDate = DateTime.Parse("2015-10-20 9:25am")},
                new LinkMeeting{Id=10, ColleagueId="E004", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , 
                    MeetingDate = new DateTime(2015,10,15),
                    ColleagueSignedOffDate = DateTime.Parse("2015-10-16 09:45am"),
                    ManagerSignedOffDate = DateTime.Parse("2015-10-19 09:25am")},
                new LinkMeeting{Id=11, ColleagueId="E001", ColleagueSignOff  = MeetingStatus.InComplete , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E003" , MeetingDate = new DateTime(2015,10,18)},
                new LinkMeeting{Id=12, ColleagueId="E003", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0010" , 
                    MeetingDate = new DateTime(2015,10,20),
                    ColleagueSignedOffDate = DateTime.Parse("2015-10-20 09:30am"),
                    ManagerSignedOffDate = null},
                new LinkMeeting{Id=13, ColleagueId="E004", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0010" , 
                    MeetingDate = new DateTime(2015,10,21),
                    ColleagueSignedOffDate = DateTime.Parse("2015-10-22 03:30pm"),
                    ManagerSignedOffDate = null},
                new LinkMeeting{Id=14, ColleagueId="E0010", ColleagueSignOff  = MeetingStatus.InComplete , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0011" , MeetingDate = new DateTime(2015,10,23)},
                new LinkMeeting{Id=15, ColleagueId="E0010", ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0011" , 
                    MeetingDate = new DateTime(2015,10,30),
                    ColleagueSignedOffDate = DateTime.Parse("2015-12-29 03:00pm"),
                    ManagerSignedOffDate = null},
            };

            //var meetings = new List<LinkMeeting>
            //{
                //new LinkMeeting{Id=1, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" ,MeetingDate = new DateTime(2014,04,01)},
                //new LinkMeeting{Id=2, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2014,08,10)},
                //new LinkMeeting{Id=3, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed , ManagerId ="E0010" , MeetingDate = new DateTime(2014,12,12)},
                //new LinkMeeting{Id=4, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2015,01,13)},
                //new LinkMeeting{Id=5, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" ,MeetingDate = new DateTime(2015,10,01)},
                //new LinkMeeting{Id=6, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,10)},
                //new LinkMeeting{Id=7, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed , ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,12)},
                //new LinkMeeting{Id=8, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,13)},
                //new LinkMeeting{Id=9, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,14)},
                //new LinkMeeting{Id=10, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.Completed, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,15)},
                //new LinkMeeting{Id=11, ColleagueSignOff  = MeetingStatus.InComplete , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,18)},
                //new LinkMeeting{Id=12, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,20)},
                //new LinkMeeting{Id=13, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0010" , MeetingDate = new DateTime(2015,10,21)},
                //new LinkMeeting{Id=14, ColleagueSignOff  = MeetingStatus.InComplete , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0011" , MeetingDate = new DateTime(2015,10,23)},
                //new LinkMeeting{Id=15, ColleagueSignOff  = MeetingStatus.Completed , ManagerSignOff = MeetingStatus.InComplete, ManagerId ="E0011" , MeetingDate = new DateTime(2015,10,30)},
            //};

            meetings.ForEach(c => context.Meeting.Add(c));
            context.SaveChanges();

            var answers = new List<Answer>{
                new Answer{Id=1, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1,LinkMeetingId = 1, Discussed=false},
                new Answer{Id=2, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=2,LinkMeetingId = 1,  Discussed=false},
                new Answer{Id=3, ManagerComments="Hard Worker", ColleagueComments="I'm a hard worker with the experience to get things done efficiently.", QuestionId=3,LinkMeetingId = 1,  Discussed=false},
                new Answer{Id=4, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4 ,LinkMeetingId = 1,  Discussed=false},
                new Answer{Id=5, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5,LinkMeetingId = 1,  Discussed=false},

                new Answer{Id=6, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1, LinkMeetingId = 10,  Discussed=false},
                new Answer{Id=7, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=2, LinkMeetingId = 10,  Discussed=false},
                new Answer{Id=8, ManagerComments="More Organizational skills", ColleagueComments="I can contribute my organizational skills and my ability to work well in a group.", QuestionId=3, LinkMeetingId = 10,  Discussed=false},
                new Answer{Id=9, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4, LinkMeetingId = 10,  Discussed=false},
                new Answer{Id=10, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5, LinkMeetingId = 10,  Discussed=false},
                
                new Answer{Id=11, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1,LinkMeetingId = 2,  Discussed=false},
                new Answer{Id=12, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=2,LinkMeetingId = 2,  Discussed=false},
                new Answer{Id=13, ManagerComments="Hard Worker", ColleagueComments="I'm a hard worker with the experience to get things done efficiently.", QuestionId=3,LinkMeetingId = 2,  Discussed=false},
                new Answer{Id=14, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4 ,LinkMeetingId = 2,  Discussed=false},
                new Answer{Id=15, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5,LinkMeetingId = 2,  Discussed=false},

                new Answer{Id=16, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1,LinkMeetingId = 3,  Discussed=false},
                new Answer{Id=17, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=2,LinkMeetingId = 3,  Discussed=false},
                new Answer{Id=18, ManagerComments="Hard Worker", ColleagueComments="I'm a hard worker with the experience to get things done efficiently.", QuestionId=3,LinkMeetingId = 3,  Discussed=false},
                new Answer{Id=19, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4 ,LinkMeetingId = 3,  Discussed=false},
                new Answer{Id=20, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5,LinkMeetingId = 3,  Discussed=false},

                new Answer{Id=21, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1,LinkMeetingId = 4,  Discussed=false},
                new Answer{Id=22, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=2,LinkMeetingId = 4,  Discussed=false},
                new Answer{Id=23, ManagerComments="Hard Worker", ColleagueComments="I'm a hard worker with the experience to get things done efficiently.", QuestionId=3,LinkMeetingId = 4,  Discussed=false},
                new Answer{Id=24, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4 ,LinkMeetingId = 4,  Discussed=false},
                new Answer{Id=25, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5,LinkMeetingId = 4,  Discussed=false},

                new Answer{Id=26, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1,LinkMeetingId = 5,  Discussed=false},
                new Answer{Id=27, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=2,LinkMeetingId = 5,  Discussed=false},
                new Answer{Id=28, ManagerComments="Hard Worker", ColleagueComments="I'm a hard worker with the experience to get things done efficiently.", QuestionId=3,LinkMeetingId = 5,  Discussed=false},
                new Answer{Id=29, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4 ,LinkMeetingId = 5,  Discussed=false},
                new Answer{Id=30, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5,LinkMeetingId = 5,  Discussed=false},

                new Answer{Id=31, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1,LinkMeetingId = 8,  Discussed=false},
                new Answer{Id=32, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=2,LinkMeetingId = 8,  Discussed=false},
                new Answer{Id=33, ManagerComments="Hard Worker", ColleagueComments="I'm a hard worker with the experience to get things done efficiently.", QuestionId=3,LinkMeetingId = 8,  Discussed=false},
                new Answer{Id=34, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4 ,LinkMeetingId = 8,  Discussed=false},
                new Answer{Id=35, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5,LinkMeetingId = 8,  Discussed=false},

                new Answer{Id=36, ManagerComments="Commitment ", ColleagueComments="More commitment towards work", QuestionId=1,LinkMeetingId = 11,  Discussed=false},
                new Answer{Id=37, ManagerComments="Approach", ColleagueComments="BY changing the approach", QuestionId=2,LinkMeetingId = 11,  Discussed=false},
                new Answer{Id=38, ManagerComments="Hard Worker", ColleagueComments="I'm a hard worker with the experience to get things done efficiently.", QuestionId=3,LinkMeetingId = 11,  Discussed=false},
                new Answer{Id=39, ManagerComments="Studying", ColleagueComments="By researching and reading books", QuestionId=4 ,LinkMeetingId = 11,  Discussed=false},
                new Answer{Id=40, ManagerComments="Training", ColleagueComments="By getting trainning in specific areas", QuestionId=5,LinkMeetingId = 11,  Discussed=false},
             };

            answers.ForEach(c => context.Answers.Add(c));
            context.SaveChanges();

            return context;
        }
    }
}
