using System;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace evolib.Databases.personal
{
	public class AppOption
	{
		// Key 1/2
		public long playerId { get; set; }

		// Key 2/2
		public int category { get; set; }


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
		public enum Const
		{
			MaxCategory = 7,
			Length = 50,//valueXXが増えたら、ここも増やしてね
		}

		public int this[int idx]
		{
			set { GetType().GetProperty(string.Format("value{0}", idx)).SetValue(this, value); }
		}

		public List<int> Values()
		{
			var type = GetType();
			var ret = new List<int>();
			for(int i = 0; i < (int)Const.Length; i++)
			{
				ret.Add((int)type.GetProperty(string.Format("value{0}", i)).GetValue(this));
			}
			return ret;
		}
	}
}
