using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	//Setup
	private Collider playerCollider;
	private Rigidbody playerRigidbody;
	//private AudioSource playerAudio;
	private Animator anim; //Holds a reference to the playeres animator
	private MouseLook mouseLookScript;
	public static bool noDamage = false;
	public GameObject alarm;
	private CharacterMovement cm;

	//Settings
	public static float playerHealthAmount = 100; //Players Health
	//public AudioClip damageClip;
	//public AudioClip deathClip;
	public float playerRespawnDelay = 6f;
	public static bool isDead = false;

	public void Awake()
	{
		playerCollider = GetComponent<Collider>();
		playerRigidbody = GetComponent<Rigidbody>();
		//playerAudio = GetComponent<AudioSource>();
		anim = GetComponentInChildren<Animator>();
		cm = GetComponent<CharacterMovement>();
		playerHealthAmount = 100;

	}
	public void Start()
	{
		GameController.gameController.playerRespawnPosition = transform.position; //This holds the players starting poistion, so that if the player has not found a checkpoint the player will appear where we started.
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
		GameController.gameController.UpdateHUDManager(); //Updates the health in the hud
															  //Plays damage audio
		//playerAudio.clip = damageClip;
		//playerAudio.Play();
		//If we have run out of health then call Death function
		if (playerHealthAmount <= 0)
		{
			Death();
		}

	}

	//This function is called when the player has < 0 health
	public void Death()
	{
		GameController.gameController.numberOfLivesLeft--; //Takes a life from the player
															   //Resets health

		if (GameController.gameController.numberOfLivesLeft > 0)
		{
			playerHealthAmount = 100;
		}

		GameController.gameController.UpdateHUDManager(); //updates value on hud
		cm.enabled = false; //Stops the player from being moved

		isDead = true;
		playerRigidbody.isKinematic = true;

		//Plays the dead aniamtion
		if (anim)
		{
			anim.SetLayerWeight(1, 0.5f);
			anim.SetBool("Death", true);
		}
		//Plays Dead Audio
		//playerAudio.clip = deathClip;
		//playerAudio.Play();

		if (GameController.gameController.numberOfLivesLeft > 0)
		{
			Invoke("Respawn", playerRespawnDelay);  //Calls the respawns function after a set amount of time
		}

			mouseLookScript.enabled = false;
		}
	//Respawns the player
	void Respawn()
	{
		if (alarm.activeSelf)
		{
			transform.position = GameController.gameController.playerCheckpointRespawn;
		}
		else
		{
			transform.position = GameController.gameController.playerRespawnPosition; //Sets the players respawn posistion
		}
		//Sets the dead animation to false
		if (anim)
		{
			anim.SetBool("Death", false);
		}
		playerRigidbody.isKinematic = false;
		isDead = false;
		cm.enabled = true; //Allows the player to be moved again

	}
}
