#if ANDROID
using Android.App;
using Android.Content;
#endif
using MobileClient.Platforms.Android.LocationTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application;
using MobileClient.BackgroundTask.Location;

namespace MobileClient.Platforms.Android.BackgroundService
{
    public class BackgroundLocationService : IBackgroundLocationService
    {
        private readonly ILocationTracker _locationTracker;

        public BackgroundLocationService(ILocationTracker locationTracker)
        {
            _locationTracker = locationTracker;
        }

        public async Task Start()
        {
            #if ANDROID
                var context = global::Android.App.Application.Context;
                var intent = new Intent(context, typeof(LocationTrackerService));
                context.StartForegroundService(intent);
            #else
                await _locationTracker.StartAsync();
            #endif
        }

        public async Task Stop()
        {
            #if ANDROID
                var context = global::Android.App.Application.Context; ;
                var intent = new Intent(context, typeof(LocationTrackerService));
                context.StopService(intent);
            #else
                await _locationTracker.Stop();
            #endif
        }
    }
}
