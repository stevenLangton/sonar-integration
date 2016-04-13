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

    var showPdp = function (colleagueId, data) {
        var $promise = dataService.getPdp(colleagueId);
        $promise.done(function (result) {
        	var pdpTabKoVm = result;
        	pdpTabKoVm.data = data;
            refreshTabContent(pdpTabKoVm, "<pdp-accordion  params='data: $root'></pdp-accordion>");
        });
        $promise.error(function (request, status, error) {
            toastr.error(request.responseText);
        });
    };

    var showObjectives = function (colleagueId, data) {
        var $promise = dataService.getSharedObjectives(colleagueId);
        $promise.done(function (result) {
            var objectivesTabKoVm = {};
            objectivesTabKoVm.objectives = ko.observableArray(result);
            refreshTabContent(objectivesTabKoVm, "<objectives-list params='data: objectives, readOnly: true, managerView: true, colleagueFullName: \""+data.FullName+"\"'></objectives-list>");
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
            	showPdp(vm.colleagueId, vm.data);
                break;
            case 3:
                //Show colleague Objectives
            	showObjectives(vm.colleagueId, vm.data);
                break;
            }
        };

        return vm;
    };

    var gotMeetingsData = function (colleagueMeetings, koBoundDivId) {
        var vm = viewModel();
        vm.data = meetingService.buildColleagueMeetingsViewModel(colleagueMeetings);
        vm.colleagueId = vm.data.ColleagueId;
        ko.applyBindings(vm, document.getElementById(koBoundDivId));
        vm.tabChanged(1);
    };

    var init = function (koBoundDivId, colleagueId) {
        var $promise = dataService.getColleagueMeetings(colleagueId);

        $promise.done(function (result) { gotMeetingsData(result.data, koBoundDivId) });//result.data is ColleagueTeamView

        $promise.fail(function () { toastr.error("We encountered a problem while retrieving data")})
    };

    return {
        init: init
    };
});