using GoogleAuthentication.Models;
using Microsoft.AspNetCore.SignalR;

namespace GoogleAuthentication.Hubs
{
    public class ConectedHub:Hub
    {
        private readonly IHttpContextAccessor _httpContext;
     private readonly static ConnectionMapping<string> _connections = 
            new ConnectionMapping<string>();
        public ConectedHub(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        public override Task OnConnectedAsync()
        {
            var userId = Context.User.Identities.Last().Claims.Last().Value;
            //  ConnectedUsers.AllUsers.Add(userId,Context.ConnectionId);
            _connections.Add(userId, Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // var userId = _httpContext.HttpContext.Session.GetString("UserId");
            var userId = Context.User.Identities.Last().Claims.Last().Value;
            // ConnectedUsers.AllUsers.Remove(userId);
            _connections.Remove(userId, Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
        
        public async Task SendMessage(string user, string message)
        {
             var userId = Context.User.Identities.Last().Claims.Last().Value;
           
            if (string.IsNullOrEmpty(user))
            {

                await Clients.All.SendAsync("ReceiveMessage", userId, message);
            }
            else
            {
                foreach (var connectionId in _connections.GetConnections(user))
                {
                    await Clients.Clients(connectionId).SendAsync("ReceiveMessage", userId, message);
                }
                
            }
            
        }   
    }
}
