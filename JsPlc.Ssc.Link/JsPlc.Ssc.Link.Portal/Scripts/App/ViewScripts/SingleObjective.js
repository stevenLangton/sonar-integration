define(["jquery", "knockout", "komap"], function ($, ko, komap) {
    "use strict";

    var init = function (serverModel) {
        var vm = komap.fromJS(serverModel);
        ko.applyBindings(vm, document.getElementById('ObjectiveView'));
    };

    return {
        init: init
    };
});