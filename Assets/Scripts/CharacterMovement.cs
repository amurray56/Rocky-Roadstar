using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Tooltip("Movement Values")]
    [SerializeField] float speedMultiplier, rotatationSpeed, gravityForce, jumpForce, groundCastRange;

    //Components
    CharacterController cc;
    Animator anim;

    Vector3 movementDirection;
    Vector3 playerVelocity;
    bool groundedPlayer;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        groundedPlayer = cc.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            if (anim.GetBool("Jumping")) anim.SetBool("Jumping", false);
            playerVelocity.y = 0f;
        }
       
        ProcessGravity();
        Move();
    }

    public void ProcessGravity()
    {
        if(Input.GetKey(KeyCode.Space) && !anim.GetBool("Jumping"))
        {
            anim.SetBool("Jumping", true);
            playerVelocity.y += Mathf.Sqrt(jumpForce * -3.0f * gravityForce);
        }
        playerVelocity.y += gravityForce * Time.deltaTime;
        cc.Move(playerVelocity * Time.deltaTime);
    }

    void Move()
    {

        if (Input.GetKey(KeyCode.A))
        {
            movementDirection.Set(-1, 0, 0);
            cc.Move(movementDirection * speedMultiplier * Time.deltaTime);
            anim.SetBool("HasInput", true);
            transform.LookAt(movementDirection);
        }
        else
        {
            anim.SetBool("HasInput", false);
        }

        if (Input.GetKey(KeyCode.W))
        {
            movementDirection.Set(0, 0, 1);
            cc.Move(movementDirection * speedMultiplier * Time.deltaTime);
            anim.SetBool("HasInput", true);
            transform.LookAt(movementDirection);
        }
        else
        {
            anim.SetBool("HasInput", false);
        }

        if (Input.GetKey(KeyCode.S))
        {
            movementDirection.Set(0, 0, -1);
            cc.Move(movementDirection * speedMultiplier * Time.deltaTime);
            anim.SetBool("HasInput", true);
            transform.LookAt(movementDirection);
        }
        else
        {
            anim.SetBool("HasInput", false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            movementDirection.Set(1, 0, 0);
            cc.Move(movementDirection * speedMultiplier * Time.deltaTime);
            anim.SetBool("HasInput", true);
            transform.LookAt(movementDirection);
        }
        else
        {
            anim.SetBool("HasInput", false);
        }

        var desiredDirection = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredDirection, rotatationSpeed);

        var animationVector = transform.InverseTransformDirection(cc.velocity);

        anim.SetFloat("ForwardMomentum", animationVector.z);
        anim.SetFloat("SideMomentum", animationVector.x);
    }

    void DanielRMoveScript()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            movementDirection.Set(h, 0, v);
            cc.Move(movementDirection * speedMultiplier * Time.deltaTime);
            anim.SetBool("HasInput", true);
        }
        else
        {
            anim.SetBool("HasInput", false);
        }

        var desiredDirection = Quaternion.LookRotation(movementDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredDirection, rotatationSpeed);

        var animationVector = transform.InverseTransformDirection(cc.velocity);

        anim.SetFloat("ForwardMomentum", animationVector.z);
        anim.SetFloat("SideMomentum", animationVector.x);
    }
}
