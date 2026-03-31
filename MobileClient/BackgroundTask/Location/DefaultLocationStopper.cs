using MobileClient.Services.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.BackgroundTask.Location
{
#if !ANDROID
public class DefaultLocationStopper : ILocationStopper
{
    private readonly IBackgroundLocationService _locationTracker;
    private readonly ILocationService _locationService;

    public DefaultLocationStopper(
        IBackgroundLocationService locationTracker,
        ILocationService locationService)
    {
        _locationTracker = locationTracker;
        _locationService = locationService;
    }

    public async Task StopAsync()
    {
        await _locationTracker.Stop();
        //await _locationService.RemoveLocation();
    }
}
#endif
}
