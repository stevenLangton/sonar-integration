define(["jquery"], function ($) {
    "use strict";

    var $container = $("#confirmModal");

    var defaultOptions = {
        noHandler: $.noop,
        yesHandler: $.noop,
        titleText: "Action Confirmation",
        bodyText: "Changes have been made. Are you sure you wish to discard them?",
        noButtonText: "No",
        yesButtonText: "Yes"
    };

    var optionsUsed = defaultOptions;

    var show = function () {
        $container.modal('show');
    };

    var setHandler = function (handler) {
        return (typeof handler === "function")
            ? handler
            : $.noop;
    };

    var setYesHandler = function (clientYesHandler) {
        optionsUsed.yesHandler = setHandler(clientYesHandler);
    };

    var setNoHandler = function (clientNoHandler) {
        optionsUsed.noHandler = setHandler(clientNoHandler);
    };

    var init = function (options) {
        optionsUsed = $.extend({}, defaultOptions, options || {});

        if (optionsUsed.titleText) {
            $("#confirmModal .modal-title").text(optionsUsed.titleText);
        }

        if (optionsUsed.bodyText) {
            $("#confirmModal .modal-body p").text(optionsUsed.bodyText);
        }

        if (optionsUsed.noButtonText) {
            $("#confirmModal #cancel").text(optionsUsed.noButtonText);
        }

        if (optionsUsed.yesButtonText) {
            $("#confirmModal #proceed").text(optionsUsed.yesButtonText);
        }

        setYesHandler(optionsUsed.yesHandler);
        setNoHandler(optionsUsed.noHandler);
    };

    //Initialisation
    $("#confirmModal button#proceed").on('click', function () {
        $container.modal('hide');
        optionsUsed.yesHandler();
    });

    $("#confirmModal button#cancel").on('click', function () {
        optionsUsed.noHandler();
    });

    //Public interface
    return {
        show: show,
        init: init,
        setYesHandler: setYesHandler,
        setNoHandler: setNoHandler
    };
});