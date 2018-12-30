using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// By attaching this require component. It will automatically add the rigidbody script to the prefab
[RequireComponent(typeof(Rigidbody2D))]
public class Knife : MonoBehaviour {

	[SerializeField]
	private float speed;

	private Rigidbody2D myRigidbody;

	private Vector2 direction;


	// Use this for initialization
	void Start () {
		myRigidbody = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		myRigidbody.velocity = direction * speed;
	}

	// We can access this from player as it is public
	// When we know Player's direction we feed in the correct direction and run this function in this scripts
	// This then sets this.direction (the direction in Knife.cs) to the correct direction. We then use this in the FixedUpdate
	public void Initialize(Vector2 direction)
	{
		this.direction = direction;
	}

	// Deletes object once it gets outside the screen
	void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
}
