define(["jquery", "knockout", "text!App/kocomponents/objectivesList.html", "common"], function ($, ko, htmlTemplate, common) {
    "use strict";

    //View model
    function viewModel(params) {
        var vm = {};
        vm.objectives = params.data;
        vm.readOnly = params.readOnly !== undefined ? params.readOnly : true;
        vm.managerView = params.managerView !== undefined ? params.managerView : false;

        vm.expandedAll = ko.observable(false);
        vm.expandCollapseAll = function () {
        	vm.expandedAll(true);
        	window.print();
        	vm.expandedAll(false);
        }

        return vm;
    }

    return {viewModel: viewModel, template: htmlTemplate};
});