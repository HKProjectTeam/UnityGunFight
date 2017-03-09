using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonPlayerShooting : MonoBehaviour {

	public Rigidbody missile;                   // Prefab of the missile.
	public Transform fireTransform;           // A child of the tank where the missiles are spawned.
	public Slider aimSlider;                  // A child of the tank that displays the current launch force.
	public AudioSource shootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
	public AudioClip chargingClip;            // Audio that plays when each shot is charging up.
	public AudioClip fireClip;                // Audio that plays when each shot is fired.
	public float minLaunchForce = 15f;        // The force given to the missile if the fire button is not held.
	public float maxLaunchForce = 30f;        // The force given to the missile if the fire button is held for the max charge time.
	public float maxChargeTime = 0.75f;       // How long the missile can charge for before it is fired at max force.


	private string fireButton;                // The input axis that is used for launching missiles.
	private float currentLaunchForce;         // The force that will be given to the missile when the fire button is released.
	private float chargeSpeed;                // How fast the launch force increases, based on the max charge time.
	private bool isFired;                       // Whether or not the missile has been launched with this button press.


	private void OnEnable()
	{
		//Set AimSlider min, max Value
		aimSlider.maxValue = maxLaunchForce;
		aimSlider.minValue = minLaunchForce;
		// When the tank is turned on, reset the launch force and the UI
		currentLaunchForce = minLaunchForce;
		aimSlider.value = minLaunchForce;

		isFired = true;
	}


	private void Start ()
	{
		// The fire axis is based on the player number.
		fireButton = "Fire1";

		// The rate that the launch force charges up is the range of possible forces by the max charge time.
		chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
	}


	private void Update ()
	{
		// If the fire button is released and the missile hasn't been launched yet...
		if (Input.GetButtonUp (fireButton) && !isFired)
		{
			aimSlider.value = minLaunchForce;
			// ... launch the missile.
			Fire ();

		}
		// Otherwise,if the max force has been exceeded and the missile hasn't yet been launched...
		else if (currentLaunchForce >= maxLaunchForce && !isFired)
		{
			// ... use the max force and launch the missile.
			currentLaunchForce = maxLaunchForce;
			//Fire ();
		}
		// Otherwise, if the fire button has just started being pressed...
		else if (Input.GetButtonDown (fireButton))
		{
			// ... reset the fired flag and reset the launch force.
			isFired = false;
			currentLaunchForce = minLaunchForce;

			// Change the clip to the charging clip and start it playing.
			shootingAudio.clip = chargingClip;
			shootingAudio.Play ();
		}
		// Otherwise, if the fire button is being held and the missile hasn't been launched yet...
		else if (Input.GetButton (fireButton) && !isFired)
		{
			// Increment the launch force and update the slider.
			currentLaunchForce += chargeSpeed * Time.deltaTime;

			aimSlider.value = currentLaunchForce;
		}

	}


	private void Fire ()
	{
		// Set the fired flag so only Fire is only called once.
		isFired = true;

		// Create an instance of the missile and store a reference to it's rigidbody.
		Rigidbody missileInstance =
			Instantiate (missile, fireTransform.position, fireTransform.rotation) as Rigidbody;

		// Set the missile's velocity to the launch force in the fire position's forward direction.
		missileInstance.velocity = currentLaunchForce * fireTransform.forward; 

		// Change the clip to the firing clip and play it.
		shootingAudio.clip = fireClip;
		shootingAudio.Play ();

		// Reset the launch force.  This is a precaution in case of missing button events.
		currentLaunchForce = minLaunchForce;
	}
}
