using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  This script is run when player or enemy enters attack or throw animation
public class AttackBehaivor : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		// Get the charecter component of what is in the animator (player or enemy) and set Attack property of it to true
		// Charecter component is t he player.cs script
		animator.GetComponent<Character>().Attack = true;

		animator.SetFloat("speed", 0);

		if (animator.tag == "player")
		{
			if (Player.Instance.OnGround)
				{
					Player.Instance.MyRigidbody.velocity = Vector2.zero;
				}
		}
   // Stop player moving if they attack

	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	   animator.GetComponent<Character>().Attack = false;
     animator.ResetTrigger("attack");
		 animator.ResetTrigger("throw");
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
