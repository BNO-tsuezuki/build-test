using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.Chat
{
    public class Whisper
	{
		public class Request
		{
			[Required]
			public long? playerId { get; set; }

			[Required]
			[StringLength((int)evolib.Chat.Const.MaxStringLength, MinimumLength = 1)]
			public string text { get; set; }
		}

		public class Response
		{

		}
	}
}
