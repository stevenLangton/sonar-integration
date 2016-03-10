define(["jquery", "knockout", "komap", "moment", "common", "toastr", "text!App/kocomponents/oneObjective.html", "dataService", "confirmModal", "autogrow", "RegisterKoComponents"], function ($, ko, komap, moment, common, toastr, htmlTemplate, dataService, confirmModal, autogrow) {
    "use strict";
    var getDateStr = function (jsonDate) {
        var moDate = moment(jsonDate);
        if (moDate.isValid()) {
            return moDate.toISOString();
        } else {
            return "";
        }
    };

    var oneObjectiveModel = function (params) {
        var vm = {};
        vm.data = komap.fromJS(params.data);//params.data is JSON form of a LinkObjective server object
        vm.readOnly = params.readOnly !== undefined ? params.readOnly : true;
        vm.expandedView = ko.observable(false);

        vm.dirtyFlag = ko.oneTimeDirtyFlag(vm.data);
        //vm.displayCreateDate = moment(vm.data.CreatedDate()).format('LLLL');
        //vm.displayUpdateDate = moment(vm.data.LastAmendedDate()).format('LLLL');

        vm.update = function () {
            var jsonArgs = komap.toJS(vm.data);
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
                    if (mvcAction === mvcCreateAction) {
                        toastr.info("You have successfully created a new objective");
                    } else {
                        toastr.info("Your objective has been updated");
                    }

                    //window.location.href = common.getSiteRoot() + "Objective";
                } else {
                    toastr.error("We encountered a problem while processing your request.");
                }
            });


            $promise.error(function (request, status, error) {
                window.alert(request.responseText);
            });
        };

        vm.cancel = function () {

            if (vm.dirtyFlag()) {
                var dialogOptions = {};

                var yesHandler = function () {
                    //window.location.href = common.getSiteRoot() + "Objective";
                    //window.history.go(-1);
                    //Restore the data as user cancels modifications
                    var $promise = dataService.getOneObjective(vm.data.Id());
                    $promise.done(function (data) {
                        komap.fromJS(data, vm.data);
                        vm.dirtyFlag = ko.oneTimeDirtyFlag(vm.data);
                    });
                };

                if (vm.data.Id() === 0) {
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
                window.history.go(-1);
            }

        };

        vm.toggleView = function (data, event) {
            var $childIcon = $(event.currentTarget).children("icon");
            if ($childIcon.hasClass("fa-plus")) {
                //Open expanded view
                $childIcon.removeClass("fa-plus");
                $childIcon.addClass("fa-minus");
                vm.expandedView(true);
            } else {
                //Open collapsed view
                $childIcon.removeClass("fa-minus");
                $childIcon.addClass("fa-plus");
                vm.expandedView(false);
            }
        };

        //ko.applyBindings(vm, document.getElementById('ObjectiveView'));
        //$('textarea').autogrow({ onInitialize: true });

        return vm;
    };

    var viewModel = {
        createViewModel: function (params, componentInfo) {
            var vm = oneObjectiveModel(params);

            //$(componentInfo.element).on('show', function (event, tabNo) {
            //    $("#showTab" + tabNo).trigger("click");
            //});

            return vm;
        }
    };

    return {viewModel: viewModel, template: htmlTemplate};
});