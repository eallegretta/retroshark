define(['ko', 'app/views/retrospective/config'], function (ko, config) {

    var model = {
        target: '#participants',

        participants: ko.observableArray()
    };

    $.connection.retrospectiveHub.client.userLogin = function (sourceConnectionId, user) {
        if (!ko.utils.arrayFirst(model.participants(), function (item) { return item.Id.toString() == user.Id.toString(); })) {
            model.participants.push(user);
        }

        if (user.Id != config.userId && $.connection.hub.id != sourceConnectionId) {
            $.connection.retrospectiveHub.server.sendUsername(sourceConnectionId);
        }
    };
    
    $.connection.retrospectiveHub.client.userLogout = function(connectionId) {
        model.participants.remove(function(item) { return item.connectionId == connectionId; });
    };

    return model;
});