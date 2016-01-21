define(["jquery", "knockout", "komap", "moment"], function ($, ko, komap, moment) {
    "use strict";

    var init = function (serverModel) {
        var vm = komap.fromJS(serverModel);
        vm.formattedCreateDate = moment(vm.CreateDate);
        ko.applyBindings(vm, document.getElementById('ObjectiveView'));
    };

    return {
        init: init
    };
});