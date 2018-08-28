function ContentController($scope, $routeParams, angularHelper) {
    $scope.answerInput = { value: '', hasError: false };

    $scope.amountOfAnswers = Umbraco.Sys.ServerVariables.pollIt.AmountOfAnswers;

    $scope.sortableOptions = {
        axis: 'y',
        containment: 'parent',
        cursor: 'move',
        items: '> div.control-group',
        tolerance: 'pointer',
        stop: function (ev, ui) {
            angularHelper.safeApply($scope, function () {
                angular.forEach($scope.model.question.answers, function (val, index) {
                    val.index = index;
                });
            });
        }
    };

    $scope.config = {
        datePicker: {
            pickDate: true,
            pickTime: false,
            useSeconds: false,
            useMinutes: false,
            format: "YYYY-MM-DD HH:mm:ss",
            icons: {
                time: "icon-time",
                date: "icon-calendar",
                up: "icon-chevron-up",
                down: "icon-chevron-down",
                today: "icon-locate"
            }
        }
    }

    $scope.startDatePicker = { view: 'datepicker', value: $scope.model.question.startDate, config: $.extend({}, $scope.config.datePicker) };
    $scope.endDatePicker = { view: 'datepicker', value: $scope.model.question.endDate, config: $.extend({}, $scope.config.datePicker) };

    $scope.$watch('startDatePicker', function () {
        if (typeof $scope.startDatePicker !== 'undefined') {
            $scope.model.question.startDate = $scope.startDatePicker.value;
        }
    }, true);

    $scope.$watch('endDatePicker', function () {
        if (typeof $scope.endDatePicker !== 'undefined') {
            $scope.model.question.endDate = $scope.endDatePicker.value;
        }
    }, true);

    $scope.addAnswer = function (event) {
        event.preventDefault();

        if ($scope.answerInput.value) {
            if (!_.find($scope.model.question.answers, function (item) { return item.value === $scope.answerInput.value })) {
                var answer = { value: $scope.answerInput.value, index: $scope.model.question.answers.length };

                $scope.model.question.answers.push(answer);
                $scope.answerInput = { value: '', hasError: false };
            }
        } else {
            $scope.answerInput.hasError = true;
        }
    };

    $scope.updateAnswer = function (answer) {
        if (!_.find($scope.model.question.answers, function (item) { return item.value === answer.value && item.id !== answer.id })) {
            answer.hasError = false;
        } else {
            answer.hasError = true;
        }
    };

    $scope.removeAnswer = function (answer, event) {
        event.preventDefault();

        $scope.model.question.answers = _.reject($scope.model.question.answers, function (x) {
            return x.index === answer.index;
        });
        });
    };
}

angular.module("umbraco").controller("PollIt.ContentController", ContentController);