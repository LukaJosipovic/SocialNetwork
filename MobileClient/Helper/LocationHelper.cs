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
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                #if ANDROID
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
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
    }
}
