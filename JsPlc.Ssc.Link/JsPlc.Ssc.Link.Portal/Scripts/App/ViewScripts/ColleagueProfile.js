define(["jquery", "knockout", "dataService", "RegisterKoComponents"], function ($, ko, dataService) {
    "use strict";

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
                break;
            case 2:
                //Show colleague Pdp
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
                //vm.objectives(result);

                //Remove ko bindings etc..
                ko.unapplyBindings($('#featureContainer'), false);
                $('#featureContainer').empty(); //Remove contents

                //Apply new bindings
                $('#featureContainer').html("<objectives-list params='data: objectives'></objectives-list>");
                ko.applyBindings(objectivesTabKoVm, document.getElementById('featureContainer'));
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