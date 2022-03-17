using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InflictDamage : MonoBehaviour
{
    public float pushForce = 10f; //How far to push the victim
    public float pushHeight = 2f; //How high to push the victim
    public int damage = 10; //Damage to deal to victim
    public bool disableNavmesh = true; //If the nav mesh should be disabled
    public AudioClip hitSound; //Sound to play when an object takes damage
    public float pushTime = 0.2f;

    public void inflictDamageOnVictim(GameObject victim) //Remove health from victim and push it
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

        if (hitSound)
        {
            GetComponent<AudioSource>().clip = hitSound;
            GetComponent<AudioSource>().Play();
        }

        if (victim.tag == "Player" && !HUDManager.gamePaused && GetComponent<EnemyAttack>().playerInRange == true)
        {
            victim.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
        else if (victim.tag == "Enemy")
        {
            victim.GetComponent<EnemyHealth>().TakeDamage(damage, disableNavmesh);
        }
    }
}
