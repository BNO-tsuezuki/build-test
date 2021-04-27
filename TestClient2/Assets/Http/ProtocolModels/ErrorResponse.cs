[System.Serializable]
public class ErroResponse
{
	[System.Serializable]
	public class Error
	{
		public int level;
		public string code;
	}

	public Error error;
}
