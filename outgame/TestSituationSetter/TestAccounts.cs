using System;
using System.Collections.Generic;
using System.Text;

namespace TestSituationSetter
{
	public static class TestAccounts
	{
		public class Desc
		{
			public string Account { get; set; }
			public string Password { get; set; }
			public string Name { get; set; }
		}

		public static int MaxActorsNum { get { return 500; } }

		public static IEnumerable<Desc> Actors()
		{
			for ( var i=0; i<MaxActorsNum; i++)
			{
				var prefix = $"testactor{i:D04}";

				yield return new Desc
				{
					Account = $"{prefix}@bandainamco-ol.co.jp",
					Password = $"{prefix}",
					Name = $"{prefix}",
				};
			}
		}


		public static string DsAccount { get { return "dsfortss@bandainamco-ol.co.jp"; } }
		public static string DsPassword { get { return "abcd1234"; } }
	}
}
