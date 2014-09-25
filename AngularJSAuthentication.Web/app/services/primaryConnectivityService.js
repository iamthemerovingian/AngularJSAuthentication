(function ()
{
    'use strict';
    app.factory('primaryConnectivityService', ['$http', 'ngAuthSettings', 'localStorageService', '$rootScope', '$q', function ($http, ngAuthSettings, localStorageService, $rootScope, $q)
          {

              var serviceBase = ngAuthSettings.apiServiceBaseUri;
              var chatHub = {};
              var chatHubServer = {};
              var started = false;

              var initialize = function ()
              {
                  var chatHubName = "PrimaryConnectivity";
                  chatHub = $.connection[chatHubName];

                  chatHubServer = chatHub.server;
                  var authData = localStorageService.get('authorizationData');
                  $.connection.hub.qs = { Bearer: authData.token };
                  $.connection.hub.url = serviceBase + 'signalr';
                  //$.connection.hub.logging = true;
                  $.connection.hub.error(function (error)
                  {
                      console.log('SignalR error: ' + error);
                  });                 

                  // Presence Related
                  $.connection[chatHubName].client.welcome = function (message) {                      
                      $rootScope.$broadcast("onWelcome", message);
                  };

                  $.connection[chatHubName].client.welcomeback = function (message)
                  {
                      $rootScope.$broadcast("onWelcomeBack", message);
                  };

                  return $q.when($.connection.hub.start().done(setStarted));
              };


              function getServerMessage(id) { return $q.when(chatHubServer.getServerMessage(id)); };

              var setStarted = function ()
              {
                  started = true;
              }

              var isStarted = function ()
              {
                  return started;
              }

              return {
                  initialize: initialize,
                  isStarted: isStarted,
                  getServerMessage: getServerMessage
              }
          }]);
})();