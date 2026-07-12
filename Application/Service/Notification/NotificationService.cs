using Application.Contracts;
using Application.DTO.Response;
using Application.Helper;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IAccountRepository _accountRepository;

        public NotificationService(INotificationRepository notificationRepository, IAccountRepository accountRepository)
        {
            _notificationRepository = notificationRepository;
            _accountRepository = accountRepository;
        }

        public async Task<GeneralResponse> DeactivateDevice(string userId, string deviceToken)
        {
            try
            {
                var user = await _accountRepository.GetUserById(userId);
                var deactivated = await _notificationRepository.DeactivateDevice(userId, deviceToken);
                
                if (deactivated)
                    return ResponseHelper.CreateGeneralResponse(true, null);

                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }

        public async Task<List<string>> GetDeviceToken(string userId)
        {
            try
            {
                var deviceTokens = new List<string>();
                var devices = await _notificationRepository.GetUserDevice(userId);
                foreach (var device in devices)
                {
                    deviceTokens.Add(device.DeviceToken);
                }
                return deviceTokens;
            }
            catch (KeyNotFoundException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<GeneralResponse> RegisterDevice(string userId, string deviceToken)
        {
            try
            {
                var user = await _accountRepository.GetUserById(userId);
                var deviceExists = await _notificationRepository.DeviceExists(userId, deviceToken);
                
                if (deviceExists)
                {
                    var activated = await _notificationRepository.ActivateDevice(userId, deviceToken);
                    
                    if (activated)
                        return ResponseHelper.CreateGeneralResponse(true, null);

                    return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
                }
                else
                {
                    var userDevice = new UserDevice
                    {
                        DeviceToken = deviceToken,
                        IsActive = true,
                        User = user
                    };

                    var result = await _notificationRepository.RegisterDevice(userDevice);

                    if (result)
                        return ResponseHelper.CreateGeneralResponse(true, null);
                
                    return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
                }
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateGeneralResponse(false, "Something went wrong");
            }
        }
    }
}
