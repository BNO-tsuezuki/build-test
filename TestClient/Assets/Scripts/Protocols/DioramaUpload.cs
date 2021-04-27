public class DioramaUpload : Protocol<DioramaUpload, DioramaUpload.Req, DioramaUpload.Res>, IHttpRequester
{
	public string Uri
	{
		get { return "/api/Diorama/UploadVisual"; }
	}

	[System.Serializable]
	public class Req
	{
	}

	[System.Serializable]
	public class Res
	{
	}
}
