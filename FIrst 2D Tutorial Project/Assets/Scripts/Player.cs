using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// This defines we are using a Rigidbody
	private Rigidbody2D myRigidbody;

	// Define the correct animator
	private Animator myAnimator;
	// Define Speed
	// Private means can't access from other scripts
	// Serialized will make it appear in Unity
	[SerializeField]
	private float movementSpeed;

	private bool attack;
	private bool slide;
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
	private bool isGrounded;
	// Jumping variables
	private bool jump;
	[SerializeField]
	private float jumpForce;
	// AirControl allows us to move whilst in the air
	[SerializeField]
	private bool airControl;

	private bool jumpAttack;

	// Use this for initialization
	void Start () {
		// Initially facing right
		facingRight = true;
		// This means that we set the rigidbody equal to the correct ones (i.e. the player that we run this script on) This basically gets the right reference for the correct rigidbody
		myRigidbody = GetComponent<Rigidbody2D>();
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
			isGrounded = IsGrounded();
			// This runs the movement function
			HandleMovement(horizontal);
			// Runs the attack function
			HandleAttacks();
			// Handle animation Layers
			HandleLayers();
			// Test if we need to Flip
			Flip(horizontal);


			// Resets all values to their defaults
			ResetValues();
	}

	// This handles inputs to player
	private void HandleInputs()
	{
		// Jumping
		if (Input.GetKeyDown(KeyCode.Space))
		{
			jump = true;
		}
		// If you hold down LeftShift then you attack
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			attack = true;
			jumpAttack = true;
		}
		// Slide inputs
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			slide= true;
		}
	}


	// This is a function for player movement
	private void HandleMovement(float horizontal)
	{
		// If we are falling then start the landing animation
		if (myRigidbody.velocity.y < 0)
		{
			myAnimator.SetBool("land", true);
		}
	//	else if (myRigidbody.velocity.y >= 0) {
	//		myAnimator.SetBool("land", false);
	//	}
		// If NOT current animation in layer 0 (base layer) has the tag AttackTag (which we input as tag in Unity by clicking on attack in the Animator)
		// Basically if we arn't attacking then we can move. If we are attacking gotta stay still
		// We also need to be not sliding currently in order to change direction
		// And it requires us to be grounded, or have aircontrol switched on (which means we can move whilst in the air)
		if (!myAnimator.GetBool("slide") && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("AttackTag") && (isGrounded || airControl))
		{
			// We set the velocity of the player here to be left or right
			myRigidbody.velocity = new Vector2(movementSpeed * horizontal, myRigidbody.velocity.y);
		}

		// Jumping
		if (isGrounded && jump)
		{
			isGrounded = false;
			myRigidbody.AddForce(new Vector2(0, jumpForce));
			// jump animation
			myAnimator.SetTrigger("jump");
		}

		// If slide = true and player not currently in slide animation then lets enable slide animation
		if (slide && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("SlideTag"))
		{
			myAnimator.SetBool("slide", true);
		}
		// If we are not sliding then let's stop the sliding animation happening
		else if (!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("SlideTag"))
		{
			myAnimator.SetBool("slide", false);
		}

		// Sets the parameter "speed" in the animator to the absolute speed
		myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
	}

	private void HandleAttacks()
	{
		// If attack and you not attacking already and not in air
		if (attack && isGrounded && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("AttackTag"))	{
			// Sets the attack trigger which forces attack animation
			myAnimator.SetTrigger("attack");
			// Set velocity to 0 when you attacking
			myRigidbody.velocity = new Vector2(0, myRigidbody.velocity.y);
		}
		if (jumpAttack && !isGrounded && !this.myAnimator.GetCurrentAnimatorStateInfo(1).IsName("JumpAttackTag"))
		{
			myAnimator.SetBool("jumpAttack", true);
		}
		if (!jumpAttack && !this.myAnimator.GetCurrentAnimatorStateInfo(1).IsName("JumpAttackTag"))
		{
			myAnimator.SetBool("jumpAttack", false);
		}
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
		if (myRigidbody.velocity.y <= 0)
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
						// Reset the jump trigger and turn off landing animation
						myAnimator.ResetTrigger("jump");
						myAnimator.SetBool("land", false);
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
		if (!isGrounded)
		{
			myAnimator.SetLayerWeight(1, 1);
		}
		else
		{
			myAnimator.SetLayerWeight(1, 0);
		}
	}

	// Resets parameters to inital vlaues
	private void ResetValues()
	{
		attack = false;
		slide = false;
		jump = false;
		jumpAttack = false;
	}

}
