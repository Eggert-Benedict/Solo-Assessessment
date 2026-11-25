using UnityEngine;
using System.Collections;
public class SphereTest : MonoBehaviour
{
    public Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void TriggerPause()
    {
        StartCoroutine(PauseRoutine());
    }

    public IEnumerator PauseRoutine()
    {
        Debug.Log("Object is pausing...");
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
        // This is the core of unity how to pause an object for a period o time
        yield return new WaitForSeconds(3.0f);
        Debug.Log("Object is active again!");
        rb.constraints = RigidbodyConstraints.None;
    }

    void OnCollisionEnter(Collision other)
    {
        transform.position = new Vector3(-2.2f, 5.7f, -2.6f);
    }
}
