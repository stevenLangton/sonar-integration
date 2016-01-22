define(["jquery", "knockout", "komap", "moment"], function ($, ko, komap, moment) {
    "use strict";
    var getDateStr = function (jsonDate) {
        var moDate = moment(jsonDate);
        if (moDate.isValid()) {
            return moDate.toISOString();
        } else {
            return "";
        }
    };

    var init = function (serverModel) {
        var vm = komap.fromJS(serverModel);
        vm.displayCreateDate = moment(vm.CreatedDate()).format('LLLL');
        vm.displayUpdateDate = moment(vm.LastAmendedDate()).format('LLLL');

        vm.update = function () {
            var jsonArgs = komap.toJS(vm);
            //jsonArgs.CreatedDate = vm.displayCreateDate;
            //jsonArgs.LastAmendedDate = vm.displayUpdateDate;
            jsonArgs.CreatedDate = getDateStr(jsonArgs.CreatedDate);
            jsonArgs.LastAmendedDate = getDateStr(jsonArgs.LastAmendedDate);
            jsonArgs.SignOffDate = getDateStr(jsonArgs.SignOffDate);

            var $promise = $.ajax({
                data: jsonArgs,
                url: "objective/objective",
                type: "put",
                dataType: "json"
            });

            $promise.done(function (result) {
                var result = result;
            });

            $promise.error(function (request, status, error) {
                alert(request.responseText);
            });
        };

        ko.applyBindings(vm, document.getElementById('ObjectiveView'));
    };

    return {
        init: init
    };
});