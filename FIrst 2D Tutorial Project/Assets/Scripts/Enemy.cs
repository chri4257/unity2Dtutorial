using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

	// We are using the IEnemyState interface. currenState is a state in that interface
	// Hence we can run the Enter Execute Exit functions from the interface on currentState
	private IEnemyState currentState;

	// We need to have a target for enemy to approach
	// We set this property in the EnemySight.cs script. (The target is the player if the enemy sees the player)
	public GameObject Target { get; set; }

	// Use this for initialization
	public override void Start () {
		// Make sure base charecter start function runs too
		base.Start();
		ChangeState(new IdleState());

	}




	// Update is called once per frame
	void Update () {
		// Runs the current state execute code
		currentState.Execute();
		// Keep facing Target
		LookAtTarget();
	}

	// This keeps enemy looking at target
	private void LookAtTarget()
	{
		if (Target != null)
		{
			// If xDir >0 then that means target is on the right of the Enemy
			float xDir = Target.transform.position.x - transform.position.x;

			if (xDir < 0 && facingRight || xDir >=0 && !facingRight)
			{
				ChangeDirection();
			}
		}
	}



	// This is our function for changing states in IEnemyState
	public void ChangeState(IEnemyState newState)
	{
		// If state is not empty then run the Exit() code for that state
		if (currentState != null)
		{
			currentState.Exit();
		}
		currentState = newState;

		// Run the Enter() function for the state. With input this = enemy
		// Nowe we run Enter() on the current state which = nsewState
		// When Start{} is run this will load up IdleState() and the input will be this Enemy.cs
		currentState.Enter(this);
	}

	// Function to allow charecter to move
	public void Move()
	{
		// Starts run animation
		MyAnimator.SetFloat("speed", 1);
		// Time.deltaTime means it takes time into account depending on framerate of device
		transform.Translate(GetDirection() * movementSpeed * Time.deltaTime);
	}

	public Vector2 GetDirection()
	{
		// Shortcut if statement
		// if facingRight return Vector2.right else return Vector2.left
		return facingRight ? Vector2.right : Vector2.left;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		currentState.OnTriggerEnter(other);
	}

}
