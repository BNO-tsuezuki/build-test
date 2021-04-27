using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AnalysisLogProcess
{
    public static class MyExtensions
    {
        private static byte[] pepper = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("HASH_PEPPER"));

        private static IDictionary<string, string> cache = new Dictionary<string, string>();

        public static string Hash(this long playerId)
        {
            return Hash(playerId.ToString());
        }

        public static string Hash(this string playerId)
        {
            // todo: lock?

            if (cache.ContainsKey(playerId))
            {
                return cache[playerId];
            }
            else
            {
                var hash = HashByPBKDF2(playerId);

                cache[playerId] = hash;

                return hash;
            }
        }

        private static string HashByPBKDF2(string value)
        {
            return Convert.ToBase64String(new Rfc2898DeriveBytes(value, pepper, 1).GetBytes(32));
        }
    }
}
