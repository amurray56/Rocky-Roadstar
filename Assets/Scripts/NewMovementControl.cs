using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMovementControl : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    public float speed = 1;

    bool isGrounded;

    public float rotationSpeed = 1;

    private Animator anim;

    Vector3 playerVelocity;
    public float jumpForce = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector3(0 * speed, rb.velocity.y, movementY * speed);

        if(rb.velocity.z > 0 || rb.velocity.x > 0 || rb.velocity.z < 0 || rb.velocity.x < 0)
        {
            anim.SetBool("HasInput", true);
        }
        else
        {
            anim.SetBool("HasInput", false);
        }

        if (Input.GetKey(KeyCode.Space) && !anim.GetBool("Jumping"))
        {
            anim.SetBool("Jumping", true);
            rb.velocity = new Vector3(movementX * speed, jumpForce * speed, movementY * speed);
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(-Vector3.up * 100 * Time.deltaTime);

        if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up * 100 * Time.deltaTime);
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
