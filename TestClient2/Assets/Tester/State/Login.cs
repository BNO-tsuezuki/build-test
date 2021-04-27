using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : StateMachineBehaviour
{
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		var tester = animator.GetComponent<Tester>();

		int i = 1100;

		tester.PlayerList.ForEach(player =>
		{
			var account = "test" + i++.ToString("D4");

			player.AddTask(() =>
			{
				var login = new Http.ProtocolModels.Auth.Login();
				login.PreAction = (req) =>
				{
					req.account = account + "@bandainamco-ol.co.jp";
					req.password = account;
					return null;
				};

				login.PostAction = (res) =>
				{
					player.Token = res.token;
					player.StartHandShake();
				};

				return login.RequestCoroutine("");
			});
		});

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		var tester = animator.GetComponent<Tester>();
		if (0 <= tester.PlayerList.FindIndex(p => p.TaskCount != 0))
		{
			return;
		}

		animator.SetTrigger("generalTrigger");
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove()
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that processes and affects root motion
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK()
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    // Implement code that sets up animation IK (inverse kinematics)
	//}
}
