using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evoapi.ProtocolModels.Option
{
    public class SetMobileSuitOptions
	{
		public class Option
		{
			[Required]
			public string mobileSuitId { get; set; }
			[Required]
			[MaxLength((int)evolib.Databases.personal.MobileSuitOption.Const.Length)]
			public int[] values { get; set; }
		}

		public class Request
		{
			[Required]
			public List<Option> options { get; set; }
		}

		public class Response
		{

		}
	}
}
