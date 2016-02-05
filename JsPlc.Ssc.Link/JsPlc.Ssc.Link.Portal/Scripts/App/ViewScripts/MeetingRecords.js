define(["jquery", "knockout", "moment", "URI", "underscore", "common", "helpers"], function ($, ko, moment, URI, _, common, helpers) {
    "use strict";

    function PageViewModel() {

        var self = this;

        self.dataModel = ko.observable(""); // Array of TeamView objects
        self.dataAvailable = ko.observable(false);
        self.dataNotAvailableReason = ko.observable('');

        self.bind = function () { };

        var listOfMeetings = [];

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

        self.getColleagueAposName = function (colleague) {
            if (!colleague) return null;

            var lenFname = colleague.FirstName.length;
            if (colleague.FirstName) {
                return (colleague.FirstName.substring(lenFname - 1, lenFname) == 's') ?
                    colleague.FirstName + "'" : colleague.FirstName + "'s";
            }
            return null;
        };


        var buildViewModels = function (data) {
            moment.locale("en-gb"); // Set Locale for moment (aka moment.locale("en-gb"))
            //var meetingDate = moment(data.MeetingDate).format("L"); // we get dd/mm/yyyy

            debugger;

            // each ColleagueTeamView item
            _.each(data, function (meetingItem) {

                // pre process each item
                //item.MeetingDate = moment(item.MeetingDate).format("L");

                // Build the item from LinkMeetingView
                var meetingView = { 
                    Meeting: meetingItem,
                    CreateLink: getCreateLink(meetingItem.Colleague.ColleagueId),
                    MeetingLink: self.getEditOrViewLink(meetingItem)
                };

                // add item to array
                listOfMeetings.push(meetingView);
            });
            //self.dataModel('');

            self.dataModel(listOfMeetings);
        };

        self.formatDateMonthDYHM = function (dateObj) {
            if (!dateObj) return '-';

            var formattedString = moment(dateObj).format('dddd, MMMM Do YYYY [at] HH:mma');
            return formattedString;
        }

        // ### GET PastMeetings Data (assume there is data, it will show up)
        self.loadPageData = function () {
            //debugger;
            $.ajax({
                url: common.getSiteRoot() + "Team/GetTeamsMeetingRecords",
                method: "GET"
            })
                .done(function (data) {
                    //debugger;
                    if (data == "Error" || data == "AccessDenied") { // We can bind diff views based on this value
                        $('#msgs').html("No data");
                        self.dataAvailable(false);
                        self.dataNotAvailableReason(data); // set value to enable diff views
                    } else {
                        //$('#msgs').html("Fetch success");
                        buildViewModels(data);
                        self.dataAvailable(true);
                        self.dataNotAvailableReason('');
                        self.bind();
                    }
                })
                .fail(function () {
                    $('#msgs').html("Error occured");
                    self.dataAvailable(false);
                    self.dataNotAvailableReason("Fail"); // set value to enable diff views
                });
        };
    }

    function init() {
        moment.locale("en-gb"); // Set Locale for moment (aka moment.locale("en-gb"))

        var vm = new PageViewModel();

        var binder = function () {
            ko.applyBindings(vm, $("#linkpage")[0]); // important - we have to refer to div element with index 0.
        };

        vm.bind = binder;

        vm.loadPageData();
    }

    return {
        init: init
    }

});