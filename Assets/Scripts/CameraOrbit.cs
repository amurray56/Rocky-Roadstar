using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform player;
    public float turnSpeed = 0.4f;
    public float height;
    public float distance;

    private Vector3 offsetX;
    private Vector3 offsetY;


    [AddComponentMenu("Camera-Control/Mouse Look")]

    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;
    float rotationX = 0F;

    // Start is called before the first frame update
    void Start()
    {
        offsetX = new Vector3(0, 0.3f, -5);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.position);
        if (Input.GetMouseButton(1))
        {
            offsetX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offsetX;
            offsetY = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * turnSpeed, Vector3.right) * offsetY;
            //transform.position = player.position + offsetX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
    }
}
