define(["jquery", "toastr"], function ($, toastr) {
    "use strict";

    var init = function (pdpUpdated) {
        if (pdpUpdated) {
            //toastr to inform successful update
            toastr.info('Your Pdp was successfully updated');
        } else {
            toastr.warning('There was a problem updating your Pdp');
        }
    };

    return {
        init: init
    };
});