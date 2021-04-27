using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace evoapi.ProtocolModels.Diorama
{
    public class GetSavedSceneDataUrls
	{
		public class Request
		{
		}

		public class Response
		{
			public class Url
			{
				public string sceneDataUrl { get; set; }
				public int index { get; set; }
				public string hashCode { get; set; }
			}

			public List<Url> urls { get; set; }
		}

	}
}
