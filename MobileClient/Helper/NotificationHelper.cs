using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Helper
{
    public static class NotificationHelper
    {
        public static async Task GetNotificationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

            if (status != PermissionStatus.Granted)
            {
#if ANDROID
                status = await Permissions.RequestAsync<Permissions.PostNotifications>();
#endif
            }
        }
    }
}
