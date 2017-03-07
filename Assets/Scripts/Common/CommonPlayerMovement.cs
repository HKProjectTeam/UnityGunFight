using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonPlayerMovement : MonoBehaviour {

	public float speed = 6f; 
	Vector3 movement;
	private Rigidbody playerRigidbody;         
	Animator anim;

	#if !MOBILE_INPUT
	int floorMask;
	float camRayLength = 100f;
	#endif

	private void Awake()
	{
		#if !MOBILE_INPUT
		// Create a layer mask for the floor layer.
		floorMask = LayerMask.GetMask ("Floor");
		#endif
		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent<Rigidbody>();
	}

	private void OnEnable ()
	{
		// When the tank is turned on, make sure it's not kinematic.
		playerRigidbody.isKinematic = false;
	}


	private void OnDisable ()
	{
		// When the tank is turned off, set it to kinematic so it stops moving.
		playerRigidbody.isKinematic = true;
	}

	private void FixedUpdate()
	{
		// Move and turn the tank.
		/*
		float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
		float v = CrossPlatformInputManager.GetAxisRaw("Vertical");
		*/

		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		Move (h, v);
		Turn ();
		if (anim != null) {
			Animating (h, v);
		}
	}


	private void Move(float h, float v)
	{
		// Set the movement vector based on the axis input.
		movement.Set (h, 0f, v);

		// Normalise the movement vector and make it proportional to the speed per second.
		movement = movement.normalized * speed * Time.deltaTime;

		// Move the player to it's current position plus the movement.
		playerRigidbody.MovePosition (transform.position + movement);
	}


	private void Turn()
	{
		#if !MOBILE_INPUT
		// Create a ray from the mouse cursor on screen in the direction of the camera.
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		// Create a RaycastHit variable to store information about what was hit by the ray.
		RaycastHit floorHit;

		// Perform the raycast and if it hits something on the floor layer...
		if(Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
		{
			// Create a vector from the player to the point on the floor the raycast from the mouse hit.
			Vector3 playerToMouse = floorHit.point - transform.position;

			// Ensure the vector is entirely along the floor plane.
			playerToMouse.y = 0f;

			// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
			Quaternion newRotatation = Quaternion.LookRotation (playerToMouse);

			// Set the player's rotation to this new rotation.
			playerRigidbody.MoveRotation (newRotatation);
		}
		#else

		Vector3 turnDir = new Vector3(CrossPlatformInputManager.GetAxisRaw("Mouse X") , 0f , CrossPlatformInputManager.GetAxisRaw("Mouse Y"));

		if (turnDir != Vector3.zero)
		{
		// Create a vector from the player to the point on the floor the raycast from the mouse hit.
		Vector3 playerToMouse = (transform.position + turnDir) - transform.position;

		// Ensure the vector is entirely along the floor plane.
		playerToMouse.y = 0f;

		// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
		Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

		// Set the player's rotation to this new rotation.
		playerRigidbody.MoveRotation(newRotatation);
		}
		#endif
	}

	void Animating (float h, float v)
	{
		// Create a boolean that is true if either of the input axes is non-zero.
		bool walking = h != 0f || v != 0f;

		// Tell the animator whether or not the player is walking.
		anim.SetBool ("IsWalking", walking);
	}
}
