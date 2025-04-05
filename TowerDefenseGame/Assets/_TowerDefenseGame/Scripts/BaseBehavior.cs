using UnityEngine;
using UnityEngine.UI;

public class BaseBehavior : MonoBehaviour
{
    
    public Slider healthSlider;
    public int health = 100;
    public int maxHealth;
    public ParticleSystem baseAttackVfx;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = health;
        if (healthSlider) {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage) {
        health -= damage;
        if(healthSlider) {
            healthSlider.value = health;
        }
        if(health <= 0) {
            Debug.Log("Game Over!");
            health = 0;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) {
            EnemyAI enemyAI = other.GetComponent<EnemyAI>();

            if(enemyAI) {
                int baseDamageValue = enemyAI.GetEnemyDamgeValue();
                TakeDamage(baseDamageValue);
                if (baseAttackVfx)
                    baseAttackVfx.Play();
                Debug.Log("Based took damage: " + baseDamageValue);
            }
            Destroy(other.gameObject);
        }
    }
}
