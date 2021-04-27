using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logout : StateMachineBehaviour
{
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		var tester = animator.GetComponent<Tester>();

		tester.PlayerList.ForEach(player =>
		{
			player.AddTask(() =>
			{
				var login = new Http.ProtocolModels.Auth.Logout();

				return login.RequestCoroutine(player.Token);
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
