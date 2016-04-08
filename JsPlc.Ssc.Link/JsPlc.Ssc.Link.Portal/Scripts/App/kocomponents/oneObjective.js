define(["jquery", "knockout", "komap", "moment", "toastr", "text!App/kocomponents/oneObjective.html", "dataService", "common", "confirmModal", "meetingService", "RegisterKoComponents"], function ($, ko, komap, moment, toastr, htmlTemplate, dataService, common, confirmModal, meetingService) {
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

        //Reference common module
        vm.meetingService = meetingService;

        vm.onCreate = setHandler(params.onCreate);
        vm.onSave = setHandler(params.onSave);
        vm.onCancel = setHandler(params.onCancel);

        vm.data = komap.fromJS(params.data);//params.data is JSON form of a LinkObjective server object
        vm.readOnly = params.readOnly !== undefined ? params.readOnly : true;

        //Once approved it's read only.
        if (vm.data.Approved()) {
            vm.readOnly = true;
        }
        vm.managerView = params.managerView !== undefined ? params.managerView : false;

        //Show expanded view initially. We default to collapsed view normally.
        vm.expanded = params.expanded !== undefined ? params.expanded : false;

        //Show/hide the +/- top right view toggle control
        vm.enableViewToggle = params.enableViewToggle !== undefined ? params.enableViewToggle : true;

        vm.getManagerName = function () {
            var mgrName = common.getUserInfo().managerName;
            if (vm.managerView) {
                if (common.getUserInfo().colleagueId === vm.data.ManagerId()) {
                    mgrName = "you";
                }
                else {
                    mgrName = "another manager";
                }
            } else {
                mgrName = mgrName !== "" ? mgrName : "manager";
            }
            return mgrName;
        };

        vm.getStatusMessage = function () {
            var shared = vm.data.SharedWithManager(),
                approved = vm.data.Approved(),
                dateStr = "",
                mgrName = vm.getManagerName(),
                statusMsg = "";

            if (shared) {
                if (approved) {
                    dateStr = moment(vm.data.DateApproved()).format("L"); // dd/mm/yyyy
                    statusMsg = "Approved by " + mgrName + " on " + dateStr;
                } else {
                    dateStr = moment(vm.data.DateShared()).format("L"); // dd/mm/yyyy
                    statusMsg = vm.managerView
                        ? "Shared for your approval on " + dateStr
                        : "Shared with " + mgrName + " on " + dateStr;
                }
            } else {
                statusMsg = "Share this objective with " + mgrName;
            }

            return statusMsg;
        };

        vm.getSubMessage = function () {
            var shared = vm.data.SharedWithManager(),
                mgrName = vm.getManagerName();

            var subMessage = "";

            if (!shared) {
                subMessage = "Allow " + mgrName + " to view and approve this objective";
            }

            return subMessage;
        };

        vm.expandedView = ko.observable(vm.expanded);
        vm.expandedAll = params.expandedAll;
        vm.statusMessage = ko.observable(vm.getStatusMessage());
        vm.subMessage = ko.observable(vm.getSubMessage());

        vm.dirtyFlag = ko.oneTimeDirtyFlag(vm.data);

        vm.update = function () {
            var jsonArgs = komap.toJS(vm.data);
            if ($.isEmptyObject(jsonArgs.Title)) {
                toastr.info("Please enter the title for the new objective");
                return;
            }

            jsonArgs.CreatedDate = getDateStr(jsonArgs.CreatedDate);
            jsonArgs.LastAmendedDate = getDateStr(jsonArgs.LastAmendedDate);
            jsonArgs.SignOffDate = getDateStr(jsonArgs.SignOffDate);
            jsonArgs.DateShared = getDateStr(jsonArgs.DateShared);
            jsonArgs.DateApproved = getDateStr(jsonArgs.DateApproved);

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
                    toastr.error(result.message);
                }
            });


            $promise.error(function (request) {
                toastr.error(request.responseText);
            });
        };

        vm.confirmDiscard = function (calledByToggle) {
            //Can be called from the "Cancel" button or the +/- toggle
            var dialogOptions = {};

            var yesHandlerForNew = function () {
                if (calledByToggle) {
                    vm.expandedView(false);
                }
                vm.onCancel();
            };

            var yesHandlerForUpdates = function () {
                //Restore the data as user cancels modifications
                var $promise = dataService.getOneObjective(vm.data.Id());
                $promise.done(function (data) {
                    komap.fromJS(data, vm.data);
                    vm.dirtyFlag = ko.oneTimeDirtyFlag(vm.data);
                    vm.statusMessage(vm.getStatusMessage());
                    vm.subMessage(vm.getSubMessage());
                    if (calledByToggle) {
                        vm.expandedView(false);
                    }
                    vm.onCancel();
                });
            };

            if (vm.data.Id() === 0) {
                //Create new objective view
                dialogOptions = {
                    titleText: "Add A New Objective",
                    bodyText: "Are you sure? This objective has not been saved. Are you sure you want to discard it?",
                    yesHandler: yesHandlerForNew
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
        };//confirmDiscard

        vm.cancel = function () {
            if (vm.dirtyFlag()) {
                vm.confirmDiscard(false);
            } else {
                vm.toggleView();
                vm.onCancel();
            }
        };

        vm.toggleView = function () {
            if (vm.dirtyFlag()) {
                if (vm.expandedView()) {
                    vm.confirmDiscard(true);
                }
            } else {
                vm.expandedView(!vm.expandedView());
            }
        };

        vm.share = function (data) {
            data.data.SharedWithManager(!data.data.SharedWithManager());//Toggle manually

            if (data.data.SharedWithManager()) {
                vm.data.DateShared(new Date());
            } else {
                vm.data.DateShared(null);
            }

            vm.statusMessage(vm.getStatusMessage());
            vm.subMessage(vm.getSubMessage());

            vm.update();
            //return true;
        };

        vm.approve = function (data) {
            data.data.Approved(!data.data.Approved());//Toggle manually

            if (data.data.Approved()) {
                vm.data.DateApproved(new Date());
                vm.readOnly = true;
            } else {
                vm.data.DateApproved(null);
            }

            vm.statusMessage(vm.getStatusMessage());
            vm.subMessage(vm.getSubMessage());

            //When associated checkbox is ticked it disappear and data is saved (as per UX doc LINK-247)
            vm.update();
            //vm.toggleView();
            //return true;
        };

        return vm;
    };

    var viewModel = {
        createViewModel: function (params, componentInfo) {
            var vm = oneObjectiveModel(params);
            return vm;
        }
    };

    return { viewModel: viewModel, template: htmlTemplate };
});
