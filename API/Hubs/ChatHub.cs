using Application.Helper;
using Application.Service.Activity;
using Application.Service.Chat;
using Application.Service.Notification;
using Domain.Model;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace API.Hubs
{
    public class ChatHub : Hub
    {
        //private readonly AppDbContext _context;
        private readonly IChatService _chatService;
        private readonly INotificationService _notificationService;
        private readonly IActivityService _activityService;

        public ChatHub(IChatService chatService, INotificationService notificationService, IActivityService activityService)
        {
            _chatService = chatService;
            _notificationService = notificationService;
            _activityService = activityService;
        }

        //private static readonly Dictionary<string, string> _connections = new();

        //public override Task OnConnectedAsync()
        //{
        //    string userId = Context.UserIdentifier;

        //    if (!string.IsNullOrWhiteSpace(userId) )
        //        _connections[userId] = Context.ConnectionId;

        //    return base.OnConnectedAsync();
        //}

        //public override Task OnDisconnectedAsync(Exception? exception)
        //{
        //    string userId = Context.UserIdentifier;

        //    if (!string.IsNullOrWhiteSpace(userId))
        //        _connections.Remove(userId);

        //    return base.OnDisconnectedAsync(exception);
        //}

        public async Task SendMessageToClient(string senderId, string receiverId, string message, int conversationId)
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
            var result = await _chatService.SaveMessage(senderId, receiverId, message, conversationId);
            var deviceTokens = await _notificationService.GetDeviceToken(receiverId);
            if (deviceTokens != null && deviceTokens.Any() && result)
            {
                await Clients.User(receiverId).SendAsync("ReceiveClientMessage", senderId, message);
                await NotificationHelper.SendNotifications(deviceTokens, "Message", "You have received a message");
            }
            //if (_connections.TryGetValue(receiverId, out string connectionId) && result)
            //    await Clients.Client(connectionId).SendAsync("ReceiveClientMessage", senderId, message);
        }
        
        //public async Task CreateAndJoinGroup(string userId, int activityId)
        //{
        //    try
        //    {
        //        var activityDescription = await _activityService.GetActivityDescriptionById(activityId);
        //        var groupExists = await _chatService.CreateGroup(activityId, activityDescription);
        //        if (groupExists)
        //        {
        //            await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation_{activityId}");
        //            //await Clients.Group($"conversation_{activityId}").SendAsync("ReceiveGroupMessage", userId, $"User {userId} has joined the group for activity {activityDescription}");
        //        }
        //    }
        //    catch (KeyNotFoundException)
        //    {

        //        throw;
        //    }
        //}

        //public async Task SendGroupMessage(string senderId, string message, int conversationId, int activityId)
        //{
        //    var result = await _chatService.SaveGroupMessage(senderId, message, conversationId);

        //    if (!result)
        //        return;

        //    await Clients.Group($"conversation_{activityId}").SendAsync("ReceiveGroupMessage", senderId, message, conversationId);
        //}

        public Task SendMessage(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
