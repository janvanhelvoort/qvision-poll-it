﻿function EditController($scope, $routeParams, $filter, $location, notificationsService, navigationService, editorState, formHelper, localizationService, pollItResource) {
    $scope.page = { isLoading: false };
    $scope.model = { question: { answers: [] } };

    $scope.page.navigation = [
        {
            "name": localizationService.localize("pollIt_tabContent"),
            "icon": "icon-document",
            "view": "/app_plugins/pollit/backoffice/poll/subviews/content/content.html",
            "active": true
        }, {
            "name": localizationService.localize("pollIt_tabResponses"),
            "icon": "icon-poll",
            "view": "/app_plugins/pollit/backoffice/poll/subviews/responses/responses.html"
        }];


    if (!$routeParams.create) {
        $scope.page.isLoading = true;

        pollItResource.getQuestionById($routeParams.id).then(function (result) {
            $scope.model.question = result.data;

            //set a shared state
            editorState.set($scope.model.question);

            pollItResource.getQuestionAnswersById($routeParams.id).then(function (result) {
                $scope.model.question.answers = $filter('orderBy')(result.data, 'index');
                $scope.page.isLoading = false;
            }, function (result) {
                notificationsService.error(result.data.message);
            });
        }, function (result) {
            notificationsService.error(result.data.message);
        });
    }

    $scope.save = function () {
        if (formHelper.submitForm({ scope: $scope, statusMessage: "Saving..." })) {
            $scope.page.saveButtonState = "busy";

            if ($scope.model.question.startDate && $scope.model.question.endDate) {
                if (new Date($scope.model.question.startDate) >= new Date($scope.model.question.endDate)) {
                    notificationsService.error(localizationService.localize("pollIt_startDateBeforeEndDate"));
                    $scope.page.saveButtonState = "error";

                    return;
                }
            }

            pollItResource.saveQuestion($scope.model.question).then(function (result) {
                formHelper.resetForm({ scope: $scope });

                $scope.page.saveButtonState = "success";

                $scope.model.question = result.data;

                //set a shared state
                editorState.set($scope.model.question);

                navigationService.syncTree({ tree: 'poll', path: ['-1', $scope.model.question.id.toString()], forceReload: true, activate: true });

                if ($routeParams.create) {
                    $location.url("/pollIt/poll/edit/" + $scope.model.question.id);
                }
            }, function (result) {
                notificationsService.error(result.data.message);
                $scope.page.saveButtonState = "error";
            });
        }
    };
}

angular.module("umbraco").controller("PollIt.EditController", EditController);