
namespace evotool.ProtocolModels
{
	public class Error
	{
		public Error()
		{
			error = new Payload();
		}

		public class Payload
		{
			public string msg{ get; set;}
		}

		public Payload error { get; private set; }
	}
}
