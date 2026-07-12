using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IRefreshTokenRepository
    {
        string GenerateRefreshToken();
        Task<RefreshToken> GetByTokenasync(string token);
        Task RotateTokenAsync(string oldToken, string newToken, string userId);
        Task StoreRefreshToken(ApplicationUser user, string refreshToken);
        Task<bool> RevokeToken(string userId, string token);
    }
}
