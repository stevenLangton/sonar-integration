define(["jquery", "knockout", "text!App/kocomponents/objectivesList.html", "common"], function ($, ko, htmlTemplate, common) {
    "use strict";

    //View model
    function viewModel(params) {
        var self = {};
        self.objectives = params.data;

        return self;
    }

    return {viewModel: viewModel, template: htmlTemplate};
});