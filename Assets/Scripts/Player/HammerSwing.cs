using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSwing : MonoBehaviour
{
    public float pushForce;

    private void OnTriggerEnter(Collider hit)
    {
        if(hit.CompareTag("Enemy") || hit.CompareTag("Player"))
        {
            if(hit.CompareTag("Enemy"))
            Physics.IgnoreCollision(gameObject.GetComponent<BoxCollider>(), hit.gameObject.GetComponent<SphereCollider>());

            Vector3 destination = transform.TransformPoint(0, 0, -pushForce);
            StartCoroutine(MoveOverSeconds(hit.gameObject, destination, 1.5f));
        }
    }

    public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.localPosition = Vector3.Lerp(objectToMove.transform.localPosition, end, elapsedTime / seconds);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.localPosition = end;
    }
}
