using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularJSAuthentication.API.Hubs
{
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Threading.Tasks;

    using AngularJSAuthentication.API.SignalRHelpers;

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    [QueryStringBearerAuthorize]
    [HubName("PrimaryConnectivity")]
    public class PrimaryConnectivityHub : Hub
    {
        #region Fields

        private const string ServerMessage = "Do Stuff"; 

        private static readonly ConnectionMapping<string> Connections = new ConnectionMapping<string>();

        #endregion Fields

        #region Methods

        public void SendWelcome(string userName, string userId)
        {
            Clients.Client(Context.ConnectionId).welcome(string.Format("Welcome: {0} - {1}", userName, userId));
        }

        public void SendWelcomeBack(string userName, string userId)
        {
            Clients.Client(Context.ConnectionId).welcomeback(string.Format("Welcome Back: {0} - {1}", userName, userId));
        }

        public string GetServerMessage(int id)
        {
            return string.Format("{0}: {1}", ServerMessage, id);
        }

        /// <summary>
        /// The on connected.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override Task OnConnected()
        {
            // Identity Related
            var connectionId = Context.ConnectionId;
            var userName = this.GetCurrentUserName();
            var userId = this.GetCurrentUserId(userName);

            // Presence Related            
            Connections.Add(userName, connectionId);
            Groups.Add(connectionId, "Room1");
            this.SendWelcome(userName, userId);

            return base.OnConnected();
        }

        /// <summary>
        /// The on disconnected.
        /// </summary>
        /// <param name="stopCalled">
        /// The stop called.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            // Identity Related
            var connectionId = Context.ConnectionId;
            var userName = this.GetCurrentUserName();

            // Presence Related            
            Connections.Remove(userName, Context.ConnectionId);
            Groups.Remove(connectionId, "Room1");            

            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// The on reconnected.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override Task OnReconnected()
        {
            var connectionId = Context.ConnectionId;
            var userName = this.GetCurrentUserName();
            var userId = this.GetCurrentUserId(userName);

            Groups.Add(connectionId, "Room1");
            this.SendWelcomeBack(userName, userId);
            if (!Connections.GetConnections(userName).Contains(connectionId))
            {
                Connections.Add(userName, Context.ConnectionId);
            }

            return base.OnReconnected();
        }

        #endregion //Methods

        private string GetCurrentUserName()
        {
            if (Context.User.GetType() == typeof(GenericPrincipal) || Context.User.GetType() == typeof(WindowsPrincipal))
            {
                var principle = Context.Request.Environment["server.User"] as ClaimsPrincipal;
                if (principle != null)
                {
                    return principle.Identity.Name;
                }

                var token = Context.Request.QueryString.Get("Bearer");
                var authenticationTicket = Startup.AuthServerOptions.AccessTokenFormat.Unprotect(token);
                return authenticationTicket.Identity.Name;
            }
            else
            {
                var principle = Context.User as ClaimsPrincipal;
                return principle != null ? principle.Identity.Name : string.Empty;
            }
        }

        private string GetCurrentUserId(string userName)
        {

            using (var authRepository = new AuthRepository())
            {
                var user = authRepository.FindUser(userName);
                if (user != null)
                {
                    return user.Id;
                }
            }

            return string.Empty;
        }
    }
}