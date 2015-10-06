//Must omit extension on right side (in values)
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
        "helpers": "Utils/helpers"
        //komoment: 'path/to/komoment', // KoMoment potentially useful
        //"datatables": "DataTables/jquery.dataTables",

        //App modules
        //"common": "App/common",

        //"alertsTable": "App/AlertsTable",
        //"productsTable": "App/_ProductsTable",

        //"alertsList": "App/_AlertsList",
        //"pageAlerts": "App/_PageAlerts",
        //"alertEditDialog": "App/_AlertEditDialog",
        //"text": "text.min",
        //"ko-binding-handlers": "App/ko-binding-handlers",

        //"alertService": "App/alertService",
        //"productsService": "App/productsService",
        //"dataRoleService": "App/dataRoleService",
        //"replenishmentService": "App/replenishmentService",

        //"dataRoleNameList": "App/_DataRoleNameList",

        //"replenishmentTable": "App/_ReplenishmentTable",
        //"tabletools": "DataTables/dataTables.tableTools.min",
        //"tableedit": "Editor-1.5.0/js/dataTables.editor",

        //"tablebuttons": "DataTables/dataTables.buttons.min",
        //"tableselect": "DataTables/dataTables.select.min",

        //"jszip": "jszip.min",
        //"pdfmake": "pdfmake.min",
        //"vfsfont": "vfs_fonts",
        //"buttonhtml5": "buttons.html5.min",

        //"moment": "moment.min",
        //"datepicker": "bootstrap-datetimepicker.min",

        //"dtBoot": "DataTables/dataTables.bootstrap",
        //"validator": "validator.min",
        //"RegisterKoComponents": "kocomponents/RegisterKoComponents"

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
