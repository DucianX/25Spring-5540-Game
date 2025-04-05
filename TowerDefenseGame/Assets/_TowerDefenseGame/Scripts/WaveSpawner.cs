using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave {
        public GameObject[] enemyPrefabs;
        public int enemyCount = 5;
        public float spawnInterval = 2f;        
    }
    public Wave[] waves;
    public float timeBetweenWaves = 5;
    public int currentWaveIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.DeleteKey("LastWave");
        currentWaveIndex = PlayerPrefs.GetInt("LastWave", 0);
        Debug.Log("Last Wave index retrieved as " + currentWaveIndex);
        StartCoroutine(ReleaseWaves());
    }
    // Release multiple waves
    IEnumerator ReleaseWaves() {
        while(currentWaveIndex < waves.Length) {
            Debug.Log("Wave " + (currentWaveIndex + 1) + "Incoming");
            // Yield to wait a few seconds, at the same time the game keeps running
            yield return new WaitForSeconds(timeBetweenWaves);
            Debug.Log("Spawning enemies in this wave...");
            // After resume this coroutine, we start SpawnWave coroutine
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            // Wait till Are.. == true, then go to next 
            Debug.Log("Wait till all enemies are gone");
            // Wait till all enemies are gone, then go into next loop and spawn some more
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
            currentWaveIndex++;

            // Save data into harddrive, such as Game Progress
            PlayerPrefs.SetInt("LastWave", currentWaveIndex);
            PlayerPrefs.Save();
            Debug.Log("Last wave is " + currentWaveIndex);
        }
    }
    // Spawn a single wave
    IEnumerator SpawnWave(Wave wave) {
        for (int i = 0; i < wave.enemyCount; i ++) {
            int enemyIndex = Random.Range(0, wave.enemyPrefabs.Length);
            GameObject enemyPrefab = wave.enemyPrefabs[enemyIndex];
            SpawnEnemy(enemyPrefab);
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }
    // Spawn a single enemy
    void SpawnEnemy(GameObject enemyPrefab) {
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }

    /* bool AreAllEnemiesDestoryed() {
        return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
    } */
}
