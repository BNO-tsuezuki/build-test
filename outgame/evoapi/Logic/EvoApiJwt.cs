namespace evoapi
{
    public class EvoApiJwt : evolib.Jwt<EvoApiJwt.Payload>
    {
		public class Payload
		{
			public string sessionId { get; set; }
			public int masterDataHash { get; set; }
		}
	}
}
