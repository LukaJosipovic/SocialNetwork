using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if ANDROID
using MobileClient.Platforms.Android.BackgroundService;
#endif

namespace MobileClient.SessionService
{
    public class UserSessionService
    {
#if ANDROID
        private readonly IBackgroundLocationService _backgroundLocationService;

        public UserSessionService(IBackgroundLocationService backgroundLocationService)
        {
            _backgroundLocationService = backgroundLocationService;
        }
#endif

        //public event Action<string>? OnUserBanned;

        //public void TriggerBanned(string message)
        //{
        //    OnUserBanned?.Invoke(message);
        //}

        //public event Action? OnUserBanned;

        //public void TriggerBanned()
        //{
        //    OnUserBanned?.Invoke();
        //}
        public event Func<Task>? OnUserBanned;

        public async Task TriggerBannedAsync()
        {
#if ANDROID
            await _backgroundLocationService.Stop();
#endif
            if (OnUserBanned is not null)
                await OnUserBanned.Invoke();
        }

    }
}
