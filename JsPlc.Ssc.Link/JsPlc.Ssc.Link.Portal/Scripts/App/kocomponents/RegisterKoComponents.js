define(['jquery', 'knockout', 'text'], function ($, ko) {
    ko.oneTimeDirtyFlag = function (root) {
        var _initialized;

        //one-time dirty flag that gives up its dependencies on first change
        var result = ko.computed(function () {
            if (!_initialized) {
                //just for subscriptions
                ko.toJS(root);

                //next time return true and avoid ko.toJS
                _initialized = true;

                //on initialization this flag is not dirty
                return false;
            }

            //on subsequent changes, flag is now dirty
            return true;
        });

        return result;
    };
});//Module end