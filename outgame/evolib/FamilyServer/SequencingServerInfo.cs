using Microsoft.Extensions.Configuration;

namespace evolib.FamilyServerInfo
{
    public static class SequencingServerInfo
	{
		static FamilyServerInfo info = new FamilyServerInfo();

		public static void Initialize(IConfiguration configuration)
		{
			info.Initialize(configuration.GetSection("SequencingServer"));
		}

		public static string Uri { get { return info.Uri;  } }
	}
}
