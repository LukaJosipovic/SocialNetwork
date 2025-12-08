using Application.Helper;
using Application.Service.Chat;
using Application.Service.Notification;
using Domain.Model;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;
        private readonly IChatService _chatService;
        private readonly INotificationService _notificationService;

        public ChatHub(AppDbContext context, IChatService chatService, INotificationService notificationService)
        {
            _context = context;
            _chatService = chatService;
            _notificationService = notificationService;
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
            //var chatMessage = new ChatMessage
            //{
            //    SenderId = senderId,
            //    ReceiverId = receiverId,
            //    Content = message,
            //    Timestamp = DateTime.Now
            //};

            //await _context.ChatMessage.AddAsync(chatMessage);
            //await _context.SaveChangesAsync();
            var result = await _chatService.SaveMessage(senderId, receiverId, message);
            var deviceToken = await _notificationService.GetDeviceToken(receiverId);
            if (deviceToken != null)
            {
                var notificationSent = await NotificationHelper.SendNotification(deviceToken, "Message", "You have received a message");
            }
            if (_connections.TryGetValue(receiverId, out string connectionId) && result)
                await Clients.Client(connectionId).SendAsync("ReceiveClientMessage", senderId, message);
        }

        public Task SendMessage(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
