define(["jquery", "text!App/kocomponents/meeting-history.html", "underscore", "knockout"], function ($, htmlTemplate, _) {
    "use strict";

    var getYears = function (colleagueMeetings) {
        var dateList = $.map(colleagueMeetings, function (item) {
            return item.Year;
        });
        return dateList;
    };

    var normaliseData = function (colleagueMeetings) {
        //Transform a colleague's meetings date into a nested list of year/quarters/meeting links
        //for convenience use by ui
        var meetingTree = {};

        meetingTree.years = getYears(colleagueMeetings);
        meetingTree.years = _.uniq(meetingTree.years);
    };

    //View model
    var viewModel = function (params) {
        var self = this;
        normaliseData(params.data.Meetings);

        return self;
    };

    return {
        viewModel: viewModel,
        template: htmlTemplate
    };
});