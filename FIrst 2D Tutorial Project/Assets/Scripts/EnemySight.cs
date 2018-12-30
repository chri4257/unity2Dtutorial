using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {

	// Here we set the enemy object
	[SerializeField]
	private Enemy enemy;

	// If enemy sees the playr, then enemy makes player their target
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "player")
		{
			enemy.Target = other.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "player")
		{
			enemy.Target = null;
		}
	}
}
