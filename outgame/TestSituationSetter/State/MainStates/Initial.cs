using System;
using System.Collections.Generic;
using System.Text;

namespace TestSituationSetter.State.MainStates
{
	public class Initial : IceMilkTea.Core.ImtStateMachine<Main, Main.EventTrigger>.State
	{
		protected internal override void Enter()
		{
			//var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			//path += $"/{Application.Info.ProductName}";
			//path += $"/settings";


			StateMachine.SendEvent(Main.EventTrigger.Next);
		}

		protected internal override void Update()
		{

		}
	}
}
