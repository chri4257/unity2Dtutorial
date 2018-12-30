using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState {

	private Enemy enemy;
	// To create a timer
	private float patrolTimer;
	private float patrolDuration = 5;


	public void Enter(Enemy enemy)
	{
		// See idlestate for explanation
		this.enemy = enemy;
	}

	public void Execute()
	{
		Patrol();
		// Move the enemy object
		enemy.Move();

		if (enemy.Target != null)
		{
			enemy.ChangeState(new RangedState());
		}

	}

	public void Exit()
	{

	}

	public void OnTriggerEnter(Collider2D other)
	{
		if (other.tag == "Edge")
		{
			enemy.ChangeDirection();
		}
	}

	private void Patrol()
	{


		// Time to switch to idle
		patrolTimer += Time.deltaTime;
		if (patrolTimer >= patrolDuration)
		{
			enemy.ChangeState(new IdleState());
		}
	}

}
