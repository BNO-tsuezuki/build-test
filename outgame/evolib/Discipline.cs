using System;
using System.Collections.Generic;
using System.Text;

namespace evolib
{
	public static class Discipline
	{
		public enum Level
		{
			None,
			Warning,
			Ban,
		}

		public enum RejectTarget
		{
			DiskId,
			MacAddr,
//			IpAddr,
		}

	}
}
