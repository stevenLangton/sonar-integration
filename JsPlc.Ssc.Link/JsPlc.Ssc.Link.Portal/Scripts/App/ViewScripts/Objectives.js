define(["jquery", "knockout", "common", "LinkService"], function ($, ko, common, LinkService) {
    "use strict";

    var makeEmptyObjective = function () {
        var newObj = {
            Id: 0,
            ColleagueId: "",
            ManagerId: "",
            ColleagueSignOff: 0,
            ManagerSignOff: 0,
            CreatedDate: new Date(),
            SignOffDate: "",
            LastAmendedDate: new Date(), //2016-03-11T11:30:48.542Z
            LastAmendedBy: "",
            Objective: "",
            MeasuredBy: "",
            RelevantTo: "",
            Title: ""
        };

        newObj.ColleagueId = common.getUserInfo().colleagueId;
        newObj.ManagerId = common.getUserInfo().managerId;
        newObj.LastAmendedBy = newObj.ColleagueId;

        return newObj;
    };

    //When user cancels the "create a new objective" panel, we want to collapse it
    var onNewObjectiveCancelled = function () {
        $('#demo').collapse('hide');
    };

    var onNewObjectiveSaved = function () {
        $('#demo').collapse('hide');
    };

    //View model
    var ObjectivesListVm = function () {
        var vm = {};

        vm.newObjective = makeEmptyObjective();
        vm.objectives = ko.observableArray([]);

        vm.enterNewObjective = function (data, event) {
            var $newObjButton = $(event.currentTarget);

            if ($newObjButton.attr("aria-expanded") === "false") {
                var $container = $('#objectiveContainer');

                ko.unapplyBindings($container, false);
                $container.empty(); //Remove contents

                //Apply new bindings

                vm.newObjective = makeEmptyObjective(); //Get rid of old object
                var containerViewmodel = {};
                containerViewmodel.data = vm.newObjective;
                containerViewmodel.onNewObjectiveCancelled = onNewObjectiveCancelled;
                containerViewmodel.onNewObjectiveSaved = onNewObjectiveSaved;

                $container.html('<one-objective params="data: $root.data, readOnly: false, expanded: true, enableViewToggle: false, \
                                onCancel: onNewObjectiveCancelled, onSave: onNewObjectiveSaved"></one-objective>');
                ko.applyBindings(containerViewmodel, $container[0]);
            }
        };

        return vm;
    };

    //var init = $.noop;
    var init = function (divId, colleagueId) {
        var $promise = $.ajax({
            url: "objective/GetAllColleagueObjectives",
            type: "get",
            dataType: "json",
            data: { ColleagueId: colleagueId }
        });

        $promise.done(function (result) {
            var result = result;
            var vm = ObjectivesListVm();
            vm.objectives(result);

            //Sort the objectives. Latest amended first
            vm.objectives.sort(function (left, right) { return left.LastAmendedDate == right.LastAmendedDate ? 0 : (left.LastAmendedDate > right.LastAmendedDate ? -1 : 1) });
            ko.applyBindings(vm, document.getElementById(divId));
        });

        $promise.error(function (request, status, error) {
            alert(request.responseText);
        });
    };

    return {
        init: init
    };
});