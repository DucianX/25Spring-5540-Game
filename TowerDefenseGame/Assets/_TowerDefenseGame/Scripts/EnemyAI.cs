using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyAI : MonoBehaviour
{
    public enum EnemyState {Navigate, Attack, Die}
    [Header("General Settings")]
    public Transform targetBase; // navigate to this base
    public EnemyState currentState = EnemyState.Navigate;
    public float rotateSpeed = 1f;
    [Header("Navigate Settings")]
    public Transform turret;
    public float rotationSpeed = 30f;
    public float detectionRange = 10f;
    public Slider healthSlider;

    [Header("Attack Settings")]
    public bool canAttack = true;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2;
    float fireCooldown = 0f;
    // float gunnerRotation = 126f;
    Transform attackTarget;
    public int baseDamageValue = 10;
    [Header("Die Settings")]
    public int health = 100;
    public GameObject destroyPrefab;
    Transform target;
    NavMeshAgent agent;
    bool isEnemyDead;
    Quaternion initialTurretRotation;
    int maxHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        if (!targetBase) {
            targetBase = GameObject.FindGameObjectWithTag("Target").transform;
            if (!targetBase){
                Debug.LogWarning("No targetBASE FOUND");
                return;
            }

                
        }
         agent = GetComponent<NavMeshAgent>();
         agent.SetDestination(targetBase.position);
         if(turret) 
            initialTurretRotation = turret.localRotation;

        maxHealth = health;

        if (healthSlider) {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState) {
            case EnemyState.Navigate:
                Navigate();
                break;
            case EnemyState.Attack:
                if(canAttack)
                    Attack();
                else 
                    currentState = EnemyState.Navigate;
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    void Navigate() {
        // agent.SetDestination(targetBase.position);

        // If tower, switch to attack 
        if(canAttack)
            FindNearestTower();

        if(turret)
            turret.localRotation = Quaternion.Slerp(turret.localRotation, initialTurretRotation, rotateSpeed * Time.deltaTime);
    }
    void Attack() {
        // If we do not have a target or the old target is out of range, turn to navigate mode
        if(attackTarget == null || Vector3.Distance(transform.position, attackTarget.position) > detectionRange) {
            attackTarget = null;
            currentState = EnemyState.Navigate;
            return;
        }

        // attack
        // Facing the target
        Vector3 direction = attackTarget.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        // Rotate the cannon towards the target
        turret.rotation = Quaternion.Slerp(turret.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        // Cooldown for shooting
        if (fireCooldown <= 0) {
            if(HasLineOfSight(attackTarget))
                Shoot();
            fireCooldown = 1f / fireRate;
        }
        fireCooldown -= Time.deltaTime;
    }
    void Shoot() {
        if(canAttack)
            return;
        var bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        if (bulletBehavior) {
            var targetTowerTurret = attackTarget.transform.Find("Turret").transform;
            if(targetTowerTurret) 
                bulletBehavior.SetTarget(targetTowerTurret);
            else
                bulletBehavior.SetTarget(attackTarget); 
        }
    }
    void Die() {
        if (isEnemyDead)
            return;

        Debug.Log("Die");
        if (destroyPrefab)
            Instantiate(destroyPrefab, transform.position, transform.rotation);
        
        Destroy(gameObject);

        isEnemyDead = true;
    }

    // find a nearest tower and set attack target to it, then turn to attack state
    void FindNearestTower() {
        Debug.Log("car is finding");
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        Transform nearestTower = null;
        float nearestDistance = Mathf.Infinity;
        // Iterate through all towers in range and pick the nearest one to attack
        foreach(Collider collider in colliders) {
            if (collider.CompareTag("Tower")) {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < nearestDistance) {
                    nearestTower = collider.transform;
                    nearestDistance = distance;
                }
            }
        }
        if(nearestTower) {
            Debug.Log("car found tower");
            attackTarget = nearestTower;
            Debug.Log("Tower Detected: " + attackTarget.name);
            currentState = EnemyState.Attack;
        }
    }
    public void TakeDamage(int damage) {
        health -= damage;
        if(healthSlider) {
            healthSlider.value = health;
        }
        if(health <= 0) {
            currentState = EnemyState.Die;
            health = 0;
        }
    }
    bool HasLineOfSight(Transform target) {
        RaycastHit hit;
        Vector3 direction = (target.position - firePoint.position).normalized;
        
        if (Physics.Raycast(firePoint.position, direction, out hit, detectionRange)) {
            if(hit.collider.CompareTag("Tower")) {
                Debug.Log("Tower is in sight: " + hit.collider.name);
                return true;
            }
        }
        return false;
    }
    public int GetEnemyDamgeValue() {
        return baseDamageValue;
    }
    void OnCollisionEnter(Collision collision)
    {
         if(collision.transform.CompareTag("Bullet")) {
            BulletBehavior bulletBehavior = collision.gameObject.GetComponent<BulletBehavior>();
            if(bulletBehavior) {
                TakeDamage(bulletBehavior.GetDamgageValue());
                Debug.Log("Enenemy took"+ bulletBehavior.GetDamgageValue() + "damage");
            }
         }     
    }
}
