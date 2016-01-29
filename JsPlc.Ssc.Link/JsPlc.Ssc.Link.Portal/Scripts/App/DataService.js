define(["jquery", "common"], function ($, common) {
    "use strict";

    var getObjectives = function (colleagueId) {
        var $promise = $.ajax({
            data: {ColleagueId: colleagueId},
            url: "objective/GetObjectives",
            type: "get",
            dataType: "json"
        });

        return $promise;
    };

    var getPdp = function (colleagueId) {
        var $promise = common.callService("get", "pdp/getPdp", { ColleagueId: colleagueId });
        return $promise;
    };

    return {
        getObjectives: getObjectives,
        getPdp: getPdp
    };
});