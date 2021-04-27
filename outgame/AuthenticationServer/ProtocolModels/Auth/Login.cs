using System.ComponentModel.DataAnnotations;

namespace AuthenticationServer.ProtocolModels.Auth
{
    public class Login : HttpRequester<Login.Request, Login.Response>
	{
		public class Request
		{
			[Required]
			[StringLength(48, MinimumLength = 3)]
			public string account { get; set; }

			[Required]
			[StringLength(48, MinimumLength = 3)]
			public string password { get; set; }
		}

		public class Response
		{
			public enum ResultCode
			{
				Ok,
				Ng,
			}

			public ResultCode resultCode { get; set; }
			public string account { get; set; }
			public int permission { get; set; }
			public string nickname { get; set; }
			public evolib.HostType hostType { get; set; }
		}
	}
}
