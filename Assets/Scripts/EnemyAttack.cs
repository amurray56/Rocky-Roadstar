using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //Setup
    private Animator anim;
    private EnemyHealth enemyHealth;
    private InflictDamage inflictDamage;
    //Settings
    public float timeBetweenAttacks = 1f;
    //General
    private GameObject player;
    public bool playerInRange = false;
    private float timer;
    private GameObject victim;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponentInChildren<Animator>();
        inflictDamage = GetComponent<InflictDamage>();
        playerInRange = false;
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenAttacks && playerInRange && !enemyHealth.isDead && !PlayerHealth.noDamage)
        {
            Attack(victim);
            anim.SetBool("Attack", true);
            playerInRange = false;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
            victim = other.gameObject;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
            anim.SetBool("Attack", false);
        }
    }

    private void Attack(GameObject victim)
    {
        timer = 0f;
        if (PlayerHealth.playerHealthAmount > 0 && !enemyHealth.isDead && EnemyMovement.sendEnemiesBackToWayPoints == false)
        {
            anim.SetBool("Attack", false);
            inflictDamage.inflictDamageOnVictim(victim);
        }
    }
}
