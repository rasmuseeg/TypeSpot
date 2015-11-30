(function () {
    'use strict';

    angular
        .module('app')
        .controller('UsersController', usersController);

    usersController.$inject = ['$scope', '$http', '$interval'];

    function usersController($scope, $http, $interval) {
        var stop;
        var between = 10000;
        $scope.time = between;
        var timer;
        $scope.label = null;
        $scope.updated = null;
        $scope.auto = false;
        $scope.users = {};
        $scope.activeUsers = true;
        $scope.refresh = function () {
            $http.get('/Users/GetAll').
                success(function (data) {

                    angular.forEach(data, function(user, key){
                        $scope.users[key] = user;
                        // Parse dates
                        var date = moment(user.LastEntry);
                        if (date.isValid()) {
                            $scope.users[key].LastEntry = date.toDate();
                        }
                    });

                    $scope.updated = moment().toDate();
                });
        };
        $scope.refresh();

        $scope.update = function () {
            // Don't start a new fight if we are already fighting
            if (angular.isDefined(stop)) return;

            $scope.auto = true;
            stop = $interval(function () {
                if ($scope.time > 0) {
                    $scope.time = $scope.time - 1000;
                }
                 // Update and reset
                else{
                    $scope.refresh();
                    $scope.time = between;
                }
            }, 1000);
        };

        $scope.startTimer = function () {
            var timer = $interval(function () {
                if ($scope.auto) {
                    if ($scope.time > 0) {
                        $scope.label = $scope.time / 1000 + ' sekunder.';
                    }
                } else {
                    $scope.label = moment($scope.updated).fromNow();
                }
            }, 1000);
        };
        $scope.startTimer();

        $scope.stopUpdate = function () {
            if (angular.isDefined(stop)) {
                $interval.cancel(stop);
                stop = undefined;
                $scope.auto = false;
            }
        };

        $scope.toggleAuto = function () {
            if (angular.isDefined(stop)) {
                $scope.stopUpdate();
            } else {
                $scope.refresh();
                $scope.update();
            }
        };
    }
})();
