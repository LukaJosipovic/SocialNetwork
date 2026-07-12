using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
