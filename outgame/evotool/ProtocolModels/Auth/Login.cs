using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.Auth
{
    public class Login
    {
		public class Request : AuthenticationServer.ProtocolModels.Auth.Login.Request
		{
		}

		public class Response
		{
			public string token { get; set; }
		}
	}
}
