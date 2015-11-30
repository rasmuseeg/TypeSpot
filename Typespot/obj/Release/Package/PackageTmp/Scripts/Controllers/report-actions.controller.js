(function () {
    'use strict';

    angular
        .module('app')
        .controller('ReportActionsController', ReportActionsController);

    ReportActionsController.$inject = ['$scope', '$http', '$modal'];

    function ReportActionsController($scope, $http, $modal) {
        // Delete
        // inherits $scope.report
        $scope.report = $scope.report;

        $scope.trash = function () {
            if ($scope.report !== null) {
                if (confirm("Sikker på du vil slette?")) {
                    $http.post('/Reports/Trash', { id: $scope.report.Id }).
                        success(function () {
                            $scope.report.Trashed = true;
                        });
                }
            }
        };

        // Restore trashed item
        $scope.restore = function ($id, $index) {
            $http.post('/Reports/Restore', { id: $id }).
                success(function () {
                    $scope.reports[$index].Trashed = false;
                });
        };

        // Remove permantly
        $scope.delete = function ($id, $index) {
            if (confirm("Sikker på du vil slette permanent?")) {
                $http.post('/Reports/Delete', { id: $id }).
                    success(function () {
                        $scope.reports.splice($index, 1);
                    });
            }
        };

        // Initialize Edit Modal Controller

        $scope.reportIndex = null;

        $scope.edit = function ($id, $index) {
            $scope.editable = $scope.reports[$index];
            $scope.reportIndex = $index;

            var editModal = $modal.open({
                templateUrl: '/Scripts/Views/Modals/EditReport.html',
                controller: 'EditReportModalController',
                size: null,
                resolve: {
                    report: function () {
                        return $scope.reports[$index];
                    }
                }
            });

            editModal.result.then(function (modifiedReport) {
                $scope.reports[$scope.editableIndex] = modifiedReport;
                $scope.editable = {};
            }, function () {
                //$log.info('Modal dismissed at: ' + new Date());
            });
        };
    }
})();

(function () {
    'use strict';

    angular
        .module('app')
        .controller('EditReportModalController', function ($scope, $http, $modalInstance, report) {
            $scope.title = 'Edit report';
            $scope.report = report;
            $scope.personalities = [];
            $scope.modified = false;
            var copy = angular.copy($scope.report);

            $scope.$watch(function () {
                return $scope.report.Customer + $scope.report.Message + $scope.report.PersonalityId;
            }, function (newValue, oldValue) {
                if (newValue !== oldValue && !$scope.modified) {
                    $scope.modified = true;
                }
            });

            $http.get('/Personalities/GetSelectList', { cache: true }).
                success(function (data, status, headers, config) {
                    $scope.personalities = data;
                }).
                error(function (data, status, headers, config) {
                    // Error
                });

            $scope.personalityChange = function () {
                report.PersonalityId = report.Personality.Id;
            };

            $scope.ok = function () {
                $scope.report.UpdateDate = moment().toDate();
                $scope.report.CreateDate = moment($scope.report.CreateDate).toDate();
                var postData = angular.copy($scope.report);
                $http.post('/Reports/Edit', postData).
                    success(function (data) {
                        //$scope.reports[$scope.editableIndex] = data;
                    });

                $modalInstance.close($scope.report);
            };

            $scope.cancel = function () {
                // Reset
                report.Message = copy.Message;
                report.Personality = copy.Personality;
                report.PersonalityId = copy.PersonalityId;
                report.Customer = copy.Customer;

                // Cancel model
                $modalInstance.dismiss('cancel');
            };
        });
})();
