using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authentication
{
    public class JwtSettings
    {
        public const string SectionName = "Jwt";
        public required string Key { get; init; }
        public int Expires { get; init; }
        public required string Issuer { get; init; }
        public required string Audience { get; init; }
    }
}
