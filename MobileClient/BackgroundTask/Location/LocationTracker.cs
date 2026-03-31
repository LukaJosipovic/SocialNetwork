using MobileClient.Services.Location;
using MobileClient.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom;

namespace MobileClient.BackgroundTask.Location
{
    public class LocationTracker : IBackgroundLocationService
    {
        private readonly ILocationService _locationService;
        private CancellationTokenSource _cts;
        private Task _trackingTask;

        public LocationTracker(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public Task Start()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
                return Task.CompletedTask; // Already running

            _cts = new CancellationTokenSource();
            Task trackLocation = new Task(async () =>
            {
                await TrackLocationAsync(_cts.Token);
            });
            trackLocation.Start();
            return Task.CompletedTask; // Let the caller continue
        }

        public async Task Stop()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }
            var response = await _locationService.RemoveLocation();

            if (!response)
                throw new Exception("Location cannot be removed");
        }

        private async Task TrackLocationAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var location = await LocationHelper.GetCurrentLocation();

                    if (location == null)
                    {
                        location = new Microsoft.Maui.Devices.Sensors.Location
                        {
                            Latitude = 45.83111,
                            Longitude = 16.11639
                        };
                    }

                    if (location != null)
                         await _locationService.AddLocation(location.Latitude, location.Longitude);
                
                    await Task.Delay(20000, token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    // handle error (e.g., log it)
                }
            }
        }
    }
}
