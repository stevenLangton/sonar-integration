define(["jquery", "knockout", "komap", "moment", "common", "confirmModal", "RegisterKoComponents"], function ($, ko, komap, moment, common, confirmModal) {
    "use strict";

    var viewModel = function () {
        var vm = {};
        vm.tabChanged = function (tabNo) {
            alert(tabNo);
        };
        return vm;
    };

    var init = function (koBoundDivId) {
        var vm = viewModel();
        ko.applyBindings(vm, document.getElementById(koBoundDivId));
    };

    return {
        init: init
    };
});