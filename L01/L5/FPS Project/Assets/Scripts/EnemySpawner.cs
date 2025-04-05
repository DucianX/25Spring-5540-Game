using UnityEngine;
using System.Collections;
using System.Net.NetworkInformation;
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int maxEnemyCount;
    static bool spawning;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawning = true;
        // InvokeRepeating("SpawnEnemy", 2, 3);
        // Debug.Log("coroutine started " + Time.time); 
        StartCoroutine(SpawnEnemies(3));
    }

   

    void SpawnEnemy() {
        var positionOffset = Random.insideUnitSphere * 5;
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }

    IEnumerator SpawnEnemies(float spawnInterval) {
        // Debug.Log("before yield " + Time.time);
        var enemyCount = GameObject.FindGameObjectsWithTag("Dementor").Length;
        Debug.Log(enemyCount);
        while (true) {
            enemyCount = GameObject.FindGameObjectsWithTag("Dementor").Length;
            Debug.Log(enemyCount);
            if(spawning && enemyCount < maxEnemyCount) {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
            // Debug.Log("after yield " + Time.time);
        }
    }

    public void stopSpawning() {
        spawning = false;
        Debug.Log("Spawn stoped");
    }
}
