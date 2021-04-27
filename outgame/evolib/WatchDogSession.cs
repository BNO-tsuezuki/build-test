using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

using evolib.Kvs;
using evolib.Kvs.Models;
using evolib.Log;


namespace evolib
{
	public class WatchDogSession
	{
		object LockObj = new object();

		class KeepSessions
		{
			public Dictionary<string, int> Gettings = new Dictionary<string, int>();
			public Dictionary<string, int> Temp = new Dictionary<string, int>();
		}

		KeepSessions Current { get; set; }
		KeepSessions Uncurrent { get; set; }
		string Prefix { get; set; }

		public Func<string, bool> Alive { get; private set; }

		public Dictionary<string, int> ClientCounts { get; private set; }

		public WatchDogSession(MatchingArea matchingArea)
		{
			Current = new KeepSessions();
			Uncurrent = new KeepSessions();

			var type = typeof(Session);
			Prefix = type.Name + ":";

			Alive = (sessionId) =>
			{
				var key = Prefix + sessionId;
				lock (LockObj)
				{
					return (Current.Gettings.ContainsKey(key) || Current.Temp.ContainsKey(key));
				}
			};

			ClientCounts = new Dictionary<string, int>();

			Task.Run(async () =>
			{
				while (true)
				{
					try
					{
						var e = Kvs.Kvs.CreateKeysEnumrator<Session>(10000, matchingArea.ToString());

						while (true)
						{
							await Kvs.Kvs.ScanKeys(e);
							await Task.Delay(1500);

							if (e.Completed) break;
						}

						var clientCnts = new Dictionary<string, int>();
						var prefix = $"{e.ScanKey}{Session.PrefixClient}:";
						var prefixLen = prefix.Length;
						foreach( var k in e.Keys)
						{
							Uncurrent.Gettings[k] = 0;

							if (k.StartsWith(prefix))
							{
								var idx = k.IndexOf(':', prefixLen);
								if (0 <= idx)
								{
									// cc:country code
									var cc = k.Substring(prefixLen, idx - prefixLen);

									int cnt;
									clientCnts.TryGetValue(cc, out cnt);
									clientCnts[cc] = cnt + 1;
								}
							}
						}
						ClientCounts = clientCnts;

						lock (LockObj)
						{
							var tmp = Current;
							Current = Uncurrent;
							Uncurrent = tmp;

							foreach (var s in Uncurrent.Temp.Keys)
							{
								Current.Gettings[s] = 2;
							}

							Uncurrent.Gettings.Clear();
							Uncurrent.Temp.Clear();
						}
					}
					catch( Exception ex)
					{
						Logger.Logging(
							new LogObj().AddChild(new LogModels.ErrorReport
							{
								Msg = "WatchDog Session Error.",
								Exception = ex.ToString(),
							})
						);
					}
				}
			});
		}

		public void TemporaryAdd(string sessionId)
		{
			lock (LockObj)
			{
				Current.Temp[Prefix + sessionId] = 1;
			}
		}
	}
}
