using Microsoft.Extensions.Configuration;

namespace evolib.FamilyServerInfo
{
	public class FamilyServerInfo
	{
		string Addr { get; set; }

		public void Initialize(IConfigurationSection configurationSection)
		{
			Addr = configurationSection.GetSection("addr").Value;

			var port = configurationSection.GetSection("port").Value;
			if (!string.IsNullOrEmpty(port))
			{
				Addr += $":{port}";
			}
		}

		public string Uri
		{
			get
			{
				return $"http://{Addr}";
			}
		}
	}
}
