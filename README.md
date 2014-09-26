AngularJS Authentication
=======================

Tutorial shows Authentication in AngularJS with ASP.NET Web API 2 and Owin Middleware using access tokens and refresh tokens approach. 

The initial demo has been slightly modified. a fairly basic integration of SignalR has been done.
I will take the integration a bit further in the near future.

One point to remember for guys doing their own version of this, is the creation of the hubs.js for the Angular project.
SignalR has tools available to create a proxy file, and you can find more details here

http://www.asp.net/signalr/overview/signalr-20/hubs-api/hubs-api-guide-javascript-client

On the above link look for this part: 
How to create a physical file for the SignalR generated proxy

While in dev mode, I choose a simpler option to create my proxy files, simply start the web server hosting your hub, and navigate to for example http://localhost:84/signalr/hubs, the browser will display a hub proxy file for you and you can simply copy the contents of this and paste it in the hubs.js file in the Angular client project and you are good to go.

![Alt text](http://bitoftech.net/wp-content/uploads/2014/05/AngularJSAuthentication.png "AngularJS Authentication")
![Alt text](http://bitoftech.net/wp-content/uploads/2014/07/RefreshTokenAngularJS.jpg "AngularJS Refresh Tokens")
