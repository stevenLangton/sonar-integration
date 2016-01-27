define(["jquery", "knockout", "moment", "URI", "underscore", "common", "helpers"], function ($, ko, moment, URI, _, common, helpers) {
    "use strict";

    function PageViewModel() {

        var self = this;

        self.dataModel = ko.observable(""); // Array of TeamView objects
        self.dataAvailable = ko.observable(false);

        self.bind = function () { };

        var listOfColleagueTeamViews = [];

        self.getEditOrViewLink = function (item) {
            var editLink = common.getSiteRoot() + "LinkForm/Edit/" + item.MeetingId;
            var viewLink = common.getSiteRoot() + "LinkForm/ViewMeeting/" + item.MeetingId;

            return (item.Status == 0) ? editLink : viewLink; // 0 = InComplete
        };

        var getCreateLink = function (colleagueId) {
            var createLink = common.getSiteRoot() + "LinkForm/Create/?colleagueId=" + colleagueId;
            return createLink;
        };

        self.getColleagueName = function (colleague) {
            if (!colleague) return null;

            if (colleague.FirstName && colleague.LastName) {
                return colleague.FirstName + ' ' + colleague.LastName;
            }
            return null;
        };

        self.getColleagueFirstName = function (colleague) {
            if (!colleague) return null;

            if (colleague.FirstName) {
                return colleague.FirstName;
            }
            return null;
        };

        // Extract tree from List<LinkMeetingView>
        var buildMeetingsForMember = function (meetingsData, colleagueId) {

            var allYears = _.pluck(meetingsData, "Year");
            var years = _.uniq(allYears, function (item) { return item; });

            var yearsMore = [];
            var yrId = 0;
            // for each year find periods
            var yearsWithPeriods = _.each(years, function (yr) {

                var yearsMeetings = _.filter(meetingsData, function (item) {
                    return (item.Year === yr);
                });

                var allPeriodsForYear = _.pluck(yearsMeetings, "Period");

                var periods = _.uniq(allPeriodsForYear, function (item) { return item; });

                var periodWithMeetings = [];
                // iterate periods
                var pId = 0;
                _.each(periods, function (period) {
                    var meetingsForPeriod = _.filter(meetingsData, function (item) {
                        return (item.Year === yr && item.Period === period);
                    });
                    meetingsForPeriod = _.sortBy(meetingsForPeriod, "MeetingDate");
                    var sortedMeetings = _.map(meetingsForPeriod, function (item) {
                        return {
                            MeetingDate: moment(item.MeetingDate).format("L"),
                            Status: item.Status, // 1 = Completed, 0 = InComplete
                            ActionLink: self.getEditOrViewLink(item)
                        };
                    });

                    // are there any Incomplete meetings in this period
                    var hasIncompleteMeetings = _.any(sortedMeetings, function (item) {
                        return (item.Status == 0);
                    });

                    pId = pId + 1;
                    periodWithMeetings.push({
                        Period: period,
                        PeriodId: '_year' + yrId + '_period' + pId + '_' + colleagueId,
                        Meetings: sortedMeetings,
                        HasIncompleteMeetings: hasIncompleteMeetings
                    });
                });

                // are there any Incomplete periods in this year
                var hasIncompleteMeetingsInYear = _.any(periodWithMeetings, function (item) { return (item.HasIncompleteMeetings == true); });

                yrId = yrId + 1;
                yearsMore.push({
                    Year: yr,
                    YearId: '_year' + yrId + '_' + colleagueId,
                    Periods: periodWithMeetings,
                    HasIncompleteMeetingsInYear: hasIncompleteMeetingsInYear
                });
            });

            return yearsMore;
        };

        var buildViewModels = function (data) {
            moment.locale("en-gb"); // Set Locale for moment (aka moment.locale("en-gb"))
            //var meetingDate = moment(data.MeetingDate).format("L"); // we get dd/mm/yyyy

            // each ColleagueTeamView item
            _.each(data, function (item) {

                // pre process each item
                item.MeetingDate = moment(item.MeetingDate).format("L");

                debugger;
                var colleague = item.Colleague;
                // Build the item
                var memberView = {
                    Member: colleague,
                    ColleagueId: colleague.ColleagueId,
                    FullName: self.getColleagueName(colleague),
                    ManagerName: (colleague.HasManager) ? self.getColleagueName(colleague.Manager) : '-',
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

                // add item to array
                listOfColleagueTeamViews.push(memberView);
            });

            self.dataModel(listOfColleagueTeamViews);
        };

        self.formatDateMonthDYHM = function (dateObj) {
            if (!dateObj) return '-';

            var formattedString = moment(dateObj).format('dddd, MMMM Do YYYY [at] HH:mma');
            return formattedString;
        }

        // ### GET LinkForm Data (assume there is data, it will show up), we may have to build a Get method which returns a blank Link Meeting template
        self.loadPageData = function (myOrTeams) {
            //debugger;
            $.ajax({
                url: common.getSiteRoot() + "Team/GetMeetings/?myOrTeams=" + myOrTeams, // "MyMeetings" or "TeamMeetings"
                method: "GET"
            })
                .done(function (data) {
                    //debugger;
                    if (data == "Error") {
                        $('#msgs').html("No data");
                        self.dataAvailable(false);
                    } else {
                        //$('#msgs').html("Fetch success");
                        buildViewModels(data);

                        self.dataAvailable(true);
                        self.bind();
                    }
                })
                .fail(function () {
                    self.dataAvailable(false);
                    $('#msgs').html("Error occured");
                });
        };
    }

    function init(myOrTeams) {
        moment.locale("en-gb"); // Set Locale for moment (aka moment.locale("en-gb"))

        var vm = new PageViewModel();

        var binder = function () {
            ko.applyBindings(vm, $("#linkpage")[0]); // important - we have to refer to div element with index 0.
        };

        vm.bind = binder;

        vm.loadPageData(myOrTeams);
    }

    return {
        init: init
    }

});