define(["jquery", "knockout", "LinkService"], function ($, ko, LinkService) {
    "use strict";

    //View model
    var ObjectivesListVm = function () {
        var vm = {};

        vm.objectives = ko.observableArray([]);

        return vm;
    };

    var bindViewModel = function () {

    };

    //var init = $.noop;
    var init = function (divId) {
        var $promise = $.ajax({
            url: "objective/GetAllColleagueObjectives",
            type: "get",
            dataType: "json"
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