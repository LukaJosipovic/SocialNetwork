using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Application.Exceptions;
using MobileClient.Helper;
using MobileClient.Services.Location;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Platforms.Android.LocationTracker
{
    [Service(ForegroundServiceType = ForegroundService.TypeLocation)]
    public class LocationTrackerService : Service
    {
        ILocationService _locationService;
        CancellationTokenSource cts;

        public override void OnCreate()
        {
            base.OnCreate();
            cts = new CancellationTokenSource();
            _locationService = IPlatformApplication.Current.Services.GetRequiredService<ILocationService>();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            StartForeground(1001, CreateNotification());
            Task trackLocation = new Task(async () =>
            {
                await TrackLocationAsync(cts.Token);
            });
            trackLocation.Start();
            return StartCommandResult.NotSticky;
        }

        private async Task TrackLocationAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var location = await LocationHelper.GetCurrentLocation();
                    if (location != null)
                    {
                        // send to API
                        //System.Diagnostics.Debug.WriteLine("Tracking");
                        var response = await _locationService.AddLocation(location.Latitude, location.Longitude);
                    }

                    await Task.Delay(20000, token);
                }
            }
            catch (AccountBannedException)
            {
                StopSelf();
            }
            catch (Exception ex)
            {

            }
        }

        private Notification? CreateNotification()
        {
            var channelId = "location_channel";
            var manager = GetSystemService(NotificationService) as NotificationManager;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                manager.CreateNotificationChannel(
                    new NotificationChannel(channelId, "Location Tracking", NotificationImportance.Low));
            }

            return new Notification.Builder(this, channelId)
                .SetContentTitle("Tracking location")
                .SetSmallIcon(Resource.Drawable.abc_ic_ab_back_material)
                .Build();
        }

        public override IBinder? OnBind(Intent? intent)
        {
            return null;
        }
    }
}
