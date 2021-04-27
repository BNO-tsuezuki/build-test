using System.ComponentModel.DataAnnotations;

using System.Collections.Generic;


namespace evoapi.ProtocolModels.Option
{
    public class GetOptions
	{
		public class Request
		{
			
		}

		public class Response
		{
			public class AppOption
			{
				public int category { get; set; }
				public List<int> values { get; set; }
			}
			public class MobileSuitOption
			{
				public string mobileSuitId { get; set; }
				public List<int> values { get; set; }
			}

			public List<AppOption> appOptions { get; set; }
			public List<MobileSuitOption> mobileSuitOptions { get; set; }
		}
	}
}
