using UnityEngine;

public class ShieldPickupBehavior : MonoBehaviour
{
    public AudioClip pickupSound;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            GameObject.FindAnyObjectByType<CastShield>()?.addShield();
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            Destroy(gameObject);
        }
    }
}
