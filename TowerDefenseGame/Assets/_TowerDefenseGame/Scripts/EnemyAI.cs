using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class EnemyAI : MonoBehaviour
{
    public enum EnemyState { Navigate, Attack, Die }
    [Header("General Settings")]

    public Transform targetBase; // navigate to this base
    public EnemyState currentState = EnemyState.Navigate;
    public float rotateSpeed = 1f;
    [Header("Navigate Settings")]


    public float rotationSpeed = 30f;
    public float detectionRange = 10f;
    public Slider healthSlider;

    [Header("Attack Settings")]
    public Transform turret;
    public bool canAttack = true;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2;
    float fireCooldown = 0f;
    // float gunnerRotation = 126f;
    Transform attackTarget;
    public int baseDamageValue = 10;
    [Header("Die Settings")]
    public int reward;
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
        fireCooldown = 0f;
        // Find destination: our targetBase
        if (!targetBase)
        {
            targetBase = GameObject.FindGameObjectWithTag("Target").transform;
            if (!targetBase)
            {
                Debug.LogWarning("No targetBASE FOUND");
                return;
            }


        }
        // Move it to destination: targetBase
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(targetBase.position);
        // turret means enemy's own turret
        if (turret)
            initialTurretRotation = turret.localRotation;

        maxHealth = health;

        if (healthSlider)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
        }

    }

    // Update is called once per frame, switching states when necessary
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Navigate:
                Navigate();
                break;
            case EnemyState.Attack:
                if (canAttack)
                    Attack();
                else
                    currentState = EnemyState.Navigate;
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    void Navigate()
    {
        fireCooldown = 0.1f;
        // agent.SetDestination(targetBase.position);

        // If tower, switch to attack 
        if (canAttack)
            FindNearestTower();

        // If enemy has a turret, also rotate it to give look
        if (turret)
            turret.localRotation = Quaternion.Slerp(turret.localRotation, initialTurretRotation, rotateSpeed * Time.deltaTime);
    }
    void Attack()
    {
        // If we do not have a target or the old target is out of range, turn to navigate mode
        if (attackTarget == null || Vector3.Distance(transform.position, attackTarget.position) > detectionRange)
        {
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
        if (fireCooldown <= 0)
        {
            if (HasLineOfSight(attackTarget))
                Shoot();
            fireCooldown = 1f / fireRate;
            Debug.Log("cooldown:" + fireCooldown);
        }
        else
        {
            fireCooldown -= Time.deltaTime;
        }

    }
    void Shoot()
    {
        if (!canAttack)
            return;
        var bullet = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        if (bulletBehavior)
        {
            var targetTowerTurret = attackTarget.transform;
            if (targetTowerTurret)
                bulletBehavior.SetTarget(targetTowerTurret);
            else
                bulletBehavior.SetTarget(attackTarget);
        }
    }
    void Die()
    {
        if (isEnemyDead)
            return;

        Debug.Log("Die");
        if (destroyPrefab)
            Instantiate(destroyPrefab, transform.position, transform.rotation);

        Destroy(gameObject);
        MoneyManager.Instance.GetMoney(reward);
        isEnemyDead = true;
    }

    // find a nearest tower and set attack target to it, then turn to attack state
    void FindNearestTower()
    {
        // Debug.Log("car is finding");
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        Transform nearestTower = null;
        float nearestDistance = Mathf.Infinity;
        // Iterate through all towers in range and pick the nearest one to attack
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Tower"))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestTower = collider.transform;
                    nearestDistance = distance;
                }
            }
        }
        if (nearestTower)
        {
            attackTarget = nearestTower;
            currentState = EnemyState.Attack;
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (healthSlider)
        {
            healthSlider.value = health;
        }
        if (health <= 0)
        {
            currentState = EnemyState.Die;
            health = 0;
        }
    }
    bool HasLineOfSight(Transform target)
    {
        RaycastHit hit;
        Vector3 direction = (target.position - firePoint.position).normalized;

        if (Physics.Raycast(firePoint.position, direction, out hit, detectionRange))
        {
            if (hit.collider.CompareTag("Tower"))
            {
                return true;
            }
        }
        return false;
    }
    public int GetEnemyDamgeValue()
    {
        return baseDamageValue;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            BulletBehavior bulletBehavior = collision.gameObject.GetComponent<BulletBehavior>();
            if (bulletBehavior)
            {
                TakeDamage(bulletBehavior.GetDamageValue());
                // Debug.Log("Enenemy took"+ bulletBehavior.GetDamgageValue() + "damage");
            }
        }
    }
}
