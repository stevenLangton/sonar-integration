define(['jquery'], function ($) {
    "use strict";

    var uiDateFormat = "DD/MM/YYYY";
    var serverDateFormat = "YYYYMMDD";

    //Root page of web app
    var siteRoot = '/';

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
            url: url,
            type: verb,
            dataType: "json"
        });

        return $promise;
    };

    //Authorization
    var _canEditAlerts = false,
        _canEditProducts = false,
        _canSeeRanging = false,
        _canSeeReplenishment = false,
        _canEditReplenishments = false,
        _IsReadOnly = false,
        _IsAdmin = false,
        _IsSuperAdmin = false;



    var getUserPermissions = function () {
        var userDetailsUrl = getSiteRoot() + "Home/GetUserDetailsAsJson";

        var $promise = callService('get', userDetailsUrl, {});

        $promise.done(function (data) {
            //Save user permissions for UI use

            _IsAdmin = data.IsAdmin;
            _IsSuperAdmin = data.IsSuperAdmin;

            if (data.IsAdmin || data.IsSuperAdmin) {
                _canEditAlerts = true;
                _canEditProducts = true;
                _canSeeRanging = true;
                _canSeeReplenishment = true;
                _canEditReplenishments = true;
                _IsReadOnly = false;

                return;
            }

            if (data.IsRanging === true) {

                _canSeeRanging = true;

                if (data.IsReadOnly === true) {
                    _canEditAlerts = false;
                    _canEditProducts = false;
                }
                else {
                    _canEditAlerts = true;
                    _canEditProducts = true;
                };

                if (data.IsReadWrite === true) {
                    _canEditAlerts = true;
                    _canEditProducts = true;
                }
                else {
                    _canEditAlerts = false;
                    _canEditProducts = false;
                };
            };

            if (data.IsReplenishment === true) {

                _canSeeReplenishment = true;

                if (data.IsReadOnly === true) {
                    _canEditReplenishments = false;
                }
                else {
                    _canEditReplenishments = true;
                };

                if (data.IsReadWrite === true) {
                    _canEditReplenishments = true;
                }
                else {
                    _canEditReplenishments = false;
                };
            };


        });
    };

    var canEditAlerts = function () {
        return _canEditAlerts;
    };

    var canEditProducts = function () {
        return _canEditProducts;
    };

    var canEditReplenishments = function () {
        return _canEditReplenishments;
    };

    //Some initialisation work
    //getUserPermissions();

    return {
        setSiteRoot: setSiteRoot,
        getSiteRoot: getSiteRoot,
        randomString: randomString,
        canEditAlerts: canEditAlerts,
        canEditProducts: canEditProducts,
        canEditReplenishments: canEditReplenishments,
        getUserPermissions: getUserPermissions,
        uiDateFormat : uiDateFormat,
        serverDateFormat : serverDateFormat
    }
});