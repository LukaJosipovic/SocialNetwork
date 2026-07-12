using Application.DTO;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class ActivityHub : Hub
    {
        //private static readonly Dictionary<string, string> _connections = new();

        //public override Task OnConnectedAsync()
        //{
        //    string userId = Context.UserIdentifier;

        //    if (!string.IsNullOrWhiteSpace(userId))
        //        _connections[userId] = Context.ConnectionId;

        //    return base.OnConnectedAsync();
        //}
        
        //public async Task SendActivityNotification()
        //{
        //    await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveActivityNotification");
        //}
    }
}
