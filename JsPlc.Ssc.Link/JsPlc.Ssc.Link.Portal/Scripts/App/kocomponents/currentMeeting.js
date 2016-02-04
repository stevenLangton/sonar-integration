define(["jquery", "text!App/kocomponents/currentMeeting.html", "dataService", "meetingService", "knockout"], function ($, htmlTemplate, dataService, meetingService) {
    "use strict";

    //View model
    var viewModel = function (params) {
        var self = params.data;
        self.formatDateMonthDYHM = meetingService.formatDateMonthDYHM; //Borrow a function
        self.getEditOrViewLink = meetingService.getEditOrViewLink
        return self;
    };

    return {
        viewModel: viewModel,
        template: htmlTemplate
    };
});