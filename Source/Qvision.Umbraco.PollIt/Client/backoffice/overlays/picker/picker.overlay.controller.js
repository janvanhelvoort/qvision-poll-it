function PickerOverlayController($scope, pollItResource) {

	$scope.isLoading = true;
	$scope.content = { questions: [], error: null }

	pollItResource.getQuestions().then(function (result) {
		$scope.content.questions = result.data;
		$scope.isLoading = false;
	}, function (error) {
		$scope.content.error = "An Error has occured while loading!";
		$scope.isLoading = false;
	});

	if (!$scope.model.selection) {
	    $scope.model.selection = [];
	}

	$scope.pickPoll = function (question) {
	    $scope.model.selection.push(question);
		$scope.model.submit($scope.model);
	}
}

angular.module("umbraco").controller("PollIt.PickerOverlayController", PickerOverlayController);