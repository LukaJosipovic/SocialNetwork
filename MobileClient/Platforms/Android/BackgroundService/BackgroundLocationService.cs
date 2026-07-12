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
    public class BackgroundLocationService : MobileClient.Services.Location.IBackgroundLocationService
    {
        public async Task Start()
        {
            await SecureStorage.SetAsync("ShouldRunLocationService", "true");
            var context = global::Android.App.Application.Context;
            var intent = new Intent(context, typeof(LocationTrackerService));
            intent.SetPackage(context.PackageName);
            context.StartForegroundService(intent);
        }

        public async Task Stop()
        {
            await SecureStorage.SetAsync("ShouldRunLocationService", "false");
            var context = global::Android.App.Application.Context;
            var intent = new Intent(context, typeof(LocationTrackerService));
            intent.SetPackage(context.PackageName);
            context.StopService(intent);
        }
    }
}
