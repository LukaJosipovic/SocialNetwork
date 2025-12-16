using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public static class NotificationHelper
    {
        public async static Task<bool> SendNotification(string deviceToken, string title, string body)
        {
            var message = new Message()
            {
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = title,
                    Body = body,
                },
                Token = deviceToken,
            };

            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(message);

            if (!string.IsNullOrEmpty(result))
                return true;
            else
                return false;
        }
        public async static Task SendNotifications(List<string> deviceTokens, string title, string body)
        {
            var message = new MulticastMessage()
            {
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = title,
                    Body = body,
                },
                Tokens = deviceTokens
            };

            var messaging = FirebaseMessaging.DefaultInstance;
            await messaging.SendEachForMulticastAsync(message);
        }
    }
}
