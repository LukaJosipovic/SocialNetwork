using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Services.Alert
{
    public class AlertService : IAlertService
    {
        public Task ShowAlertAsync(string title, string message, string cancel)
        {
            var mainPage = Microsoft.Maui.Controls.Application.Current?.MainPage;
            return mainPage?.DisplayAlert(title, message, cancel) ?? Task.CompletedTask;
        }
    }
}
