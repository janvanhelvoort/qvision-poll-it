function PickerController($scope, pollItResource) {
    $scope.content = { selectedPoll: null, error: null }

    if ($scope.model.value) {
        pollItResource.getQuestionById($scope.model.value).then(function(result) {
            $scope.content.selectedPoll = result.data;            
        }, function(error) {
            var currentValue = angular.copy($scope.model.value);
            $scope.content.error = "The saved/picked poll with id '" + currentValue + "' no longer exists. Pick another poll below or clear out the old saved poll";
        });
    }

    $scope.openPollPicker = function () {
        if (!$scope.content.pollPickerOverlay) {
            $scope.content.pollPickerOverlay = {
                view: "../App_Plugins/pollit/backoffice/propertyeditors/picker.overlay.html",
                show: true,
                submit: function (model) {
                    if (model.selectedPolls && model.selectedPolls.length > 0) {
                        $scope.content.selectedPoll = model.selectedPolls[0];
                        $scope.model.value = model.selectedPolls[0].id;
                    }

                    $scope.content.pollPickerOverlay.show = false;
                    $scope.content.pollPickerOverlay = null;
                },
                close: function () {
                    $scope.content.pollPickerOverlay.show = false;
                    $scope.content.pollPickerOverlay = null;
                }
            }
        }
    }

    $scope.remove = function () {
        $scope.content.selectedPoll = null;
        $scope.model.value = null;
    }
}

angular.module("umbraco").controller("PollIt.PickerController", PickerController);