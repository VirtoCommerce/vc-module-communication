angular.module('CommunicationModule')
    .controller('CommunicationModule.helloWorldController', ['$scope', 'CommunicationModule.webApi', function ($scope, api) {
        var blade = $scope.blade;
        blade.title = 'CommunicationModule';

        blade.refresh = function () {
            api.get(function (data) {
                blade.title = 'CommunicationModule.blades.hello-world.title';
                blade.data = data.result;
                blade.isLoading = false;
            });
        };

        blade.refresh();
    }]);
