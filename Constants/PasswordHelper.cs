using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Constants
{
    public static class PasswordHelper
    {
        public static string GenerateSHA256(string password)
        {
            var hash = SHA256.Create();
            var sourceBytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = hash.ComputeHash(sourceBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
