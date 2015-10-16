﻿using System.Collections.Generic;
using JsPlc.Ssc.Link.Models;

namespace JsPlc.Ssc.Link.Repository
{
    public interface ILinkRepository
    {
        
        IEnumerable<Question> GetQuestions();
        
        MeetingView GetMeeting(int meetingId);
        MeetingView CreateMeeting(string colleagueId);
        int SaveMeeting(MeetingView meeting);
        void UpdateMeeting(int id,MeetingView meeting);


        TeamView GetMeetings(string colleagueId);
        bool IsManager(string userName);
        IEnumerable<TeamView> GetTeam(string managerId);
        
        void Dispose();
    }
}
