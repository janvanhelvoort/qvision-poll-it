function PickerController($scope, pollItResource, localizationService) {
    $scope.content = { selection: null, error: null }

    if ($scope.model.value) {
        pollItResource.getQuestionById($scope.model.value).then(function(result) {
            $scope.content.selection = result.data;
        }, function(error) {
            var currentValue = angular.copy($scope.model.value);
            $scope.content.error = "The saved/picked poll with id '" + currentValue + "' no longer exists. Pick another poll";
        });
    }

    $scope.openPollPicker = function () {
        if (!$scope.content.pollPickerOverlay) {
            $scope.content.pollPickerOverlay = {
                view: "../App_Plugins/pollit/backoffice/overlays/picker/picker.overlay.html",
                title: localizationService.localize("pollIt_selectQuestions"),
                show: true,
                hideSubmitButton: true,
                submit: function (model) {
                    if (model.selection && model.selection.length > 0) {
                        $scope.content.selection = model.selection[0];
                        $scope.model.value = model.selection[0].id;
                    }

                    $scope.content.pollPickerOverlay.show = false;
                    $scope.content.pollPickerOverlay = null;
                    $scope.content.error = null;
                },
                close: function () {
                    $scope.content.pollPickerOverlay.show = false;
                    $scope.content.pollPickerOverlay = null;
                    $scope.content.error = null;
                }
            }
        }
    }

    $scope.remove = function () {
        $scope.content.selection = null;
        $scope.model.value = null;
    }
}

angular.module("umbraco").controller("PollIt.PickerController", PickerController);