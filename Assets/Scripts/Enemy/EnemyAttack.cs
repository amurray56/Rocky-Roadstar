using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //Setup
    private Animator anim;
    //Settings
    public float timeBetweenAttacks = 1f;
    //General
    public bool playerInRange = false;
    private float timer;
    private GameObject victim;

    //Inflict Damage
    public float pushForce = 10f; //How far to push the victim
    public float pushHeight = 2f; //How high to push the victim
    public int damage = 10; //Damage to deal to victim
    public bool disableNavmesh = true; //If the nav mesh should be disabled
    public AudioClip hitSound; //Sound to play when an object takes damage
    public float pushTime = 0.2f;

    public void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        playerInRange = false;
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBetweenAttacks && playerInRange)
        {
            Attack(victim);
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            victim = other.gameObject;
            playerInRange = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            victim = null;
            playerInRange = false;
        }
    }

    private void Attack(GameObject victim)
    {
        timer = 0f;
        InflictDamageOnVictim(victim);
        anim.SetTrigger("Attack");
    }

    public void InflictDamageOnVictim(GameObject victim) //Remove health from victim and push it
    {
        if (pushHeight != 0 && GetComponent<EnemyAttack>().playerInRange == true || pushForce != 0 && GetComponent<EnemyAttack>().playerInRange == true) //If push height and force are not 0 push the object backwards
        {
            Vector3 pushDir = victim.transform.position - transform.position;

            if (victim.GetComponent<Rigidbody>() && !victim.GetComponent<Rigidbody>().isKinematic) //If the object has a rigidbody it pushes the object
            {
                victim.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                victim.GetComponent<Rigidbody>().AddForce(pushDir.normalized * pushForce, ForceMode.VelocityChange);
                victim.GetComponent<Rigidbody>().AddForce(Vector3.up * pushHeight, ForceMode.VelocityChange);
            }
        }

            victim.GetComponent<PlayerHealth>().TakeDamage(damage);
    }
}
