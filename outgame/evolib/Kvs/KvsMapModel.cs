using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace evolib.Kvs
{
    public class KvsMapModel<FIELD,VALUE> : KvsModel where VALUE : class
    {
		public class Pair
		{
			public FIELD field { get; set; }
			public VALUE value { get; set; }
		}

		public KvsMapModel(string id) : base(id)
		{
		}

		public async Task SetFieldAsync(FIELD field, VALUE value )
		{
			await Database.HashSetAsync(Key, field.ToString(), JsonConvert.SerializeObject(value));
		}

		public async Task RemoveFieldAsync(FIELD field)
		{
			await Database.HashDeleteAsync(Key, field.ToString());
		}

		public async Task<bool> ExistsFieldAsync( FIELD field )
		{
			return await Database.HashExistsAsync(Key, field.ToString());
		}

		public async Task<VALUE> GetFieldAsync(FIELD field)
		{
			var value = await Database.HashGetAsync(Key, field.ToString());
			if (!value.HasValue) return null;

			return JsonConvert.DeserializeObject<VALUE>(value);
		}

		public async Task<List<Pair>> GetAllAsync()
		{
			var src = await Database.HashGetAllAsync(Key);

			var dst = new List<Pair>();

			foreach (var i in src)
			{
				var pair = new Pair();
				pair.field = (FIELD)Util.TypeConverter.Convert(typeof(FIELD), i.Name);
				pair.value = JsonConvert.DeserializeObject<VALUE>(i.Value);
				dst.Add(pair);
			}

			return dst;
		}
	}
}
