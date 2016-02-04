define(["jquery", "knockout", "text!App/kocomponents/colleagueMeetings.html", "moment", "URI", "underscore", "common", "helpers", "meetingService"], function ($, ko, htmlTemplate, moment, URI, _, common, helpers, meetingService) {
    "use strict";

    //View model
    function PageViewModel(params) {

        var self = {}; //Luan 2/2/16

        self.myColleagueId = params.ColleagueId || "";

        self.dataModel = ko.observable(""); // Array of TeamView objects

        self.meetingService = meetingService;//04/2/16 Luan

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
                    } else {
                        //buildViewModels(data);
                        self.dataModel(meetingService.buildColleagueMeetingsViewModel(data[0]));
                    }
                })
                .fail(function () {
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