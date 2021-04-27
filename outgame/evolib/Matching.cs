using System;
using System.Collections.Generic;
using System.Text;

namespace evolib
{
	public enum MatchingArea
	{
		Unknown = -1,

		Asia = 0,
		NorthAmerica,
	}

	public static class MinMatchmakeEntriedPlayersNumber
	{
		public static int Pop(Databases.common2.GenericData rec)
		{
			if (rec == null) return 120;

			return int.Parse(rec.data1);
		}

		public static void Push(Databases.common2.GenericData rec, int num)
		{
			rec.type = Databases.common2.GenericData.Type.MinMatchmakeEntriedPlayersNumber;
			rec.data1 = num.ToString();
		}
	}


	public static class Aws
	{
		public class Region
		{
			public string Code { get; }
			public string City { get; }

			public Region(string code, string city)
			{
				Code = code;
				City = city;
			}
		}
		static public Region[] Regions = new Region[]
		{
			new Region( "us-east-2",		"Ohio"),
			new Region( "us-east-1",		"N. Virginia"),
			new Region( "us-west-1",		"N. California"),
			new Region( "us-west-2",		"Oregon"),
			new Region( "af-south-1",		"Cape Town"),
			new Region( "ap-east-1",		"Hong Kong"),
			new Region( "ap-south-1",		"Mumbai"),
			new Region( "ap-northeast-3",	"Osaka-Local"),
			new Region( "ap-northeast-2",	"Seoul"),
			new Region( "ap-southeast-1",	"Singapore"),
			new Region( "ap-southeast-2",	"Sydney"),
			new Region( "ap-northeast-1",	"Tokyo"),
			new Region( "ca-central-1",		"Central"),
			new Region( "eu-central-1",		"Frankfurt"),
			new Region( "eu-west-1",		"Ireland"),
			new Region( "eu-west-2",		"London"),
			new Region( "eu-south-1",		"Milan"),
			new Region( "eu-west-3",		"Paris"),
			new Region( "eu-north-1",		"Stockholm"),
			new Region( "me-south-1",		"Bahrain"),
			new Region( "sa-east-1",		"São Paulo"),
		};
	}

}
