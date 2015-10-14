define(['jquery', 'knockout', 'text'], function ($, ko) {
    //knockout component registrations
    ko.components.register("meeting-history",
    {
        require: "App/kocomponents/meeting-history"
    });
    //ko.components.register("location-heirarchy",
    //{
    //    require: "kocomponents/locationHierarchy"
    //});

    //ko.components.register("product-level",
    //{
    //    require: "kocomponents/productLevel"
    //});

    //ko.components.register("product-heirarchy",
    //{
    //    require: "kocomponents/productHierarchy"
    //});

    //ko.components.register("store-grades",
    //{
    //    require: "kocomponents/storeGrades"
    //});

    //ko.components.register("alerts-filter",
    //{
    //    require: "kocomponents/alertsFilter"
    //});

    //ko.components.register("products-filter",
    //{
    //    require: "kocomponents/productsFilter"
    //});

    //ko.components.register("login-panel",
    //{
    //    require: "kocomponents/loginPanel"
    //});

});//Module end