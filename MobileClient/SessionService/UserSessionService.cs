using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.SessionService
{
    public class UserSessionService
    {
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
            if (OnUserBanned is not null)
                await OnUserBanned.Invoke();
        }

    }
}
