define(["ko", "clipboard"], function (ko, clipboard) {

    var _retrospectiveCode = $("#retrospective-code").text();
    var _loggedUserId = $("#logged-user-id").text();

    $.connection.hub.qs = { "code": _retrospectiveCode };
    $.connection.hub.error(function(error) {
        console.log("The server spitted the following error: " + error);
    });
    
    $.connection.hub.start()
        .done(function () {
            console.log("Connected: " + $.connection.hub.id);
        })
        .fail(function () {
            console.log("Fail to connect");
        });

   

    var model = {
        copyLinkToClipboard: function (button) {
            clipboard(button);
        },

        addPositiveFeedback: function () {
            this.addFeedback({ Type: "Positive", id: _.guid(), Text: "" }, this.positiveFeedbacks);
        },

        addNegativeFeedback: function () {
            this.addFeedback({ Type: "Negative", Id: _.guid(), Text: "" }, this.negativeFeedbacks);
        },
        
        addFeedback: function(feedback, targetFeedbacks) {
            targetFeedbacks.push(feedback);
            
            $.connection.retrospectiveHub.server.addFeedbackContainer([feedback]);
        },
        
        addFeedbackContainer: function (feedbacks) {

            if (feedbacks) {
                      
            }
        },

        removeFeedback: function () {
            var feedbackId = this.id;

            model.positiveFeedbacks.remove(function (item) { return item.id === feedbackId; });
            model.negativeFeedbacks.remove(function (item) { return item.id === feedbackId; });
        },

        addParticipant: function (user) {
            if (!ko.utils.arrayFirst(model.participants(), function(item) { return item.Id == user.Id })) {
                model.participants.push(user);
            }
        },
        
        removeParticipant: function(connectionId) {
            model.participants.remove(function (item) { return item.connectionId == connectionId; });
        },

        participants: ko.observableArray(),
        positiveFeedbacks: ko.observableArray(),
        negativeFeedbacks: ko.observableArray(),
        positiveAnswers: ko.observableArray(),
        negativeAnswers: ko.observableArray()

    };
    
    $.connection.retrospectiveHub.client.userLogin = function(sourceConnectionId, user) {
        model.addParticipant(user);

        if (user.Id != _loggedUserId && $.connection.hub.id != sourceConnectionId) {
            $.connection.retrospectiveHub.server.sendUsername(sourceConnectionId);
        }
    };
    $.connection.retrospectiveHub.client.userLogout = model.removeParticipant;

    return model;
});