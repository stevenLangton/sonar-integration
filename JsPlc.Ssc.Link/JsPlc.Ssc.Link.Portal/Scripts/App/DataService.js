define(["jquery"], function ($) {
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

    return {
        getObjectives: getObjectives
    };
});