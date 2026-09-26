using Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Helper
{
    public static class GetPublicIpAddressHelper
    {
        public static async Task<string> GetPublicIpAddress()
        {
            using var client = new HttpClient();
            return await client.GetStringAsync("https://api.ipify.org");
        }
        public static string GetLocalIpAddress()
        {
            return "192.168.100.42:4321/socialnetwork";
        }
    }
}
