using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace evolib.Kvs.Models
{
	//[KvsModel(kvsType = KvsType.Personal)]
	[KvsModel(kvsType = KvsType.Common)]
	public class ConnectionQueue : KvsModel
	{
		public ConnectionQueue(string id) : base(id) { }
		
		public abstract class Data : Util.JsonConverter
		{
			public string dataCode { get { return GetType().Name; } }
		}

		class TmpData : Data
		{
			new public string dataCode { get; set; }
		}

		public async Task EnqueueAsync(Data data)
		{
			await TransactionExecuteAsync((tran) =>
			{
				tran.ListRightPushAsync(Key, data.ToString());
				tran.KeyExpireAsync(Key, TimeSpan.FromMinutes(60));
			});
		}

		Dictionary<string, Action<string>> ProcessingMap = new Dictionary<string, Action<string>>();
		Dictionary<string, Func<string, Task>> ProcessingAsyncMap = new Dictionary<string, Func<string, Task>>();

		public void RegisterProcessing<T>( Action<T> processing) where T:Data
		{
			var mapKey = typeof(T).Name;
			ProcessingMap[mapKey] = (json) =>
			{
				processing(JsonConvert.DeserializeObject<T>(json));
			};
			ProcessingAsyncMap.Remove(mapKey);
		}
		public void RegisterProcessing<T>(Func<T, Task> processingAsync) where T : Data
		{
			var mapKey = typeof(T).Name;
			ProcessingAsyncMap[mapKey] = async (json) =>
			{
				await processingAsync(JsonConvert.DeserializeObject<T>(json));
			};
			ProcessingMap.Remove(mapKey);
		}


		public async Task DequeueAndProcessingAsync()
		{
			var json = await Database.ListLeftPopAsync(Key);
			if (!json.HasValue) return;

			var dataCode = JsonConvert.DeserializeObject<TmpData>(json).dataCode;
			if( ProcessingMap.ContainsKey(dataCode) )
			{
				ProcessingMap[dataCode](json);
				return;
			}

			if (ProcessingAsyncMap.ContainsKey(dataCode))
			{
				await ProcessingAsyncMap[dataCode](json);
				return;
			}

			throw new Exception();
		}
	}
}
