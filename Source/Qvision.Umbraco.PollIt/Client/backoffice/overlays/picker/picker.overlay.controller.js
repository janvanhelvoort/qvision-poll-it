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

	if (!$scope.model.title) {
		$scope.model.title = "Select a poll";
	}

	if (!$scope.model.multiPicker) {
		$scope.model.hideSubmitButton = true;
	}

	if (!$scope.model.selectedPolls) {
		$scope.model.selectedPolls = [];
	}

	$scope.pickPoll = function (question) {
		if (question.selected) {
			question.selected = false;

			angular.forEach($scope.model.selectedPolls, function (selectedPoll, index) {
				if (selectedPoll.id === question.id) {
					$scope.model.selectedPolls.splice(index, 1);
				}
			});
		} else {
			question.selected = true;

			$scope.model.selectedPolls.push(question);

			if (!$scope.model.multiPicker) {
				$scope.model.submit($scope.model);
			}
		}
	}
}

angular.module("umbraco").controller("PollIt.PickerOverlayController", PickerOverlayController);