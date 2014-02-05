define(['ko', 'app/views/retrospective/config'], function (ko) {

    var model = {
        target: '#retrospective',

        positiveFeedbacks: ko.observableArray(),
        negativeFeedbacks: ko.observableArray(),

        addPositiveFeedback: function (id, publish) {
            this.addFeedback({ Type: 'Positive', Id: id || _.guid(), Text: '' }, this.positiveFeedbacks, publish);
        },

        addNegativeFeedback: function (id, publish) {
            this.addFeedback({ Type: 'Negative', Id: id || _.guid(), Text: '' }, this.negativeFeedbacks, publish);
        },

        addFeedback: function (feedback, targetFeedbacks, publish) {
            if (!_.find(targetFeedbacks(), function (item) { return item.Id.toString() == feedback.Id.toString(); })) {
                targetFeedbacks.push(feedback);
            }

            if (publish) {
                sendRequestedFeedbacks();
            }
        },

        removeFeedback: function (id, publish) {
            var feedbackId = id || this.Id.toString();

            model.positiveFeedbacks.remove(function (item) { return item.Id.toString() === feedbackId; });
            model.negativeFeedbacks.remove(function (item) { return item.Id.toString() === feedbackId; });

            if (publish) {
                $.connection.retrospectiveHub.server.removeFeedback(feedbackId);
            }
        },

        saveAnswers: function () {

        }
    };

    function sendRequestedFeedbacks(sourceConnectionId) {
        if ($.connection.hub.id == sourceConnectionId) {
            return;
        }

        var positiveFeedbacks = [];
        var negativeFeedbacks = [];

        $.each(model.positiveFeedbacks(), function () {
            positiveFeedbacks.push(this.Id);
        });

        $.each(model.negativeFeedbacks(), function () {
            negativeFeedbacks.push(this.Id);
        });

        $.connection.retrospectiveHub.server.sendRequestedFeedbacks(sourceConnectionId, positiveFeedbacks, negativeFeedbacks);
    }

    $.connection.retrospectiveHub.client.requestFeedbacks = function (sourceConnectionId) {
        if ($.connection.hub.id == sourceConnectionId) {
            return;
        }

        var positiveFeedbacks = [];
        var negativeFeedbacks = [];

        $.each(model.positiveFeedbacks(), function () {
            positiveFeedbacks.push(this.Id);
        });

        $.each(model.negativeFeedbacks(), function () {
            negativeFeedbacks.push(this.Id);
        });

        $.connection.retrospectiveHub.server.sendRequestedFeedbacks(sourceConnectionId, positiveFeedbacks, negativeFeedbacks);
    };

    $.connection.retrospectiveHub.client.addRequestedFeedbacks = function (positiveFeedbacks, negativeFeedbacks) {
        $.each(positiveFeedbacks, function () {
            model.addPositiveFeedback(this, false);
        });

        $.each(negativeFeedbacks, function () {
            model.addNegativeFeedback(this, false);
        });
    };

    $.connection.retrospectiveHub.client.removeFeedback = function(feedbackId) {
        model.removeFeedback(feedbackId, false);
    };

    return model;
});