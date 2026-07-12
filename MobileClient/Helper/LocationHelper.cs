#if ANDROID
using Android.Content;
using Android.OS;
using Android.Provider;
using AndroidX.AppCompat.App;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Helper
{
    public static class LocationHelper
    {
        private static CancellationTokenSource? _cancelTokenSource;
        public static async Task<bool> GetLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();

            if (status != PermissionStatus.Granted)
            {
#if ANDROID
                status = await Permissions.RequestAsync<Permissions.LocationAlways>();
#else
                status = PermissionStatus.Granted;
#endif
            }
            if (status == PermissionStatus.Granted)
            {
                return true;
            }
            return false;
        }

        public static async Task<Location?> GetCurrentLocation()
        {
            try
            {
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                Location? location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                return location;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task RequestIgnoreBatteryOptimizationAsync()
        {
#if ANDROID
            try
            {
                var context = Platform.CurrentActivity ?? Android.App.Application.Context;
                var powerManager = (PowerManager)context.GetSystemService(Context.PowerService);
                if (powerManager.IsIgnoringBatteryOptimizations(context.PackageName))
                    return;

                bool userAgreed = await ShowBatteryExplanationDialogAsync(context);
                if (!userAgreed)
                    return;

                var intetnt = new Intent(Settings.ActionRequestIgnoreBatteryOptimizations, Android.Net.Uri.Parse("package:" + context.PackageName));
                intetnt.AddFlags(ActivityFlags.NewTask);
                context.StartActivity(intetnt);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Battery optimization request failed: {ex}");
            }
#endif
        }

#if ANDROID
        private static Task<bool> ShowBatteryExplanationDialogAsync(Context context)
        {
            var tcs = new TaskCompletionSource<bool>();

            var builder = new AlertDialog.Builder(context)
                .SetTitle("Continuous Location Tracking")
                .SetMessage("To keep tracking your location in the background reliably, " +
                            "please allow this app to ignore battery optimizations.\n\n" +
                            "This is required for the feature to work properly.")
                .SetPositiveButton("Allow", (s, e) => tcs.SetResult(true))
                .SetNegativeButton("Later", (s, e) => tcs.SetResult(false))
                .SetCancelable(false);

            builder.Create().Show();
            return tcs.Task;
        }
#endif
    }
}
