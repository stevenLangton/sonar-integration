define(['jquery'], function ($) {
    "use strict";

    var uiDateFormat = "DD/MM/YYYY";
    var serverDateFormat = "YYYYMMDD";
    var siteRoot = $('base').attr('href');
    var userInfo = {};
    userInfo.colleagueId = "";

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

    function splitArray(a, size) {
        var len = a.length, out = [], i = 0;
        while (i < len) {
            out.push(a.slice(i, i += size));
        }
        return out;
    }

    //Set from server
    var setUserInfo = function (data) {
        userInfo.colleagueId = "";
        userInfo.managerId = "";
        userInfo.managerName = "";

        if (typeof data !== "undefined" && !$.isEmptyObject(data)) {
            userInfo.colleagueId = data.Colleague.ColleagueId;
            userInfo.managerId = data.Colleague.ManagerId;
            if (data.Colleague.Manager) {
                userInfo.managerName = data.Colleague.Manager.FirstName + " " + data.Colleague.Manager.LastName;
            } 
        }
    };

    var getUserInfo = function () {
        return userInfo;
    };

    return {
        splitArray: splitArray,
        setSiteRoot: setSiteRoot,
        getSiteRoot: getSiteRoot,
        randomString: randomString,
        uiDateFormat: uiDateFormat,
        serverDateFormat: serverDateFormat,
        callServerAction: callService,
        setUserInfo: setUserInfo,
        getUserInfo: getUserInfo
    };
});



