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

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        if(h != 0 || v != 0)
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
