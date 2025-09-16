using Domain.Model;
using Infrastructure.Data;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;

        public ChatHub(AppDbContext context)
        {
            _context = context;
        }

        private static readonly Dictionary<string, string> _connections = new();

        public override Task OnConnectedAsync()
        {
            string userId = Context.UserIdentifier;
            
            if (!string.IsNullOrWhiteSpace(userId) )
                _connections[userId] = Context.ConnectionId;

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string userId = Context.UserIdentifier;

            if (!string.IsNullOrWhiteSpace(userId))
                _connections.Remove(userId);

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToClient(string senderId, string receiverId, string message)
        {
            var chatMessage = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = message,
                Timestamp = DateTime.Now
            };

            await _context.ChatMessage.AddAsync(chatMessage);
            await _context.SaveChangesAsync();

            if (_connections.TryGetValue(receiverId, out string connectionId))
                await Clients.Client(connectionId).SendAsync("ReceiveClientMessage", senderId, message);
        }

        public Task SendMessage(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
