using System;
using System.Security.Cryptography;

namespace evolib.Util
{
    public static class RandGen
    {
		static RNGCryptoServiceProvider _provider = new RNGCryptoServiceProvider();

		public static UInt32 GetUInt32()
		{
			var randomBytes = new byte[UInt32ByteLen];
			_provider.GetBytes(randomBytes);

			return BitConverter.ToUInt32(randomBytes, 0);
		}

        public static int UInt32ByteLen { get { return 4; } }
    }
}
