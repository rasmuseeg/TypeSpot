(function () {
    'use strict';

    angular
        .module('app')
        .service('reports', reportsService);

    reportsService.$inject = ['$http'];

    function reportsService($http) {
        this.getPagedReports = function (data) {
            return $http.post('/ReportsApi/GetPagedReports', data);
        };

        this.recycle = function (reportId) {
            return $http.post('/Report/Trash', { id: reportId });
        };

        this.restore = function (reportId) {
            return $http.post('/Report/Trash', { id: reportId });
        };
    }
})();