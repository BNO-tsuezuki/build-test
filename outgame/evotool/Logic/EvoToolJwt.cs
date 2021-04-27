namespace evotool
{
    public class EvoToolJwt : evolib.Jwt<EvoToolJwt.Payload>
    {
		public class Payload
		{
			public string accountId { get; set; }
		}
	}
}
