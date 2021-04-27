using System;
using Newtonsoft.Json;

namespace evolib.Util
{
    public static class TypeConverter
    {
		public static object Convert( Type type, string value )
		{
			if (type.IsEnum)
			{
				return Enum.Parse( type, value);
			}


			if( typeof(JsonConverter).IsAssignableFrom(type) )
			{
				return JsonConvert.DeserializeObject(value, type);
			}


			return System.Convert.ChangeType(value, type, System.Globalization.CultureInfo.InvariantCulture);
		}

    }
}
