define(["jquery", "text!App/kocomponents/pdpAccordion.html", "underscore", "knockout"], function ($, htmlTemplate, _) {
    "use strict";

    //View model
    var viewModel = function (params) {
        var self = params.data;

        return self;
    };

    return {
        viewModel: viewModel,
        template: htmlTemplate
    };
});