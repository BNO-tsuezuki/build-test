using System.ComponentModel.DataAnnotations;
using evolib;

namespace evoapi.ProtocolModels.Chat
{
    public class Say
	{
		public class Request
		{
			[Required]
			[Range((int)evolib.Chat.Type.PremadeGroup, (int)evolib.Chat.Type.BattleMatch)]
			public evolib.Chat.Type? type { get; set; }

			[Required]
			[StringLength((int)evolib.Chat.Const.MaxStringLength, MinimumLength = 1)]
			public string text { get; set; }
		}

		public class Response
		{

		}
	}
}
