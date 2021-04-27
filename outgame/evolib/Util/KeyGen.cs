using System;
using System.Security.Cryptography;

namespace evolib.Util
{
    public static class KeyGen
    {
		static RNGCryptoServiceProvider _provider = new RNGCryptoServiceProvider();

		public static string Get(int byteLen )
		{
			var randomBytes = new byte[byteLen];
			_provider.GetBytes(randomBytes);

			return Convert.ToBase64String(randomBytes);
		}

		public static string GetUrlSafe(int byteLen)
		{
			var ret = Get(byteLen);
			ret = ret.Replace('/', '_');
			ret = ret.Replace('+', '-');
			ret = ret.Replace("=", "");

			return ret;
		}
	}
}
