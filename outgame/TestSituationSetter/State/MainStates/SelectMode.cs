using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TestSituationSetter.State.MainStates
{
	public class SelectMode : IceMilkTea.Core.ImtStateMachine<Main, Main.EventTrigger>.State
	{
		protected internal override void Enter()
		{
			Task.Run(async () =>
			{
				ConsoleWriter.Info($"[env]: {GlobalSettings.TargetEnv}");
				ConsoleWriter.Info($"[account]: {GlobalSettings.MyAccount}");
				ConsoleWriter.Prompt("Input [mode] > ");
				var input = await InputPrompt.Create();
				var mode = $"MODE_{input}";

				foreach (Main.EventTrigger e in Enum.GetValues(typeof(Main.EventTrigger)))
				{
					if (Enum.GetName(typeof(Main.EventTrigger), e).ToLower() == mode.ToLower())
					{
						StateMachine.SendEvent(e);
						ConsoleWriter.Succeeded($"ok.");
						return;
					}
				}

				ConsoleWriter.Error($"{input}: mode not found.");
				StateMachine.SendEvent(Main.EventTrigger.Continue);
			});
		}
	}
}
