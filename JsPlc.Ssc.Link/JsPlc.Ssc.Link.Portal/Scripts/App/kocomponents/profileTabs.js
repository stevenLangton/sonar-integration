define(["jquery", "knockout", "text!App/kocomponents/profileTabs.html", "RegisterKoComponents"], function ($, ko, htmlTemplate) {
    "use strict";

    //View model
    function testModel(params) {
        var self = this;
        self.activeTab = '';
        self.tabChangedHandler = params.OnTabChanged || $.noop;

        self.showTab1 = function () {
            var $selectedTab = $(event.target.parentElement);

            if (!$selectedTab.hasClass('active')) {
                //Manage tab labels
                $selectedTab.addClass('active');
                $('#profileTabs li').not($selectedTab).removeClass('active');

                self.tabChangedHandler(1);//Tab number (1 based)

                //Remove ko bindings etc..
                ko.unapplyBindings($('#featureContainer'), false);
                $('#featureContainer').empty(); //Remove contents

                //Apply new bindings
                $('#featureContainer').html("<ranged-items-view></ranged-items-view>");
                ko.applyBindings({}, document.getElementById('featureContainer'));
            }
        };

        self.showTab2 = function ($selectedTab) {
            var $selectedTab = $(event.target.parentElement);

            if (!$selectedTab.hasClass('active')) {
                    //Manage tab labels
                $selectedTab.addClass('active');
                $('#profileTabs li').not($selectedTab).removeClass('active');

                self.tabChangedHandler(2);//Tab number (1 based)

                //Update tab contents here

                //Remove ko bindings etc..
                ko.unapplyBindings($('#featureContainer'), false);
                $('#featureContainer').empty(); //Remove contents

                //Apply new bindings
                $('#featureContainer').html("<unranged-stores-view></unranged-stores-view>");
                ko.applyBindings({}, document.getElementById('featureContainer'));
            }
        };

        self.showTab3 = function (event) {
            var $selectedTab = $(event.target.parentElement);

            if (!$selectedTab.hasClass('active')) {
                //Manage tab labels
                $selectedTab.addClass('active');
                $('#profileTabs li').not($selectedTab).removeClass('active');

                self.tabChangedHandler(3);//Tab number (1 based)

                //Update tab contents here

                //Remove ko bindings etc..
                ko.unapplyBindings($('#featureContainer'), false);
                $('#featureContainer').empty(); //Remove contents

                //Apply new bindings
                $('#featureContainer').html("<unranged-stores-view></unranged-stores-view>");
                ko.applyBindings({}, document.getElementById('featureContainer'));
            }
        };

        self.refreshTab = function () {
            //if (self.activeTab === 'ranged') {
            //    $('#showTab2').removeClass('disableLink');
            //    $('#tab2').removeClass('disableTab');
            //}

            //if (self.activeTab === 'ranged') {
            //    $('ranged-items-view').trigger('refreshTable');
            //} else {
            //    $('unranged-stores-view').trigger('refreshTable');
            //}
        };

        $('#showTab3').on('click', self.showTab3);
        $('#showTab2').on('click', self.showTab2);
        $('#showTab1').on('click', self.showTab1);
    };

    var viewModel = {
        createViewModel: function (params, componentInfo) {
            var vm = new testModel(params);

            $(componentInfo.element).on('refreshTab', function () {
                vm.refreshTab();
            });

            $(componentInfo.element).on('initialiseTab', function(event, tabName) {
                vm.activeTab = tabName;
                vm.refreshTab();
            });

            return vm;
        }
    };

    return { viewModel: viewModel, template: htmlTemplate };
});