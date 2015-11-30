(function () {
    'use strict';

    angular.module('app', [
        // Angular modules 
        'ngAnimate',
        //'ngRoute',
        'ngSanitize',


        // Custom modules 

        // 3rd Party Modules
        'ui.bootstrap'
    ]);

    angular
        .module('app')
        .filter('ctime', function () {

            return function (jsonDate) {

                var date = new Date(parseInt(jsonDate.substr(6)));
                return date;
            };

        });

    angular
        .module('app')
        .filter('seconds', function () {

            return function (date) {
                return moment(date).seconds();
            };

        });

    angular
    .module('app')
    .filter('dateFormat', function () {
        return function (date) {
            return moment(date).format('DD-MM-YYYY');
        };

    });

    angular
   .module('app')
   .filter('postDateFormat', function () {
       return function (date) {
           return moment(date).format('MM-DD-YYYY');
       };
   });

    angular
    .module('app')
    .filter('dateTime', function () {
        return function (date) {
            return moment(date).format('DD-MM-YYYY HH:mm ');
        };
    });

    angular
    .module('app')
    .filter('fromNow', function () {

        return function (jsonDate) {
            var date = moment(jsonDate);
            return date.fromNow();
        };
    });

    angular
    .module('app')
    .filter('moment', function () {
        return function (jsonDate, format) {
            var date = moment(jsonDate);
            return date.format(format);
        };
    });

    angular
    .module('app')
    .filter('calendar', function () {

        return function (jsonDate) {
            var date = moment(jsonDate);
            return date.calendar();
        };
    });

    angular.module('app').directive('datepickerPopup', function (dateFilter, datepickerPopupConfig) {
        return {
            restrict: 'A',
            priority: 1,
            require: 'ngModel',
            link: function (scope, element, attr, ngModel) {
                var dateFormat = attr.datepickerPopup || datepickerPopupConfig.datepickerPopup;
                ngModel.$formatters.push(function (value) {
                    return dateFilter(value, dateFormat);
                });
            }
        };
    });

    angular.module('app').filter('orderObjectBy', function () {
        return function (items, field, reverse) {
            var filtered = [];
            angular.forEach(items, function (item) {
                filtered.push(item);
            });
            filtered.sort(function (a, b) {
                return (a[field] > b[field] ? 1 : -1);
            });
            if (reverse) filtered.reverse();
            return filtered;
        };
    });

})();