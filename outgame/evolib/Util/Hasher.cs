using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace evolib.Util
{


	public static class Hasher
    {
		const int PBKDF2_ITERATION = 10000;
		const int PBKDF2_BYTESNUM = 64;

		public static string ToPbkdf2(string plainTxt, byte[] salt )
		{
			//var sw = new System.Diagnostics.Stopwatch();
			//sw.Start();

			var db = new Rfc2898DeriveBytes(
							plainTxt,
							salt,
							PBKDF2_ITERATION
						);

			//sw.Stop();

			return Convert.ToBase64String(db.GetBytes(PBKDF2_BYTESNUM));
		}

		public static string ToPbkdf2(string plainTxt, string salt)
		{
			return ToPbkdf2(plainTxt, Convert.FromBase64String(salt));
		}

		public static uint ToUint( string plainTxt, uint lessThan )
		{
			var sha = new SHA1CryptoServiceProvider();

			var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(plainTxt));

			uint result = 0;
			foreach (var d in hash)
			{
				result = (result * 16 + d) % lessThan;
			}

			return result;
		}

	}
}
