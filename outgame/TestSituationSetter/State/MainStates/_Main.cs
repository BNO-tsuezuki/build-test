using System;
using System.Collections.Generic;
using System.Text;

namespace TestSituationSetter.State.MainStates
{
	public class Main
	{
		IceMilkTea.Core.ImtStateMachine<Main,EventTrigger> StateMachine { get; set; }

		private Main() { }

		public void Update()
		{
			StateMachine.Update();
		}

		public enum EventTrigger
		{
			Next,
			Continue,

			MODE_SelectEnv,
			MODE_MyAccount,
			MODE_GiveMeFriends,
			MODE_ApproveFriendRequests,
			MODE_RecentPlayers,
			MODE_CreateActors,
			MODE_SessionKeep,
			// --------
			MODE_CreateAccounts,
		}

		public static Main Create()
		{
			var main = new Main();
			var stateMachine = new IceMilkTea.Core.ImtStateMachine<Main, EventTrigger>(main);

			stateMachine.AddTransition<Start, Initial>(EventTrigger.Next);

			stateMachine.AddTransition<Initial, SelectEnv>(EventTrigger.Next);

			stateMachine.AddTransition<SelectEnv, MyAccount>(EventTrigger.Next);

			stateMachine.AddTransition<SelectMode, SelectMode>(EventTrigger.Continue);
			stateMachine.AddTransition<SelectMode, SelectEnv>(EventTrigger.MODE_SelectEnv);
			stateMachine.AddTransition<SelectMode, MyAccount>(EventTrigger.MODE_MyAccount);
			stateMachine.AddTransition<SelectMode, GiveMeFriends>(EventTrigger.MODE_GiveMeFriends);
			stateMachine.AddTransition<SelectMode, ApproveFriendRequests>(EventTrigger.MODE_ApproveFriendRequests);
			stateMachine.AddTransition<SelectMode, RecentPlayers>(EventTrigger.MODE_RecentPlayers);
			stateMachine.AddTransition<SelectMode, CreateActors>(EventTrigger.MODE_CreateActors);
			stateMachine.AddTransition<SelectMode, SessionKeep>(EventTrigger.MODE_SessionKeep);
			stateMachine.AddTransition<SelectMode, CreateAccounts>(EventTrigger.MODE_CreateAccounts);


			stateMachine.AddTransition<SelectEnv, SelectEnv>(EventTrigger.Continue);

			stateMachine.AddTransition<MyAccount, SelectMode>(EventTrigger.Next);
			stateMachine.AddTransition<MyAccount, SelectEnv>(EventTrigger.Continue);

			stateMachine.AddTransition<GiveMeFriends, SelectMode>(EventTrigger.Next);

			stateMachine.AddTransition<ApproveFriendRequests, SelectMode>(EventTrigger.Next);

			stateMachine.AddTransition<RecentPlayers, SelectMode>(EventTrigger.Next);

			stateMachine.AddTransition<CreateActors, SelectMode>(EventTrigger.Next);

			stateMachine.AddTransition<SessionKeep, SelectMode>(EventTrigger.Next);

			stateMachine.AddTransition<CreateAccounts, SelectMode>(EventTrigger.Next);


			stateMachine.SetStartState<Start>();

			main.StateMachine = stateMachine;
			return main;
		}
	}
}
