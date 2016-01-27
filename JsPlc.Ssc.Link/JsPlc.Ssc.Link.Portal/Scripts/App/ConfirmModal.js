/// <reference path="_ConfirmModal.js" />
define(["jquery"], function ($) {
    "use strict";

    var $container = $("#confirmModal"),
        onCancelHandler = $.noop,
        onProceedHandler = $.noop;

    var show = function () {
        $container.modal('show');
    };

    var setHandler = function (handler) {
        return (typeof handler === "function") ? handler : $.noop;
    };

    var setProceedHandler = function (clientOnProceedHandler) {
        onProceedHandler = setHandler(clientOnProceedHandler);
    };

    var setCancelHandler = function (clientOnCancelHandler) {
        onCancelHandler = setHandler(clientOnCancelHandler);
    };

    var init = function (txtTitle, txtBody, txtNoButton, txtYesButton, clientOnCancelHandler, clientOnProceedHandler) {
        if (txtTitle) {
            $("#confirmModal .modal-title").text(txtTitle);
        }

        if (txtBody) {
            $("#confirmModal .modal-body p").text(txtBody);
        }

        if (txtNoButton) {
            $("#confirmModal #cancel").text(txtNoButton);
        }

        if (txtYesButton) {
            $("#confirmModal #proceed").text(txtYesButton);
        }

        setProceedHandler(clientOnProceedHandler);
        setCancelHandler(clientOnCancelHandler);
    };

    //Initialisation
    $("#confirmModal button#proceed").on('click', function () {
        $container.modal('hide');
        onProceedHandler();
    });

    $("#confirmModal button#cancel").on('click', function () {
        onCancelHandler();
    });

    //Public interface
    return {
        show: show,
        init: init,
        setProceedHandler: setProceedHandler,
        setCancelHandler: setCancelHandler
    };
});