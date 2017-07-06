function PollItResource($http) {
    return {
        // Overview
        getOverview: function () {
            return $http.get(Umbraco.Sys.ServerVariables.pollIt.getOverview);
        },

        // Questions
        getQuestions: function () {
            return $http.get(Umbraco.Sys.ServerVariables.pollIt.getQuestions);
        },
        getQuestionById: function (id) {
            return $http.get(Umbraco.Sys.ServerVariables.pollIt.getQuestionById + "?id=" + id);
        },
        getQuestionAnswersById: function (id) {
            return $http.get(Umbraco.Sys.ServerVariables.pollIt.getQuestionAnswersById + "?id=" + id);
        },
        getQuestionResponsesById: function (id) {
            return $http.get(Umbraco.Sys.ServerVariables.pollIt.getQuestionResponsesById + "?id=" + id);
        },
        saveQuestion: function (question) {
            return $http.post(Umbraco.Sys.ServerVariables.pollIt.saveQuestion, question);
        },
        postQuestionAnswer: function (id, question) {
            return $http.post(Umbraco.Sys.ServerVariables.pollIt.postQuestionAnswer + "?id=" + id, question);
        },
        deleteQuestion: function (id) {
            return $http.delete(Umbraco.Sys.ServerVariables.pollIt.deleteQuestion + "?id=" + id);
        },

        // Answers
        saveAnswer: function (answer) {
            return $http.post(Umbraco.Sys.ServerVariables.pollIt.saveAnswer, answer);
        },
        updateSort: function (ids) {
            return $http.post(Umbraco.Sys.ServerVariables.pollIt.updateSort, ids);
        },
        deleteAnswer: function (id) {
            return $http.delete(Umbraco.Sys.ServerVariables.pollIt.deleteAnswer + "?id=" + id);
        }
    };
};

angular.module("umbraco.resources").factory("pollItResource", PollItResource)