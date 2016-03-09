define(["jquery", "text!App/kocomponents/tabPdp.html", "underscore", "knockout"], function ($, htmlTemplate, _) {
    "use strict";

    var samplePdpModel = {
        readOnly: false,
        sections: [
            {
                name: "Section 1",
                questions: [{name: "Question1" , answer: "The sun has risen" }, {name: "Question2" , answer: "The sun has risen" }, {name: "Question3" , answer: "The sun has risen" }, {name: "Question4" , answer: "The sun has risen" }, {name: "Question5" , answer: "The sun has risen" }]
           },
            {
                name: "Section 2",
                questions: [{name: "Question1" , answer: "The sun has risen" }, {name: "Question2" , answer: "The sun has risen" }, {name: "Question3" , answer: "The sun has risen" }, {name: "Question4" , answer: "The sun has risen" } ]
           },
            {
                name: "Section 3",
                questions: [{name: "Question1" , answer: "The sun has risen" }, {name: "Question2" , answer: "The sun has risen" }, {name: "Question3" , answer: "The sun has risen" }, {name: "Question4" , answer: "The sun has risen" }, {name: "Question5" , answer: "The sun has risen" }, {name: "Question6" , answer: "The sun has risen" }]
           },
            {
                name: "Section 4",
                questions: [{name: "Question1" , answer: "The sun has risen" }, {name: "Question2" , answer: "The sun has risen" }, {name: "Question3" , answer: "The sun has risen" }, {name: "Question4" , answer: "The sun has risen" }, {name: "Question5" , answer: "The sun has risen" }]
           },
            {
                name: "Section 5",
                questions: [{name: "Question1" , answer: "The sun has risen" }, {name: "Question2" , answer: "The sun has risen" }, {name: "Question3" , answer: "The sun has risen" }, {name: "Question4" , answer: "The sun has risen" }]
           },
            {
                name: "Section 6",
                questions: [{ name: "Question1", answer: "The time is now" },
                    { name: "Question2", answer: "The sun has risen" },
                    { name: "Question3", answer: "Channel 4 news" },
                    { name: "Question4", answer: "The sun has risen" }, { name: "Question5" , answer: "The sun has risen" }, { name: "Question6" , answer: "The sun has risen" }]
           }
        ]
   };

    //View model
    var viewModel = function (params) {
        //var self = params.data;

        //Test only.to be removed
        var self = samplePdpModel;

        return self;
   };

    return {
        viewModel: viewModel,
        template: htmlTemplate
   };
});