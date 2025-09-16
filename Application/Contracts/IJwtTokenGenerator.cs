using Domain.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateToken(string userId, string name, string email);
        string GenerateRefreshToken();
        Task<IdentityResult> StoreRefreshToken(ApplicationUser user, string refreshToken);
    }
}
