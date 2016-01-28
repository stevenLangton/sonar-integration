using System.Collections.Generic;

namespace JsPlc.Ssc.Link.Models
{
    public class ColleagueTeamView
    {
        public ColleagueView Colleague { get; set; }
        public List<LinkMeetingView> Meetings { get; set; } // Whole list - Maybe remove later..

        // Below info not populated by service. Intended to split Meetings by UI/Portal methods, 
        //  Use outputColleagueTeamView = TeamController.AssignMeetingsByDate(inputColleagueTeamView)
        //  Populates in "nearest to furthest" order
        public List<LinkMeetingView> UpcomingMeetings { get; set; } 
        public List<LinkMeetingView> PastMeetings { get; set; } 

        public LinkMeetingView LatestMeeting { get; set; } // For the UI: Current Link Meeting: or Upcoming Meeting: 
        public LinkMeetingView LastInCompleteMeeting { get; set; } // For the UI text: Last Incomplete Meeting: November 16th 2016
        public LinkMeetingView LastMeeting { get; set; } // For the UI text: Last Meeting: November 16th 2016
        public int MeetingsInLast12Months { get; set; }
    }
}