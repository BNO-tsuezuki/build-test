using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestSituationSetter.State.MainStates
{
	public class SelectEnv : IceMilkTea.Core.ImtStateMachine<Main, Main.EventTrigger>.State
	{
		protected internal override void Enter()
		{
			Task.Run(async () =>
			{
				ConsoleWriter.Prompt("Input [env] > ");
				var input = await InputPrompt.Create();
				var env = $"{input}";

				foreach (Env e in Enum.GetValues(typeof(Env)))
				{
					if (Enum.GetName(typeof(Env), e) == env)
					{
						GlobalSettings.TargetEnv = e;

						ConsoleWriter.Action("Checking version of TSS ...");

						var tssVersion = new EvoApiRequester<
							evoapi.ProtocolModels.Test.TssVersion.Request,
							evoapi.ProtocolModels.Test.TssVersion.Response>();
						var res = await tssVersion.PostAsync();
						if( res.StatusCode == System.Net.HttpStatusCode.OK &&
							new Version(res.Payload.limitVersion)
								<= new Version(GlobalSettings.FileVersionInfo.ProductVersion) )
						{
							StateMachine.SendEvent(Main.EventTrigger.Next);
							ConsoleWriter.Succeeded("ok.");
							return;
						}


						var limitVer = (res.StatusCode == System.Net.HttpStatusCode.OK)
							? res.Payload.limitVersion
							: "server Unimplemented";

						ConsoleWriter.Error($"Different version. Update to ver.({limitVer})");
						StateMachine.SendEvent(Main.EventTrigger.Continue);
						return;
					}
				}

				ConsoleWriter.Error($"{input}: env not found.");
				StateMachine.SendEvent(Main.EventTrigger.Continue);
			});
		}
	}
}
