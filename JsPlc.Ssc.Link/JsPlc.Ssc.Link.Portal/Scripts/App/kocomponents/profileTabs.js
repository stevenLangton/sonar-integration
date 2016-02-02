define(["jquery", "knockout", "text!App/kocomponents/profileTabs.html", "RegisterKoComponents"], function ($, ko, htmlTemplate) {
    "use strict";

    //View model
    var tabModel = function (params) {
        var self = this;
        self.activeTab = '';
        self.tabChangedHandler = params.OnTabChanged || $.noop;

        self.showTab = function () {
            var $selectedTab = $(event.target.parentElement);
            var tabNumber = Number($selectedTab.attr("id").slice(-1));

            if (!$selectedTab.hasClass('active')) {
                $selectedTab.addClass('active');
                $('#profileTabs li').not($selectedTab).removeClass('active');

                self.tabChangedHandler(tabNumber);
            }
        };

        $('#showTab3').on('click', self.showTab);
        $('#showTab2').on('click', self.showTab);
        $('#showTab1').on('click', self.showTab);
    };

    var viewModel = {
        createViewModel: function (params, componentInfo) {
            var vm = new tabModel(params);

            //$(componentInfo.element).on('show', function (event, tabNo) {
            //    $("#showTab" + tabNo).trigger("click");
            //});

            return vm;
        }
    };

    return { viewModel: viewModel, template: htmlTemplate };
});