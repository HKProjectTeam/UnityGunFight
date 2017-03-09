using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonPlayerHealth : MonoBehaviour {

	public int startingHealth = 100;
	public int currentHealth;
	public Slider healthSlider;
	public Image damageImage;

	public float flashSpeed = 5f;
	public Color flashColor = new Color(1f, 0f, 0f, 0.1f);
	bool isDead;                                                // Whether the player is dead.
	bool isDamaged;                                               // True when the player gets damaged.

	Animator anim;                                              // Reference to the Animator component.

	public GameObject explosionPrefab;                // A prefab that will be instantiated in Awake, then used whenever the tank dies.
	private AudioSource explosionAudio;               // The audio source to play when the tank explodes.
	private ParticleSystem explosionParticles;        // The particle system the will play when the tank is destroyed.

	AudioSource playerAudio;                                    // Reference to the AudioSource component.
	public AudioClip deathClip;                                 // The audio clip to play when the player dies.


	void Awake ()
	{
		// Setting up the references.
		anim = GetComponent <Animator> ();
		playerAudio = GetComponent <AudioSource> ();

		// Setting up Health Slider max value
		healthSlider.maxValue = startingHealth;

		// Instantiate the explosion prefab and get a reference to the particle system on it.
		explosionParticles = Instantiate (explosionPrefab).GetComponent<ParticleSystem> ();

		// Get a reference to the audio source on the instantiated prefab.
		explosionAudio = explosionParticles.GetComponent<AudioSource> ();

		// Disable the prefab so it can be activated when it's required.
		explosionParticles.gameObject.SetActive (false);

		// Set the initial health of the player.
		currentHealth = startingHealth;
	}
		
	private void OnEnable()
	{
		// When the tank is enabled, reset the tank's health and whether or not it's dead.
		currentHealth = startingHealth;
		isDead = false;

		// Update the health slider's value and color.
		SetHealthUI();
	}

	void Update ()
	{
		// If the player has just been damaged...
		if(isDamaged)
		{
			// ... set the colour of the damageImage to the flash colour.
			damageImage.color = flashColor;
		}
		// Otherwise...
		else
		{
			// ... transition the colour back to clear.
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}

		// Reset the damaged flag.
		isDamaged = false;
	}


	public void TakeDamage (int amount)
	{
		isDamaged = true;
		// Reduce current health by the amount of damage done.
		currentHealth -= amount;

		// Change the UI elements appropriately.
		SetHealthUI ();

		// If the current health is at or below zero and it has not yet been registered, call OnDeath.
		if (currentHealth <= 0f && !isDead)
		{
			Death ();
		}
	}

	private void SetHealthUI ()
	{
		// Set the slider's value appropriately.
		healthSlider.value = currentHealth;
	}

	void Death ()
	{
		// Set the death flag so this function won't be called again.
		isDead = true;

		// Tell the animator that the player is dead.
		if (anim != null) {
			anim.SetTrigger ("Die");
		}

		// Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
		playerAudio.clip = deathClip;
		playerAudio.Play ();

		// Move the instantiated explosion prefab to the tank's position and turn it on.
		explosionParticles.transform.position = transform.position;
		explosionParticles.gameObject.SetActive (true);

		// Play the particle system of the tank exploding.
		explosionParticles.Play ();

		// Play the tank explosion sound effect.
		explosionAudio.Play();

		// Turn the player off 
		gameObject.SetActive (false);
	}
}
