using System;
using System.Collections.Generic;
using System.Text;

using evolib.Services;

namespace evolib.Log
{
	public interface ILogObj
	{
		ILogObj AddChild(object obj);
		string Tag();
		string Serialize();
	}

	public class LogObj : ILogObj
	{
		class LogNode
		{
			public object Obj { get; set; }
			public string Name { get; set; }
		}
		List<LogNode> Series = new List<LogNode>();
		
		public ILogObj AddChild(object obj)
		{
			Series.Add(new LogNode
			{
				Obj = obj,
				Name = obj.GetType().Name,
			});

			return this;
		}

		public string Tag()
		{
			if (Series.Count == 0) return null;
			var tag = Series[0].Name;
			for( int i=1; i<Series.Count; i++)
			{
				tag += $".{Series[i].Name}";
			}
			return tag;
		}

		public string Serialize()
		{
			string ret = null;
			for (int i = Series.Count - 1; 0 <= i; i--)
			{
				var node = Series[i];
				var tmp = Newtonsoft.Json.JsonConvert.SerializeObject(node.Obj);
				if (ret != null)
				{
					tmp = tmp.Insert(tmp.LastIndexOf('}'), $",{ret}");
				} 
				ret = $"\"{node.Name}\":{tmp}";
			}

			return ret;
		}
	}
}
