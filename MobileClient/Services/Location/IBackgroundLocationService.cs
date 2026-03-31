using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Services.Location
{
    public interface IBackgroundLocationService
    {
        Task Start();
        Task Stop();
    }
}
