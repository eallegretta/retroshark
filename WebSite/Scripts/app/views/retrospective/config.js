;
define(['jquery'], function ($) {
    var config = {
        code: $("#retrospective-code").text(),
        userId: $("#logged-user-id").text()
    };

    $(document).on("viewModelsReady", function() {
        $.connection.hub.logging = true;
        $.connection.hub.qs = { 'code': config.code };
        $.connection.hub.error(function(error) {
            console.log('The server spitted the following error: ' + error);
        });

        $.connection.hub.start()
            .done(function() {
                console.log('Connected: ' + $.connection.hub.id);
            })
            .fail(function() {
                console.log('Fail to connect');
            });
    });
    
    return config;
});