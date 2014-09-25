'use strict';
app.controller('ordersController', ['$scope', '$rootScope', 'ordersService', 'primaryConnectivityService', function ($scope, $rootScope, ordersService, primaryConnectivityService)
{

    $scope.orders = [];

    ordersService.getOrders().then(function (results) {

        $scope.orders = results.data;

    }, function (error) {
        //alert(error.data.message);
    });

    $rootScope.$on("onWelcome", function (e, message) {        
        alert(message);
    });

    $rootScope.$on("onWelcomeBack", function (e, message)
    {        
        alert(message);
    });

    function activate()
    {
        primaryConnectivityService.initialize().then(communicationServerInitialized);
        primaryConnectivityService.initialize();
    }

    var communicationServerInitialized = function () {
        alert("Started");
    }

    activate();

}]);