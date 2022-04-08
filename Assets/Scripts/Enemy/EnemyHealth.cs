using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    //Settings
    public float startingHealth = 100; //Enemy starting health
    public float enemyHealthAmount; //Enemy health
    public AudioClip deathClip; //Enemy death audio
    public AudioClip hurtClip; //Enemy hurt audio
    [HideInInspector]
    public bool enemyWasShotAtByThePlayer = false; //The enemy will target the player if this variable is set to true
    public GameObject deadParticleEffect; //Dead effect
    [HideInInspector]
    public bool isDead; //If set to true when the enemy is dead

    //General Components
    private Animator anim;
    private NavMeshAgent agent;
    private AudioSource enemyAudio; //Enemy death audio

    //Script Components
    [HideInInspector]
    public SpawnerManager enemySpawnerScript; //Stores a reference to the enemy Spawner Script

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void OnEnable()
    {
        enemyHealthAmount = startingHealth;
        enemyWasShotAtByThePlayer = false;
        agent.updatePosition = true; //If set to false this will stop the NavMeshAgent from moving the enemy
        agent.updateRotation = true; //If set to false this will stop the NavMeshAgent from rotating the enemy
        isDead = false;
        gameObject.layer = LayerMask.NameToLayer("Default"); //Allows the player to shoot raycast to hit the enemies
    }

    public void TakeDamage(float amount, bool disableNavMeshAgent = false)
    {
        if (isDead)
        {
            return;
        }
        if (disableNavMeshAgent)
        {
            agent.updatePosition = false; //Stops the enemy from moving
            agent.updateRotation = false; //Stops the enemy from rotating
            anim.SetBool("Explosion", true);
        }
        if (hurtClip && !enemyAudio.isPlaying)
        {
            enemyAudio.volume = 0.3f;
            enemyAudio.clip = hurtClip;
            enemyAudio.Play();
        }
        enemyHealthAmount -= amount;
        enemyWasShotAtByThePlayer = true;
        //Enemy has died
        if (enemyHealthAmount <= 0)
        {
            Death();
            //Add to the total number of enemies killed
            //GameController.gameController.totalEnemiesKilled++;
            GameController.gameController.UpdateHUDManager();
        }
    }

    public void Death()
    {
        if (isDead)
        {
            return;
        }
        GetComponent<EnemyAttack>().playerInRange = false;
        isDead = true;
        GameController.gameController.UpdateHUDManager();
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast"); //Allows the player to shoot through dead enemies

        agent.updatePosition = false; //Stops the enemy from moving
        agent.updateRotation = false; //Stops the enemy from rotating

        if (tag != "Hazard")
        {
            //Plays the enemy dead animation
            anim.SetBool("Dead", true);
            //Removes the enemy from the main enemy list in the game controller
            GameController.gameController.enemies.Remove(gameObject);
            //Removes the enemy from the spawners enemy list in SpawnEnemy
            enemySpawnerScript.enemiesFromThisSpawnerList.Remove(gameObject);
            Invoke("DeadEffect", 1f);
        }
        if (deathClip)
        {
            enemyAudio.clip = deathClip;
            enemyAudio.Play();
        }
    }

    void DeadEffect()
    {
        if (deadParticleEffect)
        {
            GameObject returnedGameObject = PoolManager.current.GetPooledObject(deadParticleEffect.name);
            if (returnedGameObject == null) return;
            returnedGameObject.transform.position = transform.position;
            returnedGameObject.transform.rotation = transform.rotation;
            returnedGameObject.SetActive(true);
        }
        Invoke("DeactivateEnemy", 0.2f);
    }

    void DeactivateEnemy()
    {
        gameObject.SetActive(false);
    }
}
