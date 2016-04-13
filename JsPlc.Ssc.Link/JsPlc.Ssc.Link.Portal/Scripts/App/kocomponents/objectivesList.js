define(["jquery", "knockout", "text!App/kocomponents/objectivesList.html", "common"], function ($, ko, htmlTemplate, common) {
    "use strict";
    var orderObjectives = function (left, right) {
        return left.LastAmendedDate == right.LastAmendedDate ? 0 : (left.LastAmendedDate > right.LastAmendedDate ? -1 : 1);
    }

    //View model
    function viewModel(params) {
    	var vm = {};
    	vm.objectives = params.data;
        vm.objectives.sort(orderObjectives);
        vm.colleagueFullName = params.colleagueFullName;
        vm.readOnly = params.readOnly !== undefined ? params.readOnly : true;
        vm.managerView = params.managerView !== undefined ? params.managerView : false;

        vm.expandedAll = ko.observable(false);

        vm.expandPrintCollapseAll = function () {
        	vm.expandedAll(true);
        	window.print();
        	vm.expandedAll(false);
        }

        return vm;
    }

    return {viewModel: viewModel, template: htmlTemplate};
});