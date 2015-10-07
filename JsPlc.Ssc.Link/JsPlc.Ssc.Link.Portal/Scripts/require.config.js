﻿//Must omit extension on right side (in values)
require.config({
    waitSeconds: 200,
    baseUrl: "Scripts",
    paths: {
        "bootstrap": "bootstrap.min",
        "modernizr": "modernizr-2.6.2",
        "jquery": "jquery-1.10.2",
        "jqueryval": "jquery.validate.min",
        "knockout": "knockout-3.3.0.debug",
        "moment": 'moment-with-locales',
        "helpers": "Utils/helpers",
        "linkDatePickers": "utils/linkDatePickers",
        "bootstrap-datepicker": "bootstrap-datepicker"
        "text": "text.min",
        //komoment: 'path/to/komoment', // KoMoment potentially useful
        //"datatables": "DataTables/jquery.dataTables",

        //App modules
        "common": "App/common",

        //"ko-binding-handlers": "App/ko-binding-handlers",

        "RegisterKoComponents": "App/kocomponents/RegisterKoComponents"

    },
    shim: {
        "knockout": { deps: ["jquery"] },
        "bootstrap": { deps: ["jquery"] }
        //"ko-binding-handlers": { deps: ["jquery"] }
        //"tableedit": { deps: ["tabletools"] }
    },
    map: {
        //typeahead: "typeahead-helper!typeahead.bundle"

    }
});

//To load essential modules first
//require(["RegisterKoComponents"]);
//require(['jquery', 'knockout', 'ko-binding-handlers', 'text'], function ($, ko) { });//Module end
require(['jquery', 'knockout'], function ($, ko) { });//Module end
