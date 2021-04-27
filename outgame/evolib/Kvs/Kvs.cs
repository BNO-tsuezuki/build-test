using System;
using System.Collections.Generic;
using StackExchange.Redis;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using LazyCon = System.Lazy<StackExchange.Redis.ConnectionMultiplexer>;

namespace evolib.Kvs
{
	public static class Kvs
	{
		static Dictionary<KvsType, LazyCon> _kvsMap = new Dictionary<KvsType, LazyCon>();
		//static Dictionary<KvsType, List<ConnectionMultiplexer>> _kvsMap = new Dictionary<KvsType, List<ConnectionMultiplexer>>();

		public static void Initialize(Dictionary<string, List<string>> connectionConfigListMap)
		{
			foreach (KvsType kt in Enum.GetValues(typeof(KvsType)))
			{
				var key = kt.ToString();

				if (!connectionConfigListMap.ContainsKey(key)) continue;

				var list = new List<LazyCon>();
				//var list = new List<ConnectionMultiplexer>();
				connectionConfigListMap[key].ForEach(
					(config) =>
					{
						list.Add(new LazyCon(() => ConnectionMultiplexer.Connect(config)));
						//list.Add(ConnectionMultiplexer.Connect(config));
					}
				);

				if (list.Count == 0) continue;

				_kvsMap[kt] = list[0];
			}
		}

		public static IDatabase Get(KvsType kt)
		{
			return _kvsMap[kt].Value.GetDatabase();
		}



		public interface IKeysEnumrator
		{
			IReadOnlyList<string> Keys { get;}
			bool Completed { get;  }
			string ScanKey { get; }
		}

		class KeysEnumrator : IKeysEnumrator 
		{
			public IReadOnlyList<string> Keys { get { return KeysImpl; } }
			public bool Completed
			{
				get { return (EndPoints.Count == 0 && Cursor == "0"); }
			}


			public List<string> KeysImpl { get; set; }
			public string Cursor { get; set; }
			public string ScanKey { get; set; }
			public KvsType KvsType { get; set; }
			public int PageSize { get; set; }
			public List<System.Net.EndPoint> EndPoints { get; set; }
		}

		public static IKeysEnumrator CreateKeysEnumrator<T>(int pageSize, string additionalKey ) where T : KvsModel
		{
			var type = typeof(T);
			var attribute = type.GetCustomAttributes(typeof(KvsModelAttribute), false)[0];

			if (string.IsNullOrEmpty(additionalKey))
			{
				additionalKey = "";
			}
			else
			{
				additionalKey = $"{additionalKey}:";
			}
			

			var keysEnumrator = new KeysEnumrator
			{
				KeysImpl = new List<string>(),
				Cursor = "0",
				ScanKey = $"{type.Name}:{additionalKey}",
				KvsType = ((KvsModelAttribute)attribute).kvsType,
				PageSize = pageSize,
				EndPoints = new List<System.Net.EndPoint>(),
			};

			var con = _kvsMap[keysEnumrator.KvsType].Value;
			foreach (var ep in con.GetEndPoints(true))
			{
				keysEnumrator.EndPoints.Add(ep);
			}

			return keysEnumrator;
		}

		public static async Task ScanKeys(IKeysEnumrator keysEnumrator)
		{
			if (keysEnumrator.Completed) return;
			
			var impl = (KeysEnumrator)keysEnumrator;


			var con = _kvsMap[impl.KvsType].Value;

			var server = con.GetServer(impl.EndPoints[0]);

			var result = await server.ExecuteAsync("SCAN", impl.Cursor, "MATCH", $"{impl.ScanKey}*", "COUNT", impl.PageSize);

			if ( !result.IsNull)
			{
				var dic = result.ToDictionary();

				impl.Cursor = (string)((RedisResult[])result)[0];

				var values = (RedisResult[])((RedisResult[])result)[1];
				foreach( var v in values )
				{
					impl.KeysImpl.Add((string)v);
				}

				if (impl.Cursor == "0")
				{
					impl.EndPoints.RemoveAt(0);
				}
			}
		}
	}
}
