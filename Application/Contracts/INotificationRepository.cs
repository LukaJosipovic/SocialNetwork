using Application.DTO.Response;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface INotificationRepository
    {
        Task<bool> RegisterDevice(UserDevice userDevice);
        Task<bool> DeviceExists(string userId, string deviceToken);
        Task<bool> ActivateDevice(string userId, string deviceToken);
        Task<bool> DeactivateDevice(string userId, string deviceToken);
        Task<List<UserDevice>> GetUserDevice(string userId);
    }
}
