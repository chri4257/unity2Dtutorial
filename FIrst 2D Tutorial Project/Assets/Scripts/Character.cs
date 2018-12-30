using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// abstract class stops this charecter class being added to the game
// can only add player, or enemy script to an object, but they inherit this class
public abstract class Character : MonoBehaviour {

	// Define the correct animator
	// Protected means can access in any script whichi inherits this class
	protected Animator myAnimator;

	[SerializeField]
	protected Transform knifePosition;

	// Define Speed
	// Private means can't access from other scripts
	// Serialized will make it appear in Unity
	[SerializeField]
	protected float movementSpeed;

	protected bool facingRight;

	// Knife prefab
	[SerializeField]
	private GameObject knifePrefab;

	public bool Attack { get; set; }
	public bool Throw { get; set; }


	// Use this for initialization
	// virtual means it can be overwritten
	public virtual void Start () {

		// Initially facing right
		facingRight = true;
		// Get correct Animator
		myAnimator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {

	}

	public void ChangeDirection()
	{
		facingRight = !facingRight;
		transform.localScale = new Vector3(transform.localScale.x*-1, 1, 1);
	}

	public virtual void ThrowKnife(int value)
	{
		if (facingRight)
		{
			// Create an instance of knife prefab. At the correct knife position. Rotated in correct direction
			// This instance is called tmp
			GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePosition.position, Quaternion.Euler(new Vector3(0, 0, -90)));
			// We then look at the tmp instance and look at script Knife.cs and find the Intialize public function. We feed this Initialize a right Vector
			tmp.GetComponent<Knife>().Initialize(Vector2.right);
		}
		else
		{
			GameObject tmp = (GameObject)Instantiate(knifePrefab, knifePosition.position , Quaternion.Euler(new Vector3(0, 0, 90)));
			tmp.GetComponent<Knife>().Initialize(Vector2.left);
		}
	}

}
