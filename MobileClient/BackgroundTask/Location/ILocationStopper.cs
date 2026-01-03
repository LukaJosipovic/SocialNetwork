using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.BackgroundTask.Location
{
    public interface ILocationStopper
    {
        Task StopAsync();
    }
}
