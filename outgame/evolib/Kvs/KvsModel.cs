using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using StackExchange.Redis;


namespace evolib.Kvs
{
	public abstract class KvsModel
	{
		public string Key
		{
			get { return GetType().Name + ":" + Id; }
		}

		string Id { get; set; }

		KvsType KvsType { get; set; }

		protected IDatabase Database
		{
			get
			{
				return Kvs.Get(KvsType);
			}
		}
			


		public KvsModel( string id )
		{
			Id = id;

			var attribute = GetType().GetCustomAttribute<KvsModelAttribute>();
			KvsType = attribute.kvsType;
		}


		public async Task DeleteAsync()
		{
			await Database.KeyDeleteAsync(Key);
		}

		public async Task<bool> ExistsAsync()
		{
			return await Database.KeyExistsAsync(Key);
		}

		protected async Task TransactionExecuteAsync(Action<ITransaction> action)
		{
			var tran = Database.CreateTransaction();
			action(tran);

			await tran.ExecuteAsync();
		}
	}
}
