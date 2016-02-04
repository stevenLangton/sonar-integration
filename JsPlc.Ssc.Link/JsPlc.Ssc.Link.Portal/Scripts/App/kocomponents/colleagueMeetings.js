define(["jquery", "knockout", "text!App/kocomponents/colleagueMeetings.html", "moment", "URI", "underscore", "common", "helpers", "meetingService", "dataService"],
function ($, ko, htmlTemplate, moment, URI, _, common, helpers, meetingService, dataService) {
    "use strict";

    //View model
    function PageViewModel(params) {

        var self = {};

        // params.data is an object returned from meetingService.buildColleagueMeetingsViewModel
        self.dataModel = ko.observable(params.data);

        //To provide methods used in the templates
        self.meetingService = meetingService;

        return self;
    };
    
    var viewModel = {
        createViewModel: function (params, componentInfo) {
            var vm = PageViewModel(params);
            return vm;
        }
    };

       return { viewModel: viewModel, template: htmlTemplate };
   });