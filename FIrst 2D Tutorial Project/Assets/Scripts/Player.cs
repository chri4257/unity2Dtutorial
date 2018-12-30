using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private static Player instance;
	// Some weird instance thing
	public static Player Instance
	{
		get
		{
			if (instance == null)
			{
				// Can't use if there are multiple players as this finds single object
				instance = GameObject.FindObjectOfType<Player>();
			}
			return instance;
		}
	}

	// Define the correct animator
	private Animator myAnimator;

	[SerializeField]
	private Transform knifePosition;

	// Define Speed
	// Private means can't access from other scripts
	// Serialized will make it appear in Unity
	[SerializeField]
	private float movementSpeed;

	private bool facingRight;

	// We select groundPoints from the Unity inspector
	// These ground points will let us know if we are grounded
	[SerializeField]
	private Transform[] groundPoints;
	// How far we are from ground to touch it
	[SerializeField]
	private float groundRadius;
	// We can assing in the inspector to what is ground
	[SerializeField]
	private LayerMask whatIsGround;


	[SerializeField]
	private float jumpForce;

	// Knife prefab
	[SerializeField]
	private GameObject knifePrefab;

	// AirControl allows us to move whilst in the air
	[SerializeField]
	private bool airControl;

	// This defines we are using a Rigidbody
	public Rigidbody2D MyRigidbody { get; set; }

	public bool Attack { get; set; }
	public bool Slide { get; set; }
	public bool Jump { get; set; }
	public bool OnGround { get; set; }
	public bool Throw { get; set; }



	// Use this for initialization
	void Start () {
		// Initially facing right
		facingRight = true;
		// This means that we set the rigidbody equal to the correct ones (i.e. the player that we run this script on) This basically gets the right reference for the correct rigidbody
		MyRigidbody = GetComponent<Rigidbody2D>();
		// Get correct Animator
		myAnimator = GetComponent<Animator>();
	}
	// Update to check every single frame
	void Update() {
		// Check for HandleInputs
		HandleInputs();
	}
	// Update is called once per frame
	// We change from Update to fixedUpdate. Uses set timeframe rather than your computer's fps
	void FixedUpdate () {
			// This gets the input from controller to left or right and sets horizonal
			float horizontal = Input.GetAxis("Horizontal");
			// Just to test it is working
			//Debug.Log(horizontal);
			// See if we are grounded
			OnGround = IsGrounded();
			// This runs the movement function
			HandleMovement(horizontal);

			// Handle animation Layers
			HandleLayers();
			// Test if we need to Flip
			Flip(horizontal);



	}

	// This handles inputs to player
	private void HandleInputs()
	{
		// Jumping
		if (Input.GetKeyDown(KeyCode.Space))
		{
			myAnimator.SetTrigger("jump");
		}
		// If you hold down LeftShift then you attack
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			myAnimator.SetTrigger("attack");
		}
		// Slide inputs
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			myAnimator.SetTrigger("slide");
		}
		if (Input.GetKeyDown(KeyCode.V))
		{
			myAnimator.SetTrigger("throw");
		}
	}


	// This is a function for player movement
	private void HandleMovement(float horizontal)
	{

		// If we are falling then start the landing animation
		if (MyRigidbody.velocity.y < 0)
		{
			myAnimator.SetBool("land", true);
		}
		// Horizontal movement
		if (!Attack && !Slide && (OnGround || airControl))
		{
			MyRigidbody.velocity = new Vector2(movementSpeed * horizontal, MyRigidbody.velocity.y);
		}
		if (Jump && MyRigidbody.velocity.y == 0)
		{
			MyRigidbody.AddForce(new Vector2(0, jumpForce));
		}
		// Set the speed in the Animator
		myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
	}


	// FLips the player from left to right
	private void Flip(float horizontal)
	{
		// If moving right and not facing right then flip. Or if moving left and facing right flip
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
		{
			facingRight = !facingRight;
			// We get the scale of the player
			Vector3 theScale = transform.localScale;
			// We flip the x scale
			theScale.x = -theScale.x;
			transform.localScale = theScale;
		}
	}

	private bool IsGrounded()
	{
		// If we are not moving upwards
		if (MyRigidbody.velocity.y <= 0)
		{
			// For each groundpoint we have
			foreach (Transform point in groundPoints)
			{
				// An array of colliders. Contains everything that the circle overlaps
				Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
				for (int i = 0; i < colliders.Length; i = i + 1)
				{
					// This checks that the collider we are looking at isn't the player itself
					if (colliders[i].gameObject != gameObject)
					{
						return true;
					}
				}

			}
		}
		return false;
	}

	// Layers
	private void HandleLayers()
	{
		// If not grounded then start using layer 1 = air layer
		if (!OnGround)
		{
			myAnimator.SetLayerWeight(1, 1);
		}
		else
		{
			myAnimator.SetLayerWeight(1, 0);
		}
	}

	// Throw Knife
	public void ThrowKnife(int value)
	{
		// value 1 = in the air, value 0 = on the ground
		// remember both animation layers run at the same time, so it would make two layers if we didn't stop it
		if (!OnGround && value == 1 || OnGround && value == 0)
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


}
