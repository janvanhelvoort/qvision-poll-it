function DeleteController($scope, $location, navigationService, treeService, editorState, dialogService, notificationsService, pollItResource) {
    $scope.performDelete = function () {
        // stop from firing again on double-click
        if ($scope.busy) {
            return false;
        }

        //mark it for deletion (used in the UI)
        $scope.currentNode.loading = true;
        $scope.busy = true;

        pollItResource.deleteQuestion($scope.currentNode.id).then(function () {
            $scope.currentNode.loading = false;

            treeService.removeNode($scope.currentNode);

            //if the current edited item is the same one as we're deleting, we need to navigate elsewhere
            if (editorState.current && editorState.current.id.toString() === $scope.currentNode.id) {

                //If the deleted item lived at the root then just redirect back to the root, otherwise redirect to the item's parent
                var location = "/pollIt";
                if ($scope.currentNode.parentId.toString() !== "-1") {
                    location = "/pollIt/poll/edit/" + $scope.currentNode.parentId;
                }

                $location.path(location);
            }

            navigationService.hideMenu();
        }, function (err) {
            $scope.currentNode.loading = false;
            $scope.busy = false;

            //check if response is ysod
            if (err.status && err.status >= 500) {
                dialogService.ysodDialog(err);
            }

            if (err.data && angular.isArray(err.data.notifications)) {
                for (var i = 0; i < err.data.notifications.length; i++) {
                    notificationsService.showNotification(err.data.notifications[i]);
                }
            };
        });

        return true;
    };

    $scope.cancel = function () {
        navigationService.hideDialog();
    };
}

angular.module("umbraco").controller("PollIt.DeleteController", DeleteController);