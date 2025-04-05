using UnityEngine;

public class HealthPiackupBehavior : MonoBehaviour
{
    public int healStrength;
    public AudioClip pickupSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            GameObject.FindAnyObjectByType<PlayerHealth>()?.TakeHealth(healStrength);
             AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            Destroy(gameObject);
        }
    }
}
