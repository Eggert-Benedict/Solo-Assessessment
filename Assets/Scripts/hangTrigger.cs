using UnityEngine;

public class hangTrigger : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().hangChoice = true;
        }
    }
}
