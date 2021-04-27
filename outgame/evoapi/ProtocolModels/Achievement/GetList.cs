using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace evoapi.ProtocolModels.Achievement
{
	public class GetList
	{
		public class Request
		{
		}

		public class Response
		{
			public class Achievement
			{
				public string achievementId { get; set; }
				public int value { get; set; }
				public int achievementValue { get; set; }
				public bool obtained { get; set; }
				public DateTime obtainedDate { get; set; }
			}

			public List<Achievement> list { get; set; }
		}
	}
}
