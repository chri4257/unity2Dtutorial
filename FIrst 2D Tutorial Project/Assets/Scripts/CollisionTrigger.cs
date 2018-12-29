using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour {

	private BoxCollider2D playerCollider;

	// We select the platformCollider that we want to use
	// We use the non-trigger collider (The one we actually stand on)
	[SerializeField]
	private BoxCollider2D platformCollider;
	// This is the trigger one
	[SerializeField]
	private BoxCollider2D platformTrigger;
	// Use this for initialization
	void Start () {
		// This finds the object called player, and then loads it's BoxCollider2D into playerCollider
		// This is the Player's box collider
		playerCollider = GameObject.Find("Player").GetComponent < BoxCollider2D >();
		// No collision between two platform colliders
		Physics2D.IgnoreCollision(platformCollider, platformTrigger, true);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "Player")
		{
			Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.name == "Player")
		{
			Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
