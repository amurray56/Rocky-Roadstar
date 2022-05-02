using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	//Setup
	private Rigidbody playerRigidbody;
	//private AudioSource playerAudio;
	private Animator anim; //Holds a reference to the playeres animator
	public bool noDamage = false;
	private NewMovementControl cm;
	private HUDManager hUDManager;

	//Settings
	public float playerHealthAmount = 100; //Players Health
	public int numberOfLivesLeft = 3;
	public Vector3 respawnPoint;
	//public AudioClip damageClip;
	//public AudioClip deathClip;
	public float playerRespawnDelay = 6f;
	public bool isDead = false;

	public void Awake()
	{
		playerRigidbody = GetComponent<Rigidbody>();
		//playerAudio = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
		cm = GetComponent<NewMovementControl>();
		playerHealthAmount = 100;
		hUDManager = GetComponentInChildren<HUDManager>();
	}

	public void Update()
	{
		if (playerHealthAmount > 100)
		{
			playerHealthAmount = 100;
		}
	}

	//When called this function takes health from the player
	public void TakeDamage(float amount)
	{
		if (isDead)
		{
			return;
		}

		playerHealthAmount -= amount; //Takes health
		hUDManager.UpdateHUD(); //Updates the health in the hud

		//If we have run out of health then call Death function
		if (playerHealthAmount <= 0)
		{
			Death();
		}
	}

	//This function is called when the player has < 0 health
	public void Death()
	{
		numberOfLivesLeft--; //Takes a life from the player
		GetComponent<CoinValueHeld>().coinValueHeld = 0;
		
		//Resets health
		if (numberOfLivesLeft > 0)
		{
			playerHealthAmount = 100;
		}

		GameController.gameController.UpdateHUDManager(); //updates value on hud
		cm.enabled = false; //Stops the player from being moved

		anim.SetTrigger("Death");
		anim.SetBool("isDead", true);
		isDead = true;
		playerRigidbody.isKinematic = true;
		
		if (numberOfLivesLeft > 0)
		{
			Invoke("Respawn", playerRespawnDelay);  //Calls the respawns function after a set amount of time
		}
	}
	//Respawns the player
	void Respawn()
	{
		anim.SetBool("isDead", false);
		transform.position = respawnPoint; //Sets the players respawn posistion
		playerRigidbody.isKinematic = false;
		isDead = false;
		cm.enabled = true; //Allows the player to be moved again

	}

	public void ResetDamage()
    {
		playerHealthAmount = 100;
    }
}
