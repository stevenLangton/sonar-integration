define(['jquery'], function ($) {
    "use strict";

    var uiDateFormat = "DD/MM/YYYY";
    var serverDateFormat = "YYYYMMDD";

    //Root page of web app
    var siteRoot = '/';

    siteRoot = $('base').attr('href');

    var getSiteRoot = function () {
        return siteRoot;
    };

    var setSiteRoot = function (pathString) {
        siteRoot = pathString;
    };

    var randomString = function () {
        return Math.random().toString(36).substring(7);
    };

    var callService = function (verb, url, jsonArgs) {
        var $promise = $.ajax({
            data: jsonArgs,
            url: siteRoot + url,
            type: verb,
            dataType: "json"
        });

        return $promise;
    };

    return {
        setSiteRoot: setSiteRoot,
        getSiteRoot: getSiteRoot,
        randomString: randomString,
        uiDateFormat: uiDateFormat,
        serverDateFormat: serverDateFormat,
        callService: callService,
        callServerAction: callService
    };
});



