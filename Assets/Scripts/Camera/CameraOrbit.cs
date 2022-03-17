using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    public float distanceToTarget = 5;
    public float cameraSmoothing = 1;

    private Vector3 previousPosition;


    private void Start()
    {

    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(1))
        {
            Vector3 newPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = previousPosition - newPosition;

            float rotationAroundYAxis = -direction.x * 180; // camera moves horizontally
            float rotationAroundXAxis = direction.y * 180; // camera moves vertically

            cam.transform.position = target.position;

            cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
            cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World); // <— This is what makes it work!

            cam.transform.Translate(new Vector3(0, 2, -distanceToTarget));

            previousPosition = newPosition;
        }
        else
        {
            Vector3 originalPosition = new Vector3 (0, 2, -5);
            Quaternion desiredRotation = Quaternion.Euler(0, 0, 0);

            cam.transform.localRotation = Quaternion.Lerp(transform.localRotation, desiredRotation, Time.deltaTime * cameraSmoothing);
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * cameraSmoothing);
        }
    }
}
