using Application.Contracts;
using Domain.Model;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ActivateDevice(string userId, string deviceToken)
        {
            var userDevice = await _context.UserDevices.FirstOrDefaultAsync(u => u.UserId == userId && u.DeviceToken == deviceToken) ?? throw new KeyNotFoundException("Device not found");
            
            if (userDevice.IsActive)
                return true;

            userDevice.IsActive = true;
            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return true;
            return false;
        }

        public async Task<bool> DeactivateDevice(string userId, string deviceToken)
        {
            var userDevice = await _context.UserDevices.FirstOrDefaultAsync(u => u.UserId == userId && u.DeviceToken == deviceToken) ?? throw new KeyNotFoundException("Device not found");
            userDevice.IsActive = false;
            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return true;
            return false;
        }

        public async Task<bool> DeviceExists(string userId, string deviceToken)
        {
            return await _context.UserDevices.AnyAsync(u => u.UserId == userId && u.DeviceToken == deviceToken);
        }

        public async Task<List<UserDevice>> GetUserDevice(string userId)
        {
            //return await _context.UserDevices.FirstOrDefaultAsync(d => d.UserId == userId && d.IsActive == true) ?? throw new KeyNotFoundException("Device not found");
            return await _context.UserDevices.Where(d => d.UserId == userId && d.IsActive == true).ToListAsync();
        }

        public async Task<bool> RegisterDevice(UserDevice userDevice)
        {
            await _context.UserDevices.AddAsync(userDevice);
            var result = await _context.SaveChangesAsync();

            if (result > 0)
                return true;
            return false;
        }
    }
}
