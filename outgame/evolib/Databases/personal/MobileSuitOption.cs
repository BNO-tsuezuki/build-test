using System;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace evolib.Databases.personal
{
	public class MobileSuitOption
	{
		// Key 1/2
		public long playerId { get; set; }

		// Key 2/2
		[MaxLength(32)]
		public string mobileSuitId { get; set; }

		public int value0 { get; set; }
		public int value1 { get; set; }
		public int value2 { get; set; }
		public int value3 { get; set; }
		public int value4 { get; set; }
		public int value5 { get; set; }
		public int value6 { get; set; }
		public int value7 { get; set; }
		public int value8 { get; set; }
		public int value9 { get; set; }
		public int value10 { get; set; }
		public int value11 { get; set; }
		public int value12 { get; set; }
		public int value13 { get; set; }
		public int value14 { get; set; }
		public int value15 { get; set; }
		public int value16 { get; set; }
		public int value17 { get; set; }
		public int value18 { get; set; }
		public int value19 { get; set; }
		public int value20 { get; set; }
		public int value21 { get; set; }
		public int value22 { get; set; }
		public int value23 { get; set; }
		public int value24 { get; set; }
		public int value25 { get; set; }
		public int value26 { get; set; }
		public int value27 { get; set; }
		public int value28 { get; set; }
		public int value29 { get; set; }
		public int value30 { get; set; }
		public int value31 { get; set; }
		public int value32 { get; set; }
		public int value33 { get; set; }
		public int value34 { get; set; }
		public int value35 { get; set; }
		public int value36 { get; set; }
		public int value37 { get; set; }
		public int value38 { get; set; }
		public int value39 { get; set; }
		public int value40 { get; set; }
		public int value41 { get; set; }
		public int value42 { get; set; }
		public int value43 { get; set; }
		public int value44 { get; set; }
		public int value45 { get; set; }
		public int value46 { get; set; }
		public int value47 { get; set; }
		public int value48 { get; set; }
		public int value49 { get; set; }
		public int value50 { get; set; }
		public int value51 { get; set; }
		public int value52 { get; set; }
		public int value53 { get; set; }
		public int value54 { get; set; }
		public int value55 { get; set; }
		public int value56 { get; set; }
		public int value57 { get; set; }
		public int value58 { get; set; }
		public int value59 { get; set; }
		public int value60 { get; set; }
		public int value61 { get; set; }
		public int value62 { get; set; }
		public int value63 { get; set; }
		public int value64 { get; set; }
		public int value65 { get; set; }
		public int value66 { get; set; }
		public int value67 { get; set; }
		public int value68 { get; set; }
		public int value69 { get; set; }
		public int value70 { get; set; }
		public int value71 { get; set; }
		public int value72 { get; set; }
		public int value73 { get; set; }
		public int value74 { get; set; }
		public int value75 { get; set; }
		public int value76 { get; set; }
		public int value77 { get; set; }
		public int value78 { get; set; }
		public int value79 { get; set; }
		public int value80 { get; set; }
		public int value81 { get; set; }
		public int value82 { get; set; }
		public int value83 { get; set; }
		public int value84 { get; set; }
		public int value85 { get; set; }
		public int value86 { get; set; }
		public int value87 { get; set; }
		public int value88 { get; set; }
		public int value89 { get; set; }
		public int value90 { get; set; }
		public int value91 { get; set; }
		public int value92 { get; set; }
		public int value93 { get; set; }
		public int value94 { get; set; }
		public int value95 { get; set; }
		public int value96 { get; set; }
		public int value97 { get; set; }
		public int value98 { get; set; }
		public int value99 { get; set; }
		public int value100 { get; set; }
		public int value101 { get; set; }
		public int value102 { get; set; }
		public int value103 { get; set; }
		public int value104 { get; set; }
		public int value105 { get; set; }
		public int value106 { get; set; }
		public int value107 { get; set; }
		public int value108 { get; set; }
		public int value109 { get; set; }
		public int value110 { get; set; }
		public int value111 { get; set; }
		public int value112 { get; set; }
		public int value113 { get; set; }
		public int value114 { get; set; }
		public int value115 { get; set; }
		public int value116 { get; set; }
		public int value117 { get; set; }
		public int value118 { get; set; }
		public int value119 { get; set; }
		public int value120 { get; set; }
		public int value121 { get; set; }
		public int value122 { get; set; }
		public int value123 { get; set; }
		public int value124 { get; set; }
		public int value125 { get; set; }
		public int value126 { get; set; }
		public int value127 { get; set; }
		public int value128 { get; set; }
		public int value129 { get; set; }
		public int value130 { get; set; }
		public int value131 { get; set; }
		public int value132 { get; set; }
		public int value133 { get; set; }
		public int value134 { get; set; }
		public int value135 { get; set; }
		public int value136 { get; set; }
		public int value137 { get; set; }
		public int value138 { get; set; }
		public int value139 { get; set; }

		public enum Const
		{
			Length = 140,//valueXXが増えたら、ここも増やしてね
		}


		public int this[int idx]
		{
			set { GetType().GetProperty(string.Format("value{0}", idx)).SetValue(this, value); }
		}

		public List<int> Values()
		{
			var type = GetType();
			var ret = new List<int>();
			for (int i = 0; i < (int)Const.Length; i++)
			{
				ret.Add((int)type.GetProperty(string.Format("value{0}", i)).GetValue(this));
			}
			return ret;
		}
	}
}
