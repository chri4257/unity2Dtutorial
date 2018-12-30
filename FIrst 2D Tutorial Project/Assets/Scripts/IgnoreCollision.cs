using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// We created this script to stop the player and enemies colliding with each other
public class IgnoreCollision : MonoBehaviour {

	// We load a collider called other
	[SerializeField]
	private Collider2D ignoreCollisionWith;

	// Use this for initialization
	private void Awake ()
	{
		// Ignore collisions between my own collider and the other collider specified
		Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ignoreCollisionWith, true);
	}
}
