using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMovementControl : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 1;
    private bool isGrounded;
    private Animator anim;
    public float jumpForce = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += speed * transform.forward * Time.deltaTime;
            anim.SetBool("HasInput", true);
            anim.SetFloat("ForwardMomentum", 1);
        }

        if(Input.GetKey(KeyCode.S))
        {
            transform.position -= speed * transform.forward * Time.deltaTime;
            anim.SetBool("HasInput", true);
            anim.SetFloat("ForwardMomentum", -1);
        }

        if (Input.GetKey(KeyCode.Space) && !anim.GetBool("Jumping") && isGrounded)
        {
            anim.SetBool("Jumping", true);
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("HasInput", true);
            transform.Rotate(-Vector3.up * 100 * Time.deltaTime);
            anim.SetFloat("SideMomentum", -1);
        }

        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("HasInput", true);
            transform.Rotate(Vector3.up * 100 * Time.deltaTime);
            anim.SetFloat("SideMomentum", 1);

        }

        if(!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            anim.SetBool("HasInput", false);
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            anim.SetFloat("ForwardMomentum", 0);
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            anim.SetFloat("SideMomentum", 0);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Ground")
        {
            isGrounded = true;
            anim.SetBool("Jumping", false);
        }
        else
        {
            isGrounded = false;
        }
    }
}
