using Application.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MobileClient.Helper
{
    public static class ConfigHelper
    {
        public static AppConfig GetAppConfig()
        {
            using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.json").GetAwaiter().GetResult();
            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();

            var config = JsonSerializer.Deserialize<AppConfig>(json);
            return config;
        }
    }
}
