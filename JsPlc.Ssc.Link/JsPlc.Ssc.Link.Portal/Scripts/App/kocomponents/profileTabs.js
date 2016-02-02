﻿define(["jquery", "knockout", "text!App/kocomponents/profileTabs.html", "RegisterKoComponents"], function ($, ko, htmlTemplate) {
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

    return { viewModel: tabModel, template: htmlTemplate };
});