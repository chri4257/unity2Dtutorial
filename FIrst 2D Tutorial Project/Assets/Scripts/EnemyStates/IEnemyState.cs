using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// We are using an interface
public interface IEnemyState
{
	// This is an update on all states
	void Execute();
	// Called when switch into states
	void Enter(Enemy enemy);
	// Called when switch out of states
	void Exit();
	// On trigger
	void OnTriggerEnter(Collider2D other);

}
