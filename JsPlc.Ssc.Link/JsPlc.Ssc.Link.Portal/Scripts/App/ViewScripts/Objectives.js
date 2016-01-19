define(["jquery", "knockout", "LinkService"], function ($, ko, LinkService) {
    "use strict";

    //View model
    //var init = $.noop;
    var init = function () {
        //var $promise = LinkService.getAllColleagueObjectives("E0009");
        var jsonArgs = { ColleagueId: "E0009" };

        var $promise = $.ajax({
            data: jsonArgs,
            url: "objective/GetAllColleagueObjectives",
            type: "get",
            dataType: "json"
        });

        $promise.done(function (result) {
            var result = result;
        });

        $promise.error(function (request, status, error) {
            alert(request.responseText);
        });
    };

    return {
        init: init
    };
});