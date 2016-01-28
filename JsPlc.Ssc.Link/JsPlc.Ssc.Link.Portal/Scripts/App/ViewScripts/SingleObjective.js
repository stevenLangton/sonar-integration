define(["jquery", "knockout", "komap", "moment", "common", "confirmModal", "RegisterKoComponents"], function ($, ko, komap, moment, common, confirmModal) {
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
        vm.dirtyFlag = ko.oneTimeDirtyFlag(vm);
        vm.displayCreateDate = moment(vm.CreatedDate()).format('LLLL');
        vm.displayUpdateDate = moment(vm.LastAmendedDate()).format('LLLL');

        vm.update = function () {
            var jsonArgs = komap.toJS(vm);
            jsonArgs.CreatedDate = getDateStr(jsonArgs.CreatedDate);
            jsonArgs.LastAmendedDate = getDateStr(jsonArgs.LastAmendedDate);
            jsonArgs.SignOffDate = getDateStr(jsonArgs.SignOffDate);

            var mvcAction = (jsonArgs.Id === 0) ? "objective/create" : "objective/update";

            var $promise = $.ajax({
                data: jsonArgs,
                url: mvcAction,
                type: "post",
                dataType: "json"
            });

            $promise.done(function (result) {
                var result = result;
                if (result.success) {
                     window.location.href = common.getSiteRoot() + "Objective";
                } else {
                    alert("We encountered a problem while processing your request.");
                }
            });


            $promise.error(function (request, status, error) {
                alert(request.responseText);
            });
        };

        vm.cancel = function () {
            if (vm.dirtyFlag()) {

                var yesHandler = function () {
                    window.location.href = common.getSiteRoot() + "Objective";
                };

                confirmModal.init("Edit/View Objective", //Title
                                  "Changes have been made to this Objective. Are you sure you wish to discard them?", //Body text
                                  "No",// txtNoButton 
                                  "Yes"); //txtYesButton

                confirmModal.setProceedHandler(yesHandler);
                confirmModal.show();

            } else {
                window.location.href = common.getSiteRoot() + "Objective";
            }
        };

        ko.applyBindings(vm, document.getElementById('ObjectiveView'));
    };

    return {
        init: init
    };
});