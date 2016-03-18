define(["jquery", "knockout", "text!App/kocomponents/objectivesList.html", "common"], function ($, ko, htmlTemplate, common) {
    "use strict";

    //View model
    function viewModel(params) {
        var self = {};
        self.objectives = params.data;
        self.readOnly = params.readOnly !== undefined ? params.readOnly : true;
        self.managerView = params.managerView !== undefined ? params.managerView : false;


        return self;
    }

    return {viewModel: viewModel, template: htmlTemplate};
});