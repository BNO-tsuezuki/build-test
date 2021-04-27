using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestSituationSetter
{
	public class HandShakeKeeper
	{
		class Response : evoapi.ProtocolModels.HandShake.HandShake.ResponseBase
		{
			new public string pushCode { get; set; }
		}

		public string Token { get; private set; }

		HandShakeKeeper() {;}

		bool Cancelled { get; set; }
		Task HandShakeTask { get; set; }
		Task LogoutTask { get; set; }

		public async Task CancelAsync()
		{
			Cancelled = true;

			if (HandShakeTask != null)
			{
				await HandShakeTask;
			}
			if (LogoutTask != null)
			{
				await LogoutTask;
			}
		}

		public static async Task<HandShakeKeeper> CreateAsync(string account, string password,
			string authToken="", evolib.Account.Type accountType=evolib.Account.Type.Dev1)
		{
			var login = new EvoApiRequester<
						   evoapi.ProtocolModels.Auth.Login.Request,
						   evoapi.ProtocolModels.Auth.Login.Response>();
			login.request.account = account;
			login.request.password = password;
			login.request.authToken = authToken;
			login.request.accountType = accountType;
			login.request.packageVersion = new int[] { 99, };
			var resLogin = await login.PostAsync();

			if (resLogin.StatusCode != System.Net.HttpStatusCode.OK
					|| string.IsNullOrEmpty(resLogin.Payload.token))
			{
				return null;
			}

			return Create(resLogin.Payload.token);
		}

		static HandShakeKeeper Create(string token)
		{
			var instance = new HandShakeKeeper();
			instance.Token = token;

			instance.HandShakeTask = Task.Run(async () =>
			{
				var handShake = new EvoApiRequester<
					evoapi.ProtocolModels.HandShake.HandShake.Request,
					Response>();

				while (true)
				{
					var res = await handShake.PostAsync($"{token}");

					if (res.StatusCode != System.Net.HttpStatusCode.OK) break;
					if (res.Payload.pushCode == "Close") break;
					if (instance.Cancelled) break;
				}
			});

			instance.LogoutTask = Task.Run(async () =>
			{
				var logout = new EvoApiRequester<
					evoapi.ProtocolModels.Auth.Logout.Request,
					evoapi.ProtocolModels.Auth.Logout.Response>();

				while (true)
				{
					if (instance.Cancelled)
					{
						var res = await logout.PostAsync($"{token}");
						break;
					}

					await Task.Delay(1000);
				}
			});

			return instance;
		}
	}
}
