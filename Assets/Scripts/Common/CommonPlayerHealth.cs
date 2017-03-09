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
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	bool isDead;                                                // Whether the player is dead.
	bool damaged;                                               // True when the player gets damaged.

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnEnable()
	{
		// When the tank is enabled, reset the tank's health and whether or not it's dead.
		currentHealth = startingHealth;
		isDead = false;

		// Update the health slider's value and color.
		SetHealthUI();
	}

	private void SetHealthUI ()
	{
		// Set the slider's value appropriately.
		healthSlider.value = currentHealth;

		// Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
		damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
	}
}
