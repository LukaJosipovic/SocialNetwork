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

        public async Task<string> GetDeviceToken(string userId)
        {
            try
            {
                var device = await _notificationRepository.GetUserDevice(userId);
                return device.DeviceToken;
            }
            catch (KeyNotFoundException ex)
            {

                throw;
            }
            catch (Exception ex)
            {
                throw;
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

                throw;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
