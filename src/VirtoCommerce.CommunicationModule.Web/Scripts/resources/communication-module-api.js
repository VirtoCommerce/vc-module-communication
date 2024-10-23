angular.module('CommunicationModule')
    .factory('CommunicationModule.webApi', ['$resource', function ($resource) {
        return $resource('api/communication-module');
    }]);
