using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class BallBehavior : MonoBehaviour
{
    // Inspector configurable properties
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private int maxTargets = 5;
    [SerializeField] private AudioClip ballSound;
    
    // Internal tracking variables
    private int targetsDestroyed = 0;
    private GameObject currentTarget;
    private Rigidbody rb;
    private AudioSource audioSource;
    private TrailRenderer trailRenderer;
    
    void Start()
    {
        // Initialize components
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        trailRenderer = GetComponent<TrailRenderer>();
        
        // Configure audio source for 3D spatial blending
        if (audioSource != null && ballSound != null)
        {
            audioSource.clip = ballSound;
            audioSource.spatialBlend = 1.0f; // Full 3D spatial audio
            audioSource.loop = true;
            audioSource.Play();
        }
        
    }
    
    // If has a currentTarget, run towards it. Else find a new target
    // If the chest is unopened (balls not released), do nothing
    void FixedUpdate()
    {
        if (!ChestBehavior.releaseBalls) return;
        if (currentTarget != null)
        {
            // Calculate direction to target
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            
            // Move directly toward target using Transform.position
            transform.position += direction * moveSpeed * Time.deltaTime;
            
            // Optionally, make the ball face the direction it's moving
            transform.forward = direction;
        }
        else
        {
            // If no target exists, find a new one
            FindNewTarget();
        }
    }
    
    // Find a random target and assign it to the currentTarget
    void FindNewTarget()
    {
        
        // Check if we've reached the maximum number of targets
        if (targetsDestroyed >= maxTargets)
        {
            // Self-destruct if we've reached our limit
            Destroy(gameObject);
            return;
        }
        
        // Find all Dementors in the scene
        GameObject[] dementors = GameObject.FindGameObjectsWithTag("Dementor");
        
        // If no dementors exist, do nothing
        if (dementors.Length == 0)
        {
            currentTarget = null;
            return;
        }
        
        // Select a random dementor as the target
        int randomIndex = Random.Range(0, dementors.Length);
        currentTarget = dementors[randomIndex];
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // Check if we hit a dementor
        if (collision.gameObject.CompareTag("Dementor"))
        {
            // Destroy the dementor
            Destroy(collision.gameObject);
            
            // Increment our counter
            targetsDestroyed++;
            
            // Find a new target
            currentTarget = null;
            FindNewTarget();
        }
    }
}