define(["jquery", "knockout", "dataService", "meetingService", "toastr", "RegisterKoComponents"], function ($, ko, dataService, meetingService, toastr) {
    "use strict";

    var refreshTabContent = function (koViewModel, htmlString) {
        //Remove ko bindings etc..
        ko.unapplyBindings($('#featureContainer'), false);
        $('#featureContainer').empty(); //Remove contents

        //Apply new bindings
        $('#featureContainer').html(htmlString);
        ko.applyBindings(koViewModel, document.getElementById('featureContainer'));
    };

    var showColleagueMeetings = function (colleagueId, data) {
        refreshTabContent(data, "<past-meetings params='data: $root, ColleagueId: \"" + colleagueId + "\"'></past-meetings>");
    };

    var showPdp = function (colleagueId) {
        var $promise = dataService.getPdp(colleagueId);
        $promise.done(function (result) {
            var pdpTabKoVm = result;
            refreshTabContent(pdpTabKoVm, "<pdp-accordion  params='data: $root'></pdp-accordion>");
        });
        $promise.error(function (request, status, error) {
            toastr.error(request.responseText);
        });
    };

    var showObjectives = function (colleagueId) {
        var $promise = dataService.getSharedObjectives(colleagueId);
        $promise.done(function (result) {
            var objectivesTabKoVm = {};
            objectivesTabKoVm.objectives = ko.observableArray(result);
            refreshTabContent(objectivesTabKoVm, "<objectives-list params='data: objectives, readOnly: true'></objectives-list>");
        });
        $promise.error(function (request, status, error) {
            toastr.error(request.responseText);
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
                showColleagueMeetings(vm.colleagueId, vm.data);
                break;
            case 2:
                //Show colleague Pdp
                showPdp(vm.colleagueId);
                break;
            case 3:
                //Show colleague Objectives
                showObjectives(vm.colleagueId);
                break;
            }
        };

        return vm;
    };

    //Module init function
    //var init = function (koBoundDivId, colleagueId) {
    var init = function (koBoundDivId, colleagueMeetings) {
        var vm = viewModel();
        vm.data = meetingService.buildColleagueMeetingsViewModel(colleagueMeetings);
        vm.colleagueId = vm.data.ColleagueId;
        ko.applyBindings(vm, document.getElementById(koBoundDivId));
        vm.tabChanged(1);
    };

    return {
        init: init
    };
});