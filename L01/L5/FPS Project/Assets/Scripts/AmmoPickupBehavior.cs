using UnityEngine;

public class AmmoPickupBehavior : MonoBehaviour
{
    public int addAmmoAmount;
    public AudioClip pickupSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            GameObject.FindAnyObjectByType<ShootProjectile>()?.addAmmo(addAmmoAmount);
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            Destroy(gameObject);
        }
    }
}