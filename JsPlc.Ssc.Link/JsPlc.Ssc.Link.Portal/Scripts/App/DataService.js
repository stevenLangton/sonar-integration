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

    var getSharedObjectives = function (colleagueId) {
        var $promise = common.callServerAction("get", "objective/GetSharedObjectives", { ColleagueId: colleagueId });
        return $promise;
    };

    var getOneObjective = function (objectiveId) {
        var $promise = common.callServerAction("get", "objective/GetOneObjective", { ObjectiveId: objectiveId });
        return $promise;
    };

    var getPdp = function (colleagueId) {
        var $promise = common.callServerAction("get", "pdp/getPdp", { ColleagueId: colleagueId });
        return $promise;
    };

    var getColleagueMeetings = function (colleagueId) {
        var $promise = common.callServerAction("get", "Team/GetColleagueMeetings", { ColleagueId: colleagueId });
        return $promise;
    };

    return {
        getColleagueMeetings: getColleagueMeetings,
        getPdp: getPdp,
        getObjectives: getObjectives,
        getSharedObjectives: getSharedObjectives,
        getOneObjective: getOneObjective
    };
});