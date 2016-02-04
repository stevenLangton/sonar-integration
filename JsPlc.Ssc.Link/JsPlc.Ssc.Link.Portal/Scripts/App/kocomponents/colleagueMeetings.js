define(["jquery", "knockout", "text!App/kocomponents/colleagueMeetings.html", "moment", "URI", "underscore", "common", "helpers"], function ($, ko, htmlTemplate, moment, URI, _, common, helpers) {
    "use strict";

    //View model
    function PageViewModel(params) {

        var self = {}; //Luan 2/2/16

        self.myColleagueId = params.ColleagueId || "";

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

        var buildViewModels = function (data) {
            moment.locale("en-gb"); // Set Locale for moment (aka moment.locale("en-gb"))
            //var meetingDate = moment(data.MeetingDate).format("L"); // we get dd/mm/yyyy

            // each ColleagueTeamView item
            _.each(data, function (item) {

                // pre process each item
                item.MeetingDate = moment(item.MeetingDate).format("L");

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
            $.ajax({
                //url: common.getSiteRoot() + "Team/GetMeetings/?myOrTeams=" + myOrTeams, // "MyMeetings" or "TeamMeetings"
                url: common.getSiteRoot() + "Team/GetColleagueMeetings",
                data: {ColleagueId: self.myColleagueId},
                method: "GET"
            })
                .done(function (data) {
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

        return self;//Luan 2/2/2016
    };
    
    var viewModel = {
        createViewModel: function (params, componentInfo) {
            var vm = PageViewModel(params);

            vm.loadPageData("MyMeetings");

            return vm;
        }
    };

       return { viewModel: viewModel, template: htmlTemplate };
   });