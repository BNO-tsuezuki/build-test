using System;
using Newtonsoft.Json;

namespace evolib.Util
{
	public class JsonConverter
	{
		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}

}
