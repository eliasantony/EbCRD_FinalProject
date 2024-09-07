using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public GameObject zombiePrefab;
    public float spawnRadius = 20f;
    public int initialWaveSize = 5;
    public float timeBetweenWaves = 20f;
    public float waveMultiplier = 1.5f;
    public float spawnInterval = 1f;

    private int currentWave = 0;
    private int zombiesToSpawn;
    private int zombiesRemaining;
    private Transform playerTransform;
    private bool waveInProgress = false;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(WaitAndStartNextWave());
    }

    private void Update()
    {
        // Update the spawn position to follow the player
        transform.position = playerTransform.position;

        if (zombiesRemaining <= 0 && !waveInProgress)
        {
            StartCoroutine(WaitAndStartNextWave());
        }
    }

    IEnumerator WaitAndStartNextWave()
    {
        waveInProgress = true;
        currentWave++;
        zombiesToSpawn = Mathf.RoundToInt(initialWaveSize * Mathf.Pow(waveMultiplier, currentWave - 1));
        zombiesRemaining = zombiesToSpawn;

        for (float timer = timeBetweenWaves; timer > 0; timer -= Time.deltaTime)
        {
            UIManager.instance.UpdateWaveNumber(currentWave);
            UIManager.instance.UpdateWaveTimer(timer);
            yield return null;
        }

        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        for (int i = 0; i < zombiesToSpawn; i++)
        {
            Vector3 spawnPosition = FindValidSpawnPosition();
            if (spawnPosition != Vector3.zero) // If a valid position is found
            {
                Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                zombiesRemaining--; // Decrement the count if no valid spawn position is found
                Debug.LogWarning("No valid spawn position found. Reducing remaining zombie count.");
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        waveInProgress = false;
    }

    Vector3 FindValidSpawnPosition()
    {
        for (int attempts = 0; attempts < 30; attempts++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
            randomDirection += playerTransform.position;
            NavMeshHit hit;
            // Use NavMesh.SamplePosition to check if the point is on the NavMesh
            if (NavMesh.SamplePosition(randomDirection, out hit, 2f, NavMesh.AllAreas))
            {
                if (hit.position.y < 1.5)
                {
                    if (Vector3.Distance(hit.position, playerTransform.position) > 5f) 
                    {
                        return hit.position;
                    }
                }
            }
        }
        Debug.LogWarning("Failed to find a valid spawn position after 30 attempts.");
        return Vector3.zero;
    }

    public void ZombieKilled()
    {
        zombiesRemaining--;
        
        // Start countdown to next wave if all zombies are dead
        if (zombiesRemaining <= 0 && !waveInProgress)
        {
            StartCoroutine(WaitAndStartNextWave());
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerTransform ? playerTransform.position : transform.position, spawnRadius);
    }
}