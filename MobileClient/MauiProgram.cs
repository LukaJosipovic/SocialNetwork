using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MobileClient.BackgroundTask;
using MobileClient.BackgroundTask.Location;
using MobileClient.Handlers;
using MobileClient.Services.Account;
using MobileClient.Services.Activity;
using MobileClient.Services.Alert;
using MobileClient.Services.Auth;
using MobileClient.Services.Chat;
using MobileClient.Services.Location;
using MobileClient.Services.Post;
using MobileClient.SessionService;

using System.Net;

namespace MobileClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddHttpClient("BaseApi", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7098/");
                //client.BaseAddress = new Uri("http://10.0.2.2:5209/");
                //client.BaseAddress = new Uri("http://192.168.100.4/socialnetwork/");
                //client.BaseAddress = new Uri("http://localhost/socialnetwork/");
                //client.BaseAddress = new Uri("https://localhost/socialnetwork/");
                //client.BaseAddress = new Uri("https://192.168.100.4/socialnetwork/");
                client.Timeout = TimeSpan.FromSeconds(30);
            }).AddHttpMessageHandler<TokenHandler>();

            builder.Services.AddTransient<TokenHandler>();
            builder.Services.AddSingleton<IAlertService, AlertService>();
            builder.Services.AddSingleton<UserSessionService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            //builder.Services.AddScoped<Application.Service.Auth.IAuthService, Application.Service.Auth.AuthService>();
            //builder.Services.AddScoped<Application.Service.Account.IAccountService, Application.Service.Account.AccountService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IActivityService, ActivityService>();
            builder.Services.AddScoped<IChatService, ChatService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddSingleton<ILocationTracker, LocationTracker>();

            //builder.Services.Configure<HostOptions>(x =>
            //{
            //    x.ServicesStartConcurrently = true;
            //    x.ServicesStopConcurrently = false;
            //});
            //builder.Services.AddHostedService<LocationBackgroundService>();

            builder.Services.AddMauiBlazorWebView();
            
#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
