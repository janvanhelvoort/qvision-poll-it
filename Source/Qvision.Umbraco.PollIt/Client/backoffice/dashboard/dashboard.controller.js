function DashboardController($scope, $location, $filter, pollItResource) {
    $scope.page = { loading: false };
    $scope.content = { questions: [] };

    pollItResource.getOverview().then(function (result) {
        $scope.content.questions = result.data;
        $scope.page.isLoading = false;
    });;

    $scope.navigate = function (id) {
        var location = "/pollIt/poll/edit/" + id;
        if (id === "-1") {
            location += ('?create');
        }

        $location.url(location);
    }

    $scope.getResponsesPercentage = function (question, id) {
        var amount = $filter('filter')(question.responses, { answerId: id }).length;
        return amount > 0 ? Math.round(amount / question.responses.length * 100) : 0;
    }

    $scope.getResponses = function (question, id) {
        return $filter('filter')(question.responses, { answerId: id }).length;
    }
}

angular.module("umbraco").controller("PollIt.DashboardController", DashboardController);