using System;
using System.Collections.Generic;
using System.Text;

namespace TestSituationSetter.State.MainStates
{
	public class CreateAccounts : IceMilkTea.Core.ImtStateMachine<Main, Main.EventTrigger>.State
	{
		protected internal override void Enter()
		{
			ConsoleWriter.Info("This mode doesn't work.");
		}

		protected internal override void Update()
		{
			StateMachine.SendEvent(Main.EventTrigger.Next);
		}
	}
}
