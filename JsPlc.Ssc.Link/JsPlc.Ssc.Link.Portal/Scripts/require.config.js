﻿/// <reference path="App/Services/LinkService.js" />
/// <reference path="App/ViewScripts/LinkFormCreate.js" />
//Must omit extension on right side (in values)
require.config({
    waitSeconds: 200,
    baseUrl: "Scripts",
    paths: {
        "modernizr": "modernizr-2.6.2",
        "jquery": "jquery-1.10.2",
        "jqueryval": "jquery.validate.min",
        "knockout": "knockout-3.3.0.debug",
        "moment": 'moment-with-locales',
        "bootstrap": "bootstrap.min",
        "bootstrap-datepicker": "bootstrap-datepicker",
        "bootstrap-datepickerGB": "locales/bootstrap-datepicker.en-GB.min",
        "URI": "UriJs/Uri",
        "underscore": "underscore",
        //"fragmentQuery": "UriJs/URI.fragmentQuery",
        //"fragmentUri": "UriJs/URI.fragmentUri",

        // LINK Specific js files.
        "helpers": "Utils/helpers",
        "linkDatePickers": "utils/linkDatePickers",
        "text": "text.min",
        //komoment: 'path/to/komoment', // KoMoment potentially useful
        //"datatables": "DataTables/jquery.dataTables",

        //App modules
        "common": "App/common",

        //"ko-binding-handlers": "App/ko-binding-handlers",

        "RegisterKoComponents": "App/kocomponents/RegisterKoComponents",

        //Services
        "LinkService": "App/Services/LinkService",

        //View scripts
        "LinkFormCreate": "App/ViewScripts/LinkFormCreate"

    },
    shim: {
        "bootstrap-datepicker": { deps: ["jquery"] },
        "bootstrap-datepickerGB": { deps: ["jquery", "bootstrap-datepicker"] },
        "knockout": { deps: ["jquery"] },
        "bootstrap": { deps: ["jquery"] }
        //"ko-binding-handlers": { deps: ["jquery"] }
        //"tableedit": { deps: ["tabletools"] }
    },
    map: {
        //typeahead: "typeahead-helper!typeahead.bundle"
        "URI": {
            "IPv6": "URIjs/punycode",
            "punycode": "URIjs/punycode",
            "SecondLevelDomains": "URIjs/punycode"
        }

    }
});

//To load essential modules first
//require(["RegisterKoComponents"]);
//require(['jquery', 'knockout', 'ko-binding-handlers', 'text'], function ($, ko) { });//Module end
require(['jquery', 'knockout'], function ($, ko) { });//Module end
