define(["jquery", "knockout", "komap", "moment", "common", "toastr", "confirmModal", "autogrow", "RegisterKoComponents"], function ($, ko, komap, moment, common, toastr, confirmModal, autogrow) {
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

            var mvcCreateAction = "objective/create";
            var mvcAction = (jsonArgs.Id === 0) ? mvcCreateAction : "objective/update";

            var $promise = $.ajax({
                data: jsonArgs,
                url: mvcAction,
                type: "post",
                dataType: "json"
            });

            $promise.done(function (result) {
                var result1 = result;
                if (result1.success) {
                    if (mvcAction === mvcCreateAction) {
                        toastr.info("You have successfully created a new objective");
                    }
                    else {
                        toastr.info("Your objective has been updated");
                    }
                    helpers.utils.redirectWithDelay(common.siteUrls.getObjectives);
                    //window.location.href = common.getSiteRoot() + "Objective";
                } else {
                    toastr.error("We encountered a problem while processing your request.");
                }
            });


            $promise.error(function (request, status, error) {
                alert(request.responseText);
            });
        };

        vm.cancel = function () {

            if (vm.dirtyFlag()) {
                var dialogOptions = {};

                var yesHandler = function () {
                    //window.location.href = common.getSiteRoot() + "Objective";
                    history.go(-1);
                };

                if (vm.Id() === 0) {
                    //Create new objective view
                    dialogOptions = {
                        yesHandler: yesHandler,
                        titleText: "Add A New Objective",
                        bodyText: "Are you sure? This objective has not been saved. Are you sure you want to discard it?"
                    };
                } else {
                    //Update existing objective view
                    dialogOptions = {
                        titleText: "Edit/View Objective",
                        bodyText: "You have modified this objective. Are you sure you want to discard it?",
                        yesHandler: yesHandler
                    };
                }
                confirmModal.init(dialogOptions);

                confirmModal.show();

            } else {
                //window.location.href = common.getSiteRoot() + "Objective";
                history.go(-1);
            }

        };

        ko.applyBindings(vm, document.getElementById('ObjectiveView'));
        $('textarea').autogrow({ onInitialize: true });
    };

    return {
        init: init
    };
});