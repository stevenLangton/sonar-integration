define(["jquery", "knockout", "komap", "moment", "toastr", "text!App/kocomponents/oneObjective.html", "dataService", "confirmModal", "RegisterKoComponents"], function ($, ko, komap, moment, toastr, htmlTemplate, dataService, confirmModal) {
    "use strict";
    var getDateStr = function (jsonDate) {
        var moDate = moment(jsonDate);
        if (moDate.isValid()) {
            return moDate.toISOString();
        } else {
            return "";
        }
    };

    var setHandler = function (clientHandler) {
        return (typeof clientHandler === "undefined" || !$.isFunction(clientHandler))
                ? $.noop
                : clientHandler;
    };

    var oneObjectiveModel = function (params) {
        var vm = {};

        vm.onCreate = setHandler(params.onCreate);
        vm.onSave = setHandler(params.onSave);
        vm.onCancel = setHandler(params.onCancel);

        vm.data = komap.fromJS(params.data);//params.data is JSON form of a LinkObjective server object
        vm.readOnly = params.readOnly !== undefined ? params.readOnly : true;
        vm.expanded = params.expanded !== undefined ? params.expanded : false;
        vm.enableViewToggle = params.enableViewToggle !== undefined ? params.enableViewToggle : true;
        vm.expandedView = ko.observable(vm.expanded);

        vm.dirtyFlag = ko.oneTimeDirtyFlag(vm.data);
        //vm.displayCreateDate = moment(vm.data.CreatedDate()).format('LLLL');
        //vm.displayUpdateDate = moment(vm.data.LastAmendedDate()).format('LLLL');

        vm.update = function () {
            var jsonArgs = komap.toJS(vm.data);
            if ($.isEmptyObject(jsonArgs.Title)) {
                toastr.info("Please enter the title for the new objective");
                return;
            }

            jsonArgs.CreatedDate = getDateStr(jsonArgs.CreatedDate);
            jsonArgs.LastAmendedDate = getDateStr(jsonArgs.LastAmendedDate);
            jsonArgs.SignOffDate = getDateStr(jsonArgs.SignOffDate);

            var mvcCreateAction = "objective/create";
            var mvcAction = (jsonArgs.Id === 0)
                ? mvcCreateAction
                : "objective/update";

            var $promise = $.ajax({
                data: jsonArgs,
                url: mvcAction,
                type: "post",
                dataType: "json"
            });

            $promise.done(function (result) {
                if (result.success) {
                    vm.data.LastAmendedDate(result.savedObjective.LastAmendedDate);
                    vm.data.LastAmendedBy(result.savedObjective.LastAmendedBy);

                    if (mvcAction === mvcCreateAction) {
                        toastr.info("You have successfully created a new objective");
                        vm.onCreate(result.savedObjective);
                    } else {
                        toastr.info("Your objective has been updated");
                    }

                    //We've just saved. So refresh the dirty flag
                    vm.dirtyFlag = ko.oneTimeDirtyFlag(vm.data);
                    vm.onSave();
                } else {
                    toastr.error("We encountered a problem while processing your request.");
                }
            });


            $promise.error(function (request) {
                toastr.error(request.responseText);
            });
        };

        vm.cancel = function () {

            if (vm.dirtyFlag()) {
                var dialogOptions = {};

                var yesHandlerForNew = function () {
                    vm.onCancel();
                };

                var yesHandlerForUpdates = function () {
                    //Restore the data as user cancels modifications
                    var $promise = dataService.getOneObjective(vm.data.Id());
                    $promise.done(function (data) {
                        komap.fromJS(data, vm.data);
                        vm.dirtyFlag = ko.oneTimeDirtyFlag(vm.data);
                        vm.onCancel();
                    });
                };

                if (vm.data.Id() === 0) {
                    //Create new objective view
                    dialogOptions = {
                        yesHandler: yesHandlerForNew,
                        titleText: "Add A New Objective",
                        bodyText: "Are you sure? This objective has not been saved. Are you sure you want to discard it?"
                    };
                } else {
                    //Update existing objective view
                    dialogOptions = {
                        titleText: "Edit/View Objective",
                        bodyText: "You have modified this objective. Are you sure you want to discard it?",
                        yesHandler: yesHandlerForUpdates
                    };
                }
                confirmModal.init(dialogOptions);

                confirmModal.show();
            } else {
                vm.toggleView();
                vm.onCancel();
            }

        };

        vm.toggleView = function () {
            //var $childIcon = $(event.currentTarget).children("icon");
            //if ($childIcon.hasClass("fa-plus")) {
            //    //Open expanded view
            //    $childIcon.removeClass("fa-plus");
            //    $childIcon.addClass("fa-minus");
            //    vm.expandedView(true);
            //} else {
            //    //Open collapsed view
            //    $childIcon.removeClass("fa-minus");
            //    $childIcon.addClass("fa-plus");
            //    vm.expandedView(false);
            //}
            vm.expandedView(!vm.expandedView());
        };

        //ko.applyBindings(vm, document.getElementById('ObjectiveView'));
        //$('textarea').autogrow({ onInitialize: true });

        return vm;
    };

    var viewModel = {
        createViewModel: function (params, componentInfo) {
            var vm = oneObjectiveModel(params);
            return vm;
        }
    };

    return {viewModel: viewModel, template: htmlTemplate};
});