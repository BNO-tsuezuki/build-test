using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace evoapi.ProtocolModels.HandShake
{
	public class HandShake
	{
		public static int NextResponseSeconds { get { return 15; } }

		public class Request
		{

		}

		public abstract class ResponseBase
		{
			public string pushCode { get { return GetType().DeclaringType.Name; } }
			public int nextResponseSeconds { get { return NextResponseSeconds; } }
		

			public int[] limitPackageVersionLogin { get; set; }
			public int[] limitPackageVersionMatchmake { get; set; }
			public int[] masterDataVersion { get; set; }
			public bool enabledMatchmake { get; set; }
			public List<string> opsNoticeCodes { get; set; }
			public List<string> disabledMobileSuits { get; set; }
		}
	}
}
