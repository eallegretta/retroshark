require.config({
    baseUrl: '/Scripts',
    
    paths: {
        "jquery": "jquery-1.9.1.min",
        "bootstrap": "bootstrap.min",
        "ZeroClipboard": "ZeroClipboard.min",
        "ko": "knockout-2.3.0",
        "signalr": "jquery.signalR-1.1.3.min",
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

define("config", function() {
    return globalConfig;
});

require(["ko", "signalr-hubs", "bootstrap", "app/validation-summary"], function (ko) {
    $(document).ready(function() {
        $("[data-view-module]").each(function() {
            var filename = $(this).text();

            require(["app" + filename], function(viewModule) {
                ko.applyBindings(viewModule);
            });
        });
    });
});