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
using Plugin.Firebase.CloudMessaging;
using System.Net;
using Microsoft.Maui.LifecycleEvents;
using MobileClient.Services.Notification;
using MobileClient.Services.LoadingService;


#if ANDROID
using Plugin.Firebase.Core.Platforms.Android;
using MobileClient.Platforms.Android.BackgroundService;
#endif

namespace MobileClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .RegisterFirebaseServices()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddHttpClient("BaseApi", client =>
            {
                #if ANDROID
                    client.BaseAddress = new Uri("http://10.0.2.2:5209/");
                #else
                    client.BaseAddress = new Uri("https://localhost:7098/");
                #endif
                //client.BaseAddress = new Uri("http://192.168.100.19:4321/socialnetwork/");
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
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddSingleton<ILocationTracker, LocationTracker>();
            builder.Services.AddSingleton<LoadingService>();
#if ANDROID
            builder.Services.AddSingleton<ILocationStopper, AndroidLocationStopper>();
#else
            builder.Services.AddSingleton<ILocationStopper, DefaultLocationStopper>();
#endif
#if ANDROID
            builder.Services.AddSingleton<IBackgroundLocationService, BackgroundLocationService>();
#endif
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
        private static MauiAppBuilder RegisterFirebaseServices(this MauiAppBuilder builder)
        {
            builder.ConfigureLifecycleEvents(events => {
#if ANDROID
                events.AddAndroid(android => android.OnCreate((activity, _) =>
                CrossFirebase.Initialize(activity)));
#endif
            });

            return builder;
        }
    }
}
