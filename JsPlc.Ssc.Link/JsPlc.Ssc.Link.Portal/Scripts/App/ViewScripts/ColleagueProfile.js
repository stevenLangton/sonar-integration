define(["jquery", "knockout", "dataService", "RegisterKoComponents"], function ($, ko, dataService) {
    "use strict";

    var refreshTabContent = function (koViewModel, htmlString) {
        //Remove ko bindings etc..
        ko.unapplyBindings($('#featureContainer'), false);
        $('#featureContainer').empty(); //Remove contents

        //Apply new bindings
        $('#featureContainer').html(htmlString);
        ko.applyBindings(koViewModel, document.getElementById('featureContainer'));
    };

    var showPdp = function (colleagueId) {
        var $promise = dataService.getPdp(colleagueId);
        $promise.done(function (result) {
            var pdpTabKoVm = {};
            refreshTabContent(pdpTabKoVm, "<pdp-accordion></pdp-accordion>");
        });
    };

    var viewModel = function () {
        var vm = {};

        //Properties
        vm.colleagueId = "";
        //vm.objectives = ko.observableArray([]);

        //Methods
        vm.tabChanged = function (tabNo) {
            switch (tabNo) {
            case 1:
                //Show colleague Meetings
                refreshTabContent();
                break;
            case 2:
                //Show colleague Pdp
                showPdp(vm.colleagueId);
                break;
            case 3:
                //Show colleague Objectives
                vm.showObjectives(vm.colleagueId);
                break;
            }
        };

        vm.showObjectives = function (colleagueId) {
            var $promise = dataService.getObjectives(colleagueId);
            $promise.done(function (result) {
                var objectivesTabKoVm = {};
                objectivesTabKoVm.objectives = ko.observableArray(result);
                refreshTabContent(objectivesTabKoVm, "<objectives-list params='data: objectives'></objectives-list>");
            });
        };

        return vm;
    };

    //Module init function
    var init = function (koBoundDivId, colleagueId) {
        var vm = viewModel();
        vm.colleagueId = colleagueId;
        ko.applyBindings(vm, document.getElementById(koBoundDivId));
    };

    return {
        init: init
    };
});