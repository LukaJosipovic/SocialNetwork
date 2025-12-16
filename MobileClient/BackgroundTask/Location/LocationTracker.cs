using MobileClient.Services.Location;
using MobileClient.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.BackgroundTask.Location
{
    public class LocationTracker : ILocationTracker
    {
        private readonly ILocationService _locationService;
        private CancellationTokenSource _cts;
        private Task _trackingTask;

        public LocationTracker(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public Task StartAsync()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
                return Task.CompletedTask; // Already running

            _cts = new CancellationTokenSource();
            //_trackingTask = Task.Run(() => TrackLocationAsync(_cts.Token));
            Task trackLocation = new Task(async () =>
            {
                await TrackLocationAsync(_cts.Token);
            });
            trackLocation.Start();
            return Task.CompletedTask; // Let the caller continue
        }

        public async Task<bool> Stop()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts = null;
            }
            return await _locationService.RemoveLocation();
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
