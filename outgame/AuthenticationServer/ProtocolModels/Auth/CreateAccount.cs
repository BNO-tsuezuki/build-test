using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.ProtocolModels.Auth
{
    public class CreateAccount : HttpRequester<CreateAccount.Request, CreateAccount.Response>
	{
		public class Request
		{
			[Required]
			[DataType(DataType.EmailAddress)]
			[EmailAddress]
			public string account { get; set; }

			[Required]
			[StringLength(48, MinimumLength = 8)]
			[RegularExpression("^[a-zA-Z0-9_-]+$")]// '-'の エスケープいらないみたい
			public string password { get; set; }


			[Required]
			[StringLength(48, MinimumLength = 1)]
			[RegularExpression("^[a-zA-Z0-9０-９ぁ-んァ-ヶー一-龠_-]+$")]
			public string nickname { get; set; }
		}

		public class Response
		{
			public enum ResultCode
			{
				Ok,
				AlreadyExists,
			}

			public ResultCode resultCode { get; set; }

			public string account { get; set; }
			public int permission { get; set; }
			public string nickname { get; set; }
			public evolib.HostType hostType { get; set; }
		}
	}
}
