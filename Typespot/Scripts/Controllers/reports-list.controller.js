(function () {
    'use strict';

    

    angular
        .module('app')
        .controller('ListReportsController', ListReportsController);

    ListReportsController.$inject = ['$scope', '$http', '$filter', '$modal']; 

    function ListReportsController($scope, $http, $filter, $modal) {
        // TODO: moved to root scope
        $scope.isSuperuser = false;
        //
        // Search model
        $scope.reports = [];
        $scope.model = {
            itemsPerPage: 10,
            currentPage: 1,
            dateFrom: moment().days(-7).toDate(),
            dateTo: moment().toDate(),
            userId: '',
            includeTrashed: false
        };

        // TODO: parse model from storage


        // Trashed toggle
        $scope.toggleTrashed = function () {
            $scope.model.includeTrashed = !$scope.model.includeTrashed;
        };

        
        
        // dates and datepickers
        $scope.datesFormat = 'MM-DD-YYYY';
        
        $scope.dates = {
            dateOptions: {
                formatYear: 'yy',
                startingDay: 1,
            },
            from: {
                open: false,
                format: function(){
                    return moment($scope.model.dateFrom).format($scope.datesFormat);
                },
                toggle: function ($event) {
                    $event.preventDefault();
                    $event.stopPropagation();

                    if ($scope.dates.to.open) {
                        $scope.dates.to.open = false;
                    }

                    this.open = !this.open;
                },
                max: function () {
                    return moment($scope.model.dateTo).add(-1).toDate();
                }
            },
            to: {
                open: false,
                format: function () {
                    return moment(this.value).format($scope.datesFormat);
                },
                toggle: function ($event) {
                    $event.preventDefault();
                    $event.stopPropagation();
                    // Close from
                    if ($scope.dates.from.open){
                        $scope.dates.from.open = false;
                    }
                    
                    this.open = !this.open;
                },
                min: function () {
                    return moment($scope.dates.from).add(1).toDate();
                }
            },
            saveInStorage: function () {
                localStorage.setItem('fromDate', moment($scope.model.dateFrom).format($scope.datesFormat));
                localStorage.setItem('toDate', moment($scope.model.dateTo).format($scope.datesFormat));
            },
            getFromStorage: function () {
                var from = localStorage.getItem('fromDate');
                var to = localStorage.getItem('toDate');
                if (from !== null && to !== null) {
                    var fromMoment = moment(from, $scope.datesFormat);
                    var toMoment = moment(to, $scope.datesFormat);
                    if (fromMoment.isValid() && toMoment.isValid()) {
                        $scope.model.dateFrom = fromMoment.toDate();
                        $scope.model.dateTo = toMoment.toDate();
                    }
                };
            }
        };
        //$scope.dates.getFromStorage();

        //$scope.$watch(function () {
        //    return $scope.model.dateFrom + " " + $scope.model.dateTo;
        //}, function (newValue, oldValue) {
        //    // Dates changed
        //    if (newValue !== oldValue) {
        //        $scope.dates.saveInStorage();
        //    }
        //});
       
        // Users
        $scope.selectedUser = null;
        $scope.selectUser = function ($index) {
            if ($index != null) {
                $scope.model.userId = $scope.users[$index].UserId;
                $scope.selectedUser = $index;
            } else {
                $scope.model.userId = '';
                $scope.selectedUser = null;
            }

        };
        $scope.users = new Array();
        $http.get('/Reports/GetUsers').
            success(function (data) {
                $scope.users = data;
            });
        
        

        // Pagination
        // Static values
        $scope.totalItems = 0;
        $scope.numPages = 0;
        $scope.calculateNumPages = function () {
            return Math.ceil($scope.totalItems / $scope.model.itemsPerPage) || 0;
        };
        $scope.showing = function () {
            if ($scope.totalPages() == 1
                || $scope.totalPages() == ($scope.model.currentPage + 1)) {
                return $scope.totalItems;
            }
            return $scope.model.itemsPerPage;
        };
        
        $scope.search = function () {
            //TODO parse dates, remove time
            //var postData = $scope.model;
            //postData.dateFrom = moment(postData.dateFrom).format('YYYY-MM-DD');
            //postData.dateTo = moment(postData.dateTo).format('YYYY-MM-DD');

            $http.post('/Reports/GetPagedReports', $scope.model).
                    success(function (data) {
                        $scope.reports = [];
                        angular.forEach(data.Reports, function (report) {
                            $scope.reports.push(report);
                        });
                        $scope.totalItems = data.Count;
                        $scope.calculateNumPages();
                    });
        };

        

        //// Init();

        // Watch search model for changes
        $scope.$watch(function () {
            return JSON.stringify($scope.model);
        }, function (newValue, oldValue) {
            if(newValue !== oldValue){
                $scope.search();
                console.log('model changes');
            }
        });

        $scope.search();
    }
})();
