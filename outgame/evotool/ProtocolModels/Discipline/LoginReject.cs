using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;


namespace evotool.ProtocolModels.Discipline
{
	public class LoginReject
	{
		public class Info
		{
			public evolib.Discipline.RejectTarget target { get; set; }

			public string value { get; set; }
		}

		public class Request
		{
			[Required]
			public List<Info> list { get; set; }

			[Required]
			public bool? reset { get; set; }
		}

		public class Response
		{
			public List<Info> list { get; set; }

			public bool reset { get; set; }
		}
	}
}
