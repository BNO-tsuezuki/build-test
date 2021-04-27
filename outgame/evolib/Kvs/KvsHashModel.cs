using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using StackExchange.Redis;


namespace evolib.Kvs
{
	public class ValidateChecker<T> : DispatchProxy
	{
		T target { get; set; }

		public Dictionary<string, string> _validateProperties = new Dictionary<string, string>();

		public static T Create(T target)
		{
			var proxy = Create<T, ValidateChecker<T>>();

			(proxy as ValidateChecker<T>).target = target;

			return proxy;
		}

		protected override object Invoke(MethodInfo targetMethod, object[] args)
		{
			try
			{
				var result = targetMethod.Invoke(target, args);

				foreach (var prop in typeof(T).GetProperties())
				{
					if (prop.SetMethod == targetMethod)
					{
						_validateProperties[prop.Name] = Convert.ToString(args[0]);
						break;
					}
				}

				//var setterPrefix = "set_";
				//if( targetMethod.Name.StartsWith(setterPrefix) )
				//{
				//	var key = targetMethod.Name.Substring(setterPrefix.Length);
				//	_validateProperties[key] = args[0].ToString();
				//}

				return result;
			}
			catch (Exception ex) when (ex is TargetInvocationException)
			{
				throw ex;
			}
		}
	}



	public abstract class KvsHashModel<INFC, IMPL> : KvsModel
		where IMPL : INFC, new()
	{
		public INFC Model { get; private set; }

		ValidateChecker<INFC> ValidateChecker { get; set; }

		IMPL Impl { get; set; }


		public KvsHashModel(string id):base(id)
		{
			Impl = new IMPL();
			Model = ValidateChecker<INFC>.Create(Impl);
			ValidateChecker = Model as ValidateChecker<INFC>;
		}

		List<HashEntry> UpdateFields
		{
			get
			{
				var list = new List<HashEntry>();

				var vp = ValidateChecker._validateProperties;
				
				foreach (var i in vp)
				{
					list.Add(new HashEntry(i.Key, i.Value));
				}

				return list;
			}
		}

		public async Task SaveAsync(TimeSpan? timeSpan = null )
		{
			var updateFields = UpdateFields;

			if( updateFields.Count == 0)
			{
				if( timeSpan.HasValue)
				{
					await Database.KeyExpireAsync(Key, timeSpan);
				}
				else
				{

				}
			}
			else
			{
				if(timeSpan.HasValue)
				{
					await TransactionExecuteAsync((tran) =>
					{
						tran.HashSetAsync(Key, updateFields.ToArray());
						tran.KeyExpireAsync(Key, timeSpan);
					});
				}
				else
				{
					await Database.HashSetAsync(Key, updateFields.ToArray());
				}
			}
		}

		public async Task<bool> FetchAsync()
		{
			var list = await Database.HashGetAllAsync(Key);

			if (list.Length == 0) return false;

			var t = typeof(IMPL);

			foreach (var i in list)
			{
				var prop = t.GetProperty(i.Name);
				if (null == prop) continue;

				prop.SetMethod.Invoke(Impl, new object[] { Util.TypeConverter.Convert(prop.PropertyType, i.Value) });
			}

			//Fetchによるvalidate状態はクリアする
			//(Fetch後に一部のpropertyを入れなおして、Saveしようとすると入れなおしたproperty以外も更新してしまう)
			//(別に同じ値のはずなので問題はないけど、無駄ではあるため)
			ValidateChecker._validateProperties.Clear();

			return true;
		}

	}
}
