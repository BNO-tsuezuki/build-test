using System;
using System.Collections.Generic;
using System.Text;

namespace TestSituationSetter.State.MainStates
{
	public class Start : IceMilkTea.Core.ImtStateMachine<Main, Main.EventTrigger>.State
	{
		protected internal override void Enter()
		{
			GlobalSettings.FileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(
				System.Reflection.Assembly.GetExecutingAssembly().Location
			);
			var info = GlobalSettings.FileVersionInfo;

			ConsoleWriter.Info(
				$"{info.ProductName} ver.{info.ProductVersion} {info.CompanyName}"
			);

			Console.ResetColor();

			StateMachine.SendEvent(Main.EventTrigger.Next);
		}

	}
}
