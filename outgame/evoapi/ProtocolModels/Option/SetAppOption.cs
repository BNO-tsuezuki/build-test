using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.Option
{
    public class SetAppOption
	{
		public class Request
		{
			[Required]
			[Range(0, (int)evolib.Databases.personal.AppOption.Const.MaxCategory)]
			public int? category { get; set; }
			[Required]
			[MaxLength((int)evolib.Databases.personal.AppOption.Const.Length)]
			public int[] values { get; set; }
		}

		public class Response
		{

		}
	}
}
