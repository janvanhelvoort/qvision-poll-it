function EditController($scope, $routeParams, $filter, $location, navigationService, editorState, formHelper, localizationService, pollItResource) {
    $scope.page = { loading: false, create: true, tabs: [{ id: 1, label: localizationService.localize("pollIt_tabContent") }, { id: 2, label: localizationService.localize("pollIt_tabResponses") }] };
    $scope.content = { question: {}, answers: [], answer: { value: '', hasError: false }, responses: [] };
    $scope.config = {
        datePicker: {
            pickDate: true,
            pickTime: true,
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
    
    $scope.startDatePicker = { view: 'datepicker', value: null, config: $.extend({}, $scope.config.datePicker) };
    $scope.endDatePicker = { view: 'datepicker', value: null, config: $.extend({}, $scope.config.datePicker) };

    if (!$routeParams.create) {
        $scope.page.isLoading = true;
        $scope.page.create = false;

        pollItResource.getQuestionById($routeParams.id).then(function (result) {
            $scope.content.question = result.data;

            $scope.startDatePicker.value = $scope.content.question.startDate;
            $scope.endDatePicker.value = $scope.content.question.endDate;

            //set a shared state
            editorState.set($scope.content.question);

            $scope.page.isLoading = false;            
        });;

        pollItResource.getQuestionAnswersById($routeParams.id).then(function (result) {
            $scope.content.answers = $filter('orderBy')(result.data, 'index');
        });

        pollItResource.getQuestionResponsesById($routeParams.id).then(function (result) {
            $scope.content.responses = result.data;
        });
    }

    $scope.getResponsesPercentage = function (id) {
        var amount = $filter('filter')($scope.content.responses, { answerId: id }).length;
        return amount > 0 ? Math.round(amount / question.responses.length * 100) : 0;
    }

    $scope.getResponses = function (id) {
        return $filter('filter')($scope.content.responses, { answerId: id }).length;
    }

    $scope.$watch('startDatePicker', function () {
        if ($scope.startDatePicker != undefined) {
            $scope.content.question.startDate = $scope.startDatePicker.value;
        }
    }, true);

    $scope.$watch('endDatePicker', function () {
        if ($scope.endDatePicker != undefined) {
            $scope.content.question.endDate = $scope.endDatePicker.value;
        }
    }, true);

    $scope.save = function () {
        if (formHelper.submitForm({ scope: $scope, statusMessage: "Saving..." })) {
            $scope.page.saveButtonState = "busy";

            pollItResource.saveQuestion($scope.content.question).then(function (result) {
                formHelper.resetForm({ scope: $scope });

                $scope.page.saveButtonState = "success";

                $scope.content.question = result.data;

                //set a shared state
                editorState.set($scope.content.question);

                navigationService.syncTree({ tree: 'poll', path: ['-1', $scope.content.question.id.toString()], forceReload: true, activate: true });

                if ($scope.page.create) {
                    $scope.page.create = false;
                    $location.url("/pollIt/poll/edit/" + $scope.content.question.id);
                }                
            }, function () {
                $scope.page.saveButtonState = "error";
            });;
        }
    };

    $scope.sortableOptions = {
        axis: 'y',
        containment: 'parent',
        cursor: 'move',
        items: '> div.control-group',
        tolerance: 'pointer',
        stop: function () {
            pollItResource.updateSort($scope.content.answers.map(function (answer) { return answer.id }));
        }
    };

    $scope.addAnswer = function (event) {
        event.preventDefault();

        if ($scope.content.answer.value) {
            if (!_.contains($scope.content.answers.value, $scope.content.answer.value)) {
                var answer = { value: $scope.content.answer.value, index: $scope.content.answers.length + 1 };

                pollItResource.postQuestionAnswer($routeParams.id, answer).then(function (result) {
                    $scope.content.answers.push(result.data);
                    $scope.content.answer = { value: '', hasError: false };
                    return;
                });
            }
        }

        $scope.content.answer = { hasError: false };
    };

    $scope.updateAnswer = function (answer, event) {
        event.preventDefault();

        pollItResource.saveAnswer(answer);
    }

    $scope.removeAnswer = function (answer, event) {
        event.preventDefault();

        pollItResource.deleteAnswer(answer.id).then(function () {
            $scope.content.answers = _.reject($scope.content.answers, function (x) {
                return x.id === answer.id;
            });
        });
    };
}

angular.module("umbraco").controller("PollIt.EditController", EditController);