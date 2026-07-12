using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileClient.BackgroundTask.Location;

namespace MobileClient.SessionService
{
    public class UserSessionService
    {
        private readonly ILocationStopper _locationStopper;

        public UserSessionService(ILocationStopper locationStopper)
        {
            _locationStopper = locationStopper;
        }

        public event Func<Task>? OnUserBanned;

        public async Task TriggerBannedAsync()
        {
            await _locationStopper.StopAsync();

            if (OnUserBanned is not null)
                await OnUserBanned.Invoke();
        }

    }
}
