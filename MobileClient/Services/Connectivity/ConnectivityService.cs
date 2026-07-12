using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Services.Connectivity
{
    public class ConnectivityService
    {
        public bool IsConnected => Microsoft.Maui.Networking.Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

        public string? LastKnownPage { get; private set; }

        public event Action<bool>? ConnectivityChanged;

        public ConnectivityService()
        {
            Microsoft.Maui.Networking.Connectivity.Current.ConnectivityChanged += OnConnectivityChanged;
        }

        private void OnConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            ConnectivityChanged?.Invoke(IsConnected);
        }

        public void UpdateLastKnownPage(string pageUrl)
        {
            if (!string.IsNullOrEmpty(pageUrl) && !pageUrl.Contains("/nointernet", StringComparison.OrdinalIgnoreCase))
            {
                LastKnownPage = pageUrl;
            }
        }

        public async Task<bool> CheckInternetWithAlertAsync()
        {
            if (IsConnected)
                return true;

            await Microsoft.Maui.Controls.Application.Current.MainPage.DisplayAlert(
                "No Internet Connection",
                "Please check your internet connection and try again.",
                "OK");

            return false;
        }
    }
}
