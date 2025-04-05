using System;
using System.Collections;
using UnityEngine;

public class ChestBehavior : MonoBehaviour
{
    Transform lidTransform;
    public Transform lid; 
    public float openAngle = 120f;
    public float openSpeed = 2f; 
    bool opened = false;
    private Quaternion targetRotation;
    public bool stopDementorSpawning = false;
    public AudioClip openSound;
    EnemySpawner enemySpawner;
    public static bool releaseBalls = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySpawner = FindAnyObjectByType<EnemySpawner>();
        Debug.Log("StartChestBehavior");
        lidTransform = transform.Find("Lid");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile") && !opened) {
            Debug.Log("Touched the chest");
            StartCoroutine(OpenLid());
            AudioSource.PlayClipAtPoint(openSound, lidTransform.position);
            stopDementorSpawning = true;
            opened = true;
            enemySpawner.stopSpawning();
        }
            
    }

    IEnumerator OpenLid() {
        Debug.Log("Opened");
        float timer = 0;
        while (timer < openSpeed) {
            timer += Time.deltaTime;
            lid.transform.Rotate(Vector3.right, openAngle * Time.deltaTime);
            yield return null;
        }
        releaseBalls = true;
    }
}
