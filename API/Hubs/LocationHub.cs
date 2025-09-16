using Application.DTO;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class LocationHub : Hub
    {
        // Server → everybody: ask each client for its location
        public async Task SendActivityNotification()
        {
            await Clients.All.SendAsync("RequestLocation");
        }       
    }
}
