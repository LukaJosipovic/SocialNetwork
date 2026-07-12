using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Application.DTO;
using Application.Exceptions;
using Microsoft.Maui.Devices.Sensors;
using MobileClient.Helper;
using MobileClient.Services.Location;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MobileClient.Platforms.Android.LocationTracker
{
    [Service(ForegroundServiceType = ForegroundService.TypeLocation)]
    public class LocationTrackerService : Service
    {
        ILocationService _locationService;
        CancellationTokenSource cts;
        //private PowerManager.WakeLock? wakeLock;

        public override void OnCreate()
        {
            base.OnCreate();
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                System.Diagnostics.Debug.WriteLine($"UNHANDLED: {ex}");
            };
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                System.Diagnostics.Debug.WriteLine($"UNOBSERVED: {e.Exception}");
            };
            cts = new CancellationTokenSource();

            _locationService = IPlatformApplication.Current.Services.GetRequiredService<ILocationService>();
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            StartForeground(1001, CreateNotification());

            //if (wakeLock == null)
            //{
            //    var powerManager = (PowerManager)GetSystemService(PowerService);
            //    wakeLock = powerManager.NewWakeLock(WakeLockFlags.Partial, "LocationTracker::WakeLock");
            //    wakeLock.Acquire();
            //}

            //Task trackLocation = new Task(async () =>
            //{
            //    await TrackLocationAsync(cts.Token);
            //});
            //trackLocation.Start();
            Task.Run(() => TrackLocationAsync(cts.Token));
            return StartCommandResult.Sticky;
        }

        private async Task TrackLocationAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var location = await LocationHelper.GetCurrentLocation();
                    if (location != null)
                    {
                        string? locationJson = await SecureStorage.GetAsync("userLocation");

                        if (!string.IsNullOrWhiteSpace(locationJson))
                        {
                            var oldLocationDto = JsonSerializer.Deserialize<LocationDTO>(locationJson);
                            var oldLocation = new Location(oldLocationDto.Latitude, oldLocationDto.Longitude);

                            var distanceInMeters = Location.CalculateDistance(oldLocation, location, DistanceUnits.Kilometers) * 1000;

                            if (distanceInMeters <= 20)
                            {
                                await Task.Delay(20000, token);
                            }
                        }

                        var newLocation = new LocationDTO
                        {
                            Latitude = location.Latitude,
                            Longitude = location.Longitude,
                        };
                        locationJson = JsonSerializer.Serialize(newLocation);
                        await SecureStorage.SetAsync("userLocation", locationJson);

                        var response = await _locationService.AddLocation(location.Latitude, location.Longitude);
                    }

                    await Task.Delay(20000, token);
                }
                catch (System.OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error tracking location: {ex}");
                    await Task.Delay(50000, token);
                }
            }
        }

        private Notification? CreateNotification()
        {
            var channelId = "location_channel";
            var manager = GetSystemService(NotificationService) as NotificationManager;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(channelId, "Background Location", NotificationImportance.Min);

                channel.Description = "Tracks location silently";
                channel.SetSound(null, null);
                channel.EnableVibration(false);
                channel.LockscreenVisibility = NotificationVisibility.Secret;

                manager.CreateNotificationChannel(channel);
            }

            return new Notification.Builder(this, channelId)
                .SetContentTitle("Tracking location")
                .SetSmallIcon(Resource.Mipmap.appicon)
                .SetOngoing(true)
                .Build();
        }

        public override IBinder? OnBind(Intent? intent)
        {
            return null;
        }

        public override void OnDestroy()
        {
            System.Diagnostics.Debug.WriteLine("OnDestroy.");
            
            cts?.Cancel();
            //wakeLock?.Release();

            cts?.Dispose();
            StopForeground(true);
            base.OnDestroy();
        }
    }
}
