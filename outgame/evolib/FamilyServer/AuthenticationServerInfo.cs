using Microsoft.Extensions.Configuration;

namespace evolib.FamilyServerInfo
{
    public static class AuthenticationServerInfo
	{
		static FamilyServerInfo info = new FamilyServerInfo();

		public static void Initialize(IConfiguration configuration)
		{
			info.Initialize(configuration.GetSection("AuthenticationServer"));
		}

		public static string Uri { get { return info.Uri;  } }
	}
}
