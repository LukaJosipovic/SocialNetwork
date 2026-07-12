using Application.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Notification
{
    public interface INotificationService
    {
        Task<GeneralResponse> RegisterDevice(string userId, string deviceToken);
        Task<List<string>> GetDeviceToken(string userId);
        Task<GeneralResponse> DeactivateDevice(string userId, string deviceToken);
    }
}
