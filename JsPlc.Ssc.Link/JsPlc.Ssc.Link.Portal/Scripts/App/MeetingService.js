define(["jquery", "knockout", "moment", "URI", "underscore", "common"], function ($, ko, moment, URI, _, common) {
    "use strict";

    var getEditOrViewLink = function (item) {
        var editLink = common.getSiteRoot() + "LinkForm/Edit/" + item.MeetingId;
        var viewLink = common.getSiteRoot() + "LinkForm/ViewMeeting/" + item.MeetingId;

        return (item.Status == 0) ? editLink : viewLink; // 0 = InComplete
    };

    var getCreateLink = function (colleagueId) {
        var createLink = common.getSiteRoot() + "LinkForm/Create/?colleagueId=" + colleagueId;
        return createLink;
    };

    var getColleagueName = function (colleague) {
        if (!colleague) {
            return null;
        }

        if (colleague.FirstName && colleague.LastName) {
            return colleague.FirstName + ' ' + colleague.LastName;
        }
        return null;
    };

    var getColleagueFirstName = function (colleague) {
        if (!colleague) {
            return null;
        }

        if (colleague.FirstName) {
            return colleague.FirstName;
        }
        return null;
    };

    var buildColleagueMeetingsViewModel = function (item) {
        moment.locale("en-gb"); // Set Locale for moment (aka moment.locale("en-gb"))
        //var meetingDate = moment(data.MeetingDate).format("L"); // we get dd/mm/yyyy

        item.MeetingDate = moment(item.MeetingDate).format("L");

        var colleague = item.Colleague;
        // Build the item
        var memberView = {
            Member: colleague,
            ColleagueId: colleague.ColleagueId,
            FullName: getColleagueName(colleague),
            ManagerName: (colleague.HasManager) ? getColleagueName(colleague.Manager) : '-',
            ManagerFirstName: (colleague.HasManager) ? colleague.Manager.FirstName : '-',
            //HasMeetings: false,

            // List<LinkMeetingView>
            UpcomingMeetings: item.UpcomingMeetings,
            // List<LinkMeetingView>
            PastMeetings: item.PastMeetings,

            LastInCompleteMeeting: item.LastInCompleteMeeting,
            LatestMeeting: item.LatestMeeting,
            LastMeeting: item.LastMeeting,

            MeetingsInLast12Months: item.MeetingsInLast12Months,

            CreateLink: getCreateLink(colleague.ColleagueId)
        };

        return memberView;
    };

    var formatDateMonthDYHM = function (dateObj) {
        if (!dateObj) {
            return '-';
        }

        var formattedString = moment(dateObj).format('dddd, MMMM Do YYYY [at] HH:mma');
        return formattedString;
    };

    return {
        formatDateMonthDYHM: formatDateMonthDYHM,
        getEditOrViewLink: getEditOrViewLink,
        getCreateLink: getCreateLink,
        getColleagueName: getColleagueName,
        getColleagueFirstName: getColleagueFirstName,
        buildColleagueMeetingsViewModel: buildColleagueMeetingsViewModel
    };
});