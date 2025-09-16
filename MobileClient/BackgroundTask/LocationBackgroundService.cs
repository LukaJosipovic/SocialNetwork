using Microsoft.Extensions.Hosting;
using MobileClient.Services.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.BackgroundTask
{
    public class LocationBackgroundService : IHostedService
    {
        private readonly ILocationService _locationService;

        public LocationBackgroundService(ILocationService locationService)
        {
            _locationService = locationService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _ = Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                    if (status != PermissionStatus.Granted)
                    {
                        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    }

                    GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                    //var location = await Geolocation.Default.GetLocationAsync(request);

                    Microsoft.Maui.Devices.Sensors.Location location = new Microsoft.Maui.Devices.Sensors.Location
                    {
                        Latitude = 45.815399,
                        Longitude = 15.966568
                    };

                    await _locationService.AddLocation(location.Latitude, location.Longitude);
                    await Task.Delay(30000);
                };
            });
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        //        if (status != PermissionStatus.Granted)
        //        {
        //            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        //        }

        //        GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
        ////var location = await Geolocation.Default.GetLocationAsync(request);

        //Microsoft.Maui.Devices.Sensors.Location location = new Microsoft.Maui.Devices.Sensors.Location
        //{
        //    Latitude = 45.815399,
        //    Longitude = 15.966568
        //};

        //await _locationService.AddLocation(location.Latitude, location.Longitude);
        //await Task.Delay(30000);
        //    }
        //}
    }
}
