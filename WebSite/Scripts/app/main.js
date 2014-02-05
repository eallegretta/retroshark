require.config({
    baseUrl: '/Scripts',

    paths: {
        "jquery": "jquery-2.0.3.min",
        "bootstrap": "bootstrap.min",
        "ko": "knockout-3.0.0",
        "signalr": "jquery.signalR-1.2.0.min",
        "signalr-hubs": "/signalr/hubs?_",
        "json": "json2.min",
        "underscore": "underscore-extensions"
    },

    shim: {
        "jquery": { exports: '$' },
        "bootstrap": ["jquery"],
        "ko": { exports: "ko", deps: ["jquery", "underscore"] },
        "clipboard": ["jquery"],
        "signalr": ["jquery"],
        "signalr-hubs": ["signalr"],
        "json": { exports: "JSON" },
        "underscore": { exports: "_" },
    }
});

define("config", function () {
    return globalConfig;
});

require(["ko", "signalr-hubs", "bootstrap", "app/validation-summary"], function (ko) {
    $(document).ready(function () {

        var viewModels = $("[data-view-model]");

        var loadCounter = 0;

        $(viewModels).each(function () {
            var filename = $(this).text();

            require(["app" + filename], function (viewModel) {

                if (viewModel.target) {
                    ko.applyBindings(viewModel, $(viewModel.target)[0]);
                } else {
                    ko.applyBindings(viewModel);
                }

                loadCounter++;
                
                if (loadCounter == viewModels.length) {
                    $(document).trigger("viewModelsReady");
                }
            });
        });
    });
});