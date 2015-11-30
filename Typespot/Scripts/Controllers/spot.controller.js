(function () {
    'use strict';

    angular
        .module('app')
        .controller('SpotController', SpotController);

    SpotController.$inject = ['$scope', '$http', '$filter', '$interval']; 

    function SpotController($scope, $http, $filter, $interval) {
        $scope.title = 'SpotController';
        $scope.model = {};
        $scope.personalities = [];
        $scope.spotTonality = false;
        var Report = function () {
            this.Id = 0;
            this.PersonalityId = 0;
            this.Message = '';
            this.Customer = '';
            this.Personality = {};
        };
        $scope.report = new Report();
        $scope.reports = new Array();

        $scope.groups = [];
        // Get report from storage
        var names = {
            'Center': 'Centers',
            'HarmonicGroup': 'HarmonicGroups',
            'SocialStyle': 'SocialStyles',
            'Tonality': 'Tonalities'
        };

        var Strategy = function (Id, $index) {
            this.Id = Id || null;
            this.$index = $index || null;
        };

        var SelectedViewModel = function (Center, hg, ss, t) {
            this.Center;
            this.HarmonicGroup;
            this.SocialStyle;
            this.Tonality;
        };

        $scope.selected = new SelectedViewModel(null, null, null, null);

        $scope.message = {
            classes: '',
            text: '',
            show: function () {
                $scope.message._show = true;
                $interval(function () {
                    $scope.message._show = false;
                }, 5000);
            },
            _show: false
        };

        $scope.loading = true;
        $scope.getAll = function () {
            $http.get("/Spot/GetAll").success(function (data) {
                $scope.model = data;
                $scope.reports = [];

                angular.forEach(data.Reports, function (obj) {
                    // Parse dates values 
                    var report = obj;
                    report.CreateDate = moment(report.CreateDate);
                    if (report.UpdateDate !== null) {
                        report.UpdateDate = moment(report.UpdateDate);
                    }
                    $scope.reports.push(report);
                });

                activate();
            });
        };
        $scope.getAll();

        if (localStorage !== undefined) {
            $scope.spotTonality = localStorage.getItem('spotToanlity');
        };
        $scope.toggleSpot = function () {
            $scope.spotTonality = !$scope.spotTonality;
            if (localStorage !== undefined) {
                localStorage.setItem('spotToanlity', $scope.spotTonality);
            };
        };

        function activate() {
            $scope.loading = false;
            $scope.selectPersonality = function () {
                var CenterId = $scope.getSelectedId('Center');
                var HarmonicGroupId = $scope.getSelectedId('HarmonicGroup');
                var SocialStyleId = $scope.getSelectedId('SocialStyle');
                var TonalityId = $scope.getSelectedId('Tonality');

                $scope.personalities = [];
                angular.forEach($scope.model.Personalities, function (personality) {
                    if (CenterId != null || SocialStyleId != null || HarmonicGroupId != null || TonalityId != null) {
                        if ((personality.CenterId == CenterId || CenterId == null)
                            && (personality.SocialStyleId == SocialStyleId || SocialStyleId == null)
                            && (personality.TonalityId == TonalityId || TonalityId == null)
                            && (personality.HarmonicGroupId == HarmonicGroupId || HarmonicGroupId == null)) {
                            $scope.personalities.push(personality);
                        };
                    };
                });
            };

            $scope.getSelectedId = function (name) {
                var s = $scope.selected[name];
                return angular.isDefined(s) ? s.Id : null;
            };

            $scope.select = function (name, Id, $index) {
                var s = $scope.selected[name];
                if (angular.isDefined(s) && s.Id == Id) {
                    // Deselect
                    $scope.selected[name] = undefined;
                } else {
                    // Select
                    $scope.selected[name] = $scope.model[names[name]][$index];
                    $scope.selected[name].$index = $index;
                };

                $scope.selectPersonality();
            };
            // Reset all selected and model
            $scope.clear = function () {
                $scope.selected = new SelectedViewModel();
                $scope.report = new Report();
                $scope.personalities = [];
            };

            $scope.submit = function () {
                $scope.loading = true;
                $scope.report.PersonalityId = $scope.personalities[0].Id;
                $http.post('/Spot/Create', $scope.report)
                .success(function () {
                    // Success 
                    $scope.report.Personality = $scope.personalities[0];
                    $scope.reports.push($scope.report);

                    $scope.clear();
                    $scope.loading = false;

                    // Increment
                    $scope.model.TotalToday += 1;
                    $scope.model.TotalReports += 1;

                    $scope.message.classes = ['alert-success'];
                    $scope.message.text = "Gemt! - Din raport er modtaget";
                    $scope.message.show();
                }).error(function () {
                    // Error 
                    // Display an error message
                });
            };

            $scope.preselect = function (name, Id) {
                var contains = false;

                angular.forEach($scope.personalities, function (personality) {

                    // TODO: Make faster
                    if (!angular.isDefined($scope.selected[name])) {
                        var newName = name + "Id";
                        if (personality[newName] == Id) {
                            contains = true;
                        };
                    };
                });

                return contains;
            };

            $scope.classes = function (name, Id) {
                var classes = [],
                    contains = false;

                if ($scope.getSelectedId(name) == Id) {
                    return ['active'];
                };

                if ($scope.personalities.length == 0) {
                    return [];
                };

                contains = $scope.preselect(name, Id);

                if ($scope.personalities.length == 1 && contains) {
                    return ['active'];
                } else if ($scope.personalities.length == 1 && !contains && $scope.getSelectedId(name) == null) {
                    return ['disabled'];
                };

                return [];
            };

            // Default images
            var defaultImages = {
                'Center': '/Content/images/Centers.png',
                'Tonality': '/Content/images/Tonalities.png',
                'HarmonicGroup': '/Content/images/HarmonicGroups.png',
                'SocialStyle': '/Content/images/SocialStyles.png'
            };

            // Return the apropiate url for current object name
            $scope.getImage = function (name) {
                if ($scope.personalities.length == 1) {
                    return $scope.personalities[0][name].Url;
                } else if (angular.isDefined($scope.selected[name])
                    && ($scope.selected[name].Url !== null && $scope.selected[name].Url.length > 0)) {
                    return $scope.selected[name].Url;
                } else {
                    return defaultImages[name];
                };
            };

            $scope.getDescription = function (name) {
                //<p ng-show="angular.isDefined(selected.Tonality)" class="list-group-item-text" ng-bind-html="selected.Tonality.Description"></p>
                //<p ng-show="!angular.isDefined(selected.Tonality) && personalities.length == 1" class="list-group-item-text" ng-bind-html="personalities[0].Tonality.Description"></p>
                if (angular.isDefined($scope.selected[name])) {
                    return $scope.selected[name].Description;
                } else if($scope.personalities.length == 1) {
                    return $scope.personalities[0][name].Description;
                }
                return null;
            };
        };
    };
})();
