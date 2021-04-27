using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evolib.Databases.common1
{
	public class LoginReject
	{
		// key1
		public Discipline.RejectTarget target { get; set; }

		// key2
		public string value { get; set; }
	}
}
