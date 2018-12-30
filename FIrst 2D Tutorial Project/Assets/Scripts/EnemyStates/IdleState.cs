using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState {


	private Enemy enemy;
	// To create a timer
	private float idleTimer;
	private float idleDuration = 5;

	public void Enter(Enemy enemy)
	{
		// The private enemy in this IdleState class is equal to the enemy input in this Enter function
		// the input in the function was this.enemy in the enemy.cs script
		this.enemy = enemy;
		// So now enemy in the main script will be the same as enemy in the enemy.cs script
	}

	public void Execute()
	{
		Idle();
		if (enemy.Target != null)
		{
			// Move into patrol state, and from there we can move further into ranged state
			enemy.ChangeState(new PatrolState());
		}
	}

	public void Exit()
	{

	}

	public void OnTriggerEnter(Collider2D other)
	{

	}

	private void Idle()
	{
		// We are able to access MyAnimator because it is public.
		// After the Enter() function enemy is now the same object as in the enemy.cs scripts
		// We are therefore changing the animator of the enemy object
		// We set the speed to 0 to make it idle
		enemy.MyAnimator.SetFloat("speed", 0);

		// Add to the timer every time this function is run
		idleTimer += Time.deltaTime;

		if (idleTimer >= idleDuration)
		{
			// Change state to the Patrol state after timer is done
			enemy.ChangeState(new PatrolState());
		}
	}

}
