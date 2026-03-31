//#if ANDROID
//using MobileClient.Platforms.Android.BackgroundService;
//#endif
using MobileClient.Services.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.BackgroundTask.Location
{
#if ANDROID
    public class AndroidLocationStopper : ILocationStopper
    {
        private readonly MobileClient.Services.Location.IBackgroundLocationService _backgroundLocationService;
        private readonly ILocationService _locationService;

        public AndroidLocationStopper(
            IBackgroundLocationService backgroundLocationService,
            ILocationService locationService)
        {
            _backgroundLocationService = backgroundLocationService;
            _locationService = locationService;
        }

        public async Task StopAsync()
        {
            await _backgroundLocationService.Stop();
            //await _locationService.RemoveLocation();
        }
    }
#endif
}
