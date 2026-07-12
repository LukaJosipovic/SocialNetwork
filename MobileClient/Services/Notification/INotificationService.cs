using Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Services.Notification
{
    public interface INotificationService
    {
        Task<GeneralResponse> RegisterDevice(string deviceToken);
        Task<GeneralResponse> DeactivateDevice(string deviceToken);
    }
}
