using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.Diorama
{
    public class GetUploadedVisualUrls
	{
		public class Request
		{
		}

		public class Response
		{
			public class Url
			{
				public long id { get; set; }
				public string visualUrl { get; set; }
				public string sceneDataUrl { get; set; }
				public string hashCode { get; set; }
			}

			public List<Url> urls { get; set; }
		}

	}
}
