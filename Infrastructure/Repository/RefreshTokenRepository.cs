using Application.Contracts;
using Domain.Model;
using Google.Apis.Auth.OAuth2;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;

        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<RefreshToken> GetByTokenasync(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token) ?? throw new KeyNotFoundException("Refresh token not found");
        }

        public async Task<bool> RevokeToken(string userId, string token)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == token && rt.UserId == userId && !rt.IsRevoked) ?? throw new KeyNotFoundException("Refresh token not found");

            refreshToken.IsRevoked = true;
            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
                return true;
            return false;
        }

        public async Task RotateTokenAsync(string oldToken, string newToken, string userId)
        {
            var oldRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == oldToken && rt.UserId == userId) ?? throw new KeyNotFoundException("Refresh token not found");

            oldRefreshToken.IsRevoked = true;
            
            var newRefreshToken = new RefreshToken
            {
                Token = newToken,
                UserId = userId,
                Created = DateTime.Now,
                Expires = DateTime.Now.AddDays(1)
            };

            await _context.RefreshTokens.AddAsync(newRefreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task StoreRefreshToken(ApplicationUser user, string refreshToken)
        {
            var token = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                Created = DateTime.Now,
                Expires = DateTime.Now.AddDays(1)
            };

            await _context.RefreshTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }
    }
}
