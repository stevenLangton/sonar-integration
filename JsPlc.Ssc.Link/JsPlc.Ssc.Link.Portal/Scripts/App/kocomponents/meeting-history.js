define(["jquery", "knockout", "text!App/kocomponents/meeting-history.html", "common"], function ($, ko, htmlTemplate, common) {
    var siteRoot = common.getSiteRoot() || "/";


    //View model
    function viewModel(params) {
        var self = this;

        return self;
    }

    return { viewModel: viewModel, template: htmlTemplate };
});