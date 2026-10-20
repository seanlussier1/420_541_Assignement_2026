using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage();
            Debug.Log("You hit a trap! Lost 1 health.");
        }
    }
}