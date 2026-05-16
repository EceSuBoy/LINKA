using System.Security.Cryptography;

namespace Linka.IdentityServer.Tools
{
    public class JwtTokenDefaults
    {
        public const string ValidAudience = "http://localhost";
        public const string ValidIssuer = "http://localhost";
        public const string Key = "Linka..01020304050Asp.NetCore8.0.28*/+-";
        public const int Expire = 60;
    }
}
