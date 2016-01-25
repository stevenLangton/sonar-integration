define(["jquery", "knockout", "komap", "moment", "common"], function ($, ko, komap, moment, common) {
    "use strict";

    var yourTeamVm = function () {
        var vm = {};

        vm.teamMembers = ko.observableArray([]);
        return vm;
    };


    var loadPageData = function (vm) {
        //debugger;
        $.ajax({
            url: common.getSiteRoot() + "Team/GetMeetings/?myOrTeams=TeamMeetings", // "MyMeetings" or "TeamMeetings"
            method: "GET"
        })
            .done(function (data) {
                if (data === "Error") {

                } else {
                    vm.teamMembers(data);

                    //Sort the meetings list of each team member so the youngest one appears first
                    ko.utils.arrayForEach(vm.teamMembers(), function (member) {
                        member.Meetings.sort(function (left, right) { return left.MeetingDate == right.MeetingDate ? 0 : (left.MeetingDate > right.MeetingDate ? -1 : 1) });
                    });
                }
            })
            .fail(function () {
            });
    };

    var init = function () {
        var vm = yourTeamVm();
        loadPageData(vm);
        ko.applyBindings(vm, document.getElementById('linkpage'));
    };

    return {
        init: init
    };
});