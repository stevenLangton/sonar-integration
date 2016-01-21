define(["jquery", "knockout", "komap", "moment"], function ($, ko, komap, moment) {
    "use strict";

    var init = function (serverModel) {
        var vm = komap.fromJS(serverModel);
        vm.displayCreateDate = moment(vm.CreatedDate()).format('LLLL');
        vm.displayUpdateDate = moment(vm.LastAmendedDate()).format('LLLL');
        ko.applyBindings(vm, document.getElementById('ObjectiveView'));
    };

    return {
        init: init
    };
});