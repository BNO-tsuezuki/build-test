using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.LocalAccount
{
    public class Create
    {
		public class Request : AuthenticationServer.ProtocolModels.Auth.CreateAccount.Request
		{
		}

		public class Response
		{
			public string account { get; set; }
			public string nickname { get; set; }
			public int permission { get; set; }
		}
	}
}
