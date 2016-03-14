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

    var getOneObjective = function (objectiveId) {
        var $promise = common.callService("get", "objective/GetOneObjective", { ObjectiveId: objectiveId });
        return $promise;
    };

    var getPdp = function (colleagueId) {
        var $promise = common.callService("get", "pdp/getPdp", { ColleagueId: colleagueId });
        return $promise;
    };

    var getColleagueMeetings = function (colleagueId) {
        var $promise = common.callService("get", "Team/GetColleagueMeetings", { ColleagueId: colleagueId });
        return $promise;
    };

    return {
        getColleagueMeetings: getColleagueMeetings,
        getPdp: getPdp,
        getObjectives: getObjectives,
        getOneObjective: getOneObjective
    };
});