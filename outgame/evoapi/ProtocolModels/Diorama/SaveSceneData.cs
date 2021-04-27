using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace evoapi.ProtocolModels.Diorama
{
    public class SaveSceneData

	{
		public class Request
		{
			[Required]
			//[MaxLength((int)evolib.Diorama.Const.SceneDataLimitSize)]
			public IFormFile sceneData { get; set; }

			[Required]
			[Range(0, (int)evolib.Diorama.Const.SaveDataNumPerBase * (int)evolib.Diorama.Const.BaseNum - 1)]
			public int? index { get; set; }

			[Required]
			[StringLength(64, MinimumLength = 1)]
			public string hashCode { get; set; }
		}

		public class Response
		{
		}

	}
}
