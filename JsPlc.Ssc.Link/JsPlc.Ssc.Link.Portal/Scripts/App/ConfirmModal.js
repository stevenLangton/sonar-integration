define(["jquery"], function ($) {
    "use strict";
    var dialogHtml = "<div id='confirmModal' class='modal fade'>\
    <div class='modal-dialog'>\
        <div class='modal-content'>\
            <div class='modal-header'>\
                <button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button>\
                <h4 class='modal-title'>Confirmation</h4>\
            </div>\
            <div class='modal-body'>\
                <p>This action cannot be undone and can potentially update many records. Are you very sure you want to proceed?</p>\
            </div>\
            <div class='modal-footer'>\
                <button id='cancel' type='button' class='btn btn-default' data-dismiss='modal'>Cancel</button>\
                <button id='proceed' type='button' class='btn btn-primary'>Proceed</button>\
            </div>\
        </div>\
    </div>\
</div>";

    if (!$("#confirmModal").length) {
        var divElement = document.createElement('div');
        divElement.innerHTML = dialogHtml;
        document.body.appendChild(divElement);
    }

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