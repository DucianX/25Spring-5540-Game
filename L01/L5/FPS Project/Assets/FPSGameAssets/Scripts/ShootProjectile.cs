
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class ShootProjectile : MonoBehaviour
{   
    [Header("Projectile Settings")]
    public GameObject patronusProjectile;
    public GameObject reductoProjectile;
    public GameObject defaultProjectile;
    public float projectileSpeed = 5;
    public AudioClip spellSFX;

    [Header("Reticle Settings")]
    public Image reticleImage;
    public float spellRange = 20f;
    // public Color targetColorDementor;
    public Color originalReticleColor;
    public float animationSpeed = 10f;
    Color currentReticleColor;
    UnityEngine.Vector3 originalReticleScale;
    public int ammo;
    public TextMeshProUGUI ammoText;
    GameObject currentProjectile;
    public AudioClip rejectSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalReticleColor = reticleImage.color;
        originalReticleScale = reticleImage.transform.localScale;
        if (defaultProjectile) {
            currentProjectile = defaultProjectile;
        }
        UpdateReticleColor();
    }

    void FixedUpdate()
    {
        if(!reticleImage) return;
        InteractiveEffect();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) 
            Shoot();
        ammoText.text = ammo.ToString();
    }

    void Shoot() {
        if (currentProjectile && ammo > 0) {
             GameObject spell = Instantiate(currentProjectile, transform.position + transform.forward, transform.rotation);
            ammo -= 1;
             Rigidbody rb = spell.GetComponent<Rigidbody>();

             if(rb) 
                rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);
            

            if (spellSFX) 
                AudioSource.PlayClipAtPoint(spellSFX, transform.position);
            
            spell.transform.SetParent(transform);
        }
        if(ammo == 0) {
            AudioSource.PlayClipAtPoint(rejectSFX, transform.position);
        }
       
    }
    
    // change the material of projectile when pointing at different objects
    void InteractiveEffect() {
        RaycastHit hit;

        // return Boolean if we can hit the object
        if (Physics.Raycast(transform.position, transform.forward, 
            out hit, spellRange)) 
            {
                // Debug.Log("Hit something " + hit.collider.name);
                if(hit.collider.CompareTag("Dementor")) {
                    currentProjectile = patronusProjectile;
                    UpdateReticleColor();
                    ReticleAnimation(originalReticleScale / 2, currentReticleColor, animationSpeed);
                } 
                else if (hit.collider.CompareTag("Reducto")) {
                    currentProjectile = reductoProjectile;
                    UpdateReticleColor();
                    ReticleAnimation(originalReticleScale / 2, currentReticleColor, animationSpeed);

                }
        } else {
                    currentProjectile = defaultProjectile;
                    UpdateReticleColor();
                    ReticleAnimation(originalReticleScale, originalReticleColor, animationSpeed);
                }
    }

    void ReticleAnimation(Vector3 targetScale, Color targetColor, float speed) {
        var step = animationSpeed * Time.deltaTime;
        reticleImage.color = Color.Lerp(reticleImage.color, targetColor, Time.deltaTime);
        reticleImage.transform.localScale = UnityEngine.Vector3.Lerp(reticleImage.transform.localScale, targetScale, step);
    }

    void UpdateReticleColor() {
        currentReticleColor = currentProjectile.GetComponent<Renderer>().sharedMaterial.color;
    }

    public void addAmmo(int amount) {
        ammo += amount;
    }
}
