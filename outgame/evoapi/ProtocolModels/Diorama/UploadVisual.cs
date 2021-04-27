using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace evoapi.ProtocolModels.Diorama
{
    public class UploadVisual
	{
		public class Request
		{
			[Required]
			[StringLength(64, MinimumLength = 1)]
			public string hashCode { get; set; }

			[Required]
			//[MaxLength((int)evolib.Diorama.Const.VisualLimitSize)]
			public IFormFile visual { get; set; }

			[Required]
			//[MaxLength((int)evolib.Diorama.Const.SceneDataLimitSize)]
			public IFormFile sceneData { get; set; }
		}

		public class Response
		{
		}
	}
}
