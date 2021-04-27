using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace evolib.Kvs
{
    public class KvsQueueModel<T>: KvsModel
    {
		public KvsQueueModel(string id) : base(id)
		{
		}

		public async Task EnqueueAsync(T value)
		{
			await Database.ListRightPushAsync(Key, value.ToString());
		}

		public async Task<T> DequeueAsync()
		{
			var value = await Database.ListLeftPopAsync(Key);

			return (T)Util.TypeConverter.Convert(typeof(T), value);
		}

		public async Task<List<T>> DequeueAllAsync()
		{
			var src = await Database.ListRangeAsync(Key);
			if (0 < src.Length)
			{
				await Database.ListRemoveAsync(Key, src.Length);
			}

			var dst = new List<T>();
			foreach (var i in src)
			{
				dst.Add((T)Util.TypeConverter.Convert(typeof(T), i));
			}

			return dst;
		}
	}
}
