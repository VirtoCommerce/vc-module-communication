// Call this to register your module to main application
var moduleName = 'CommunicationModule';

if (AppDependencies !== undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, [])
    .config(['$stateProvider',
        function ($stateProvider) {
        //    $stateProvider
        //        .state('workspace.CommunicationModuleState', {
        //            url: '/CommunicationModule',
        //            templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
        //            controller: [
        //                'platformWebApp.bladeNavigationService',
        //                function (bladeNavigationService) {
        //                    var newBlade = {
        //                        id: 'blade1',
        //                        controller: 'CommunicationModule.helloWorldController',
        //                        template: 'Modules/$(VirtoCommerce.CommunicationModule)/Scripts/blades/hello-world.html',
        //                        isClosingDisabled: true,
        //                    };
        //                    bladeNavigationService.showBlade(newBlade);
        //                }
        //            ]
        //        });
        }
    ])
    .run(['platformWebApp.mainMenuService', '$state',
        function (mainMenuService, $state) {
        //    //Register module in main menu
        //    var menuItem = {
        //        path: 'browse/CommunicationModule',
        //        icon: 'fa fa-cube',
        //        title: 'CommunicationModule',
        //        priority: 100,
        //        action: function () { $state.go('workspace.CommunicationModuleState'); },
        //        permission: 'CommunicationModule:access',
        //    };
        //    mainMenuService.addMenuItem(menuItem);
        }
    ]);
