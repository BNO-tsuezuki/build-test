using System;
using System.ComponentModel.DataAnnotations;

namespace evotool.ProtocolModels.Discipline
{
	public class DisciplineInfo
	{
		[Required]
		public long playerId { get; set; }

		[Required]
		public evolib.Discipline.Level level { get; set; }

		[StringLength(32, MinimumLength = 0)]
		public string title { get; set; }

		[StringLength(256, MinimumLength = 0)]
		public string text { get; set; }

		public DateTime expirationDate { get; set; }
	}


	public class Discipline
	{
		public class Request : DisciplineInfo
		{
		}

		public class Response : DisciplineInfo
		{
		}
	}
}
