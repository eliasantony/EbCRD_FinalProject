using UnityEngine;
using System.Collections;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public GameObject zombiePrefab; // The zombie prefab to spawn
    public float spawnRadius = 20f; // Radius around the player to spawn zombies
    public int numberOfSpawnPoints = 8; // Number of spawn points
    public int initialWaveSize = 5; // Initial number of zombies per wave
    public float timeBetweenWaves = 10f; // Time between waves
    public float waveMultiplier = 1.5f; // Multiplier to increase wave size
    public float spawnInterval = 1f; // Time between each zombie spawn

    [Header("UI Settings")]
    public TextMeshProUGUI waveText; // Reference to the TextMeshProUGUI for wave display
    public TextMeshProUGUI timerText; // Reference to the TextMeshProUGUI for timer display

    private int currentWave = 0; // The current wave number
    private int zombiesToSpawn; // Number of zombies to spawn in the current wave
    private int zombiesRemaining; // Number of zombies remaining in the current wave
    private Transform playerTransform; // Reference to the player's transform
    private Transform[] spawnPoints; // Array of spawn points
    private bool waveInProgress = false; // Flag to track if a wave is in progress

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        CreateSpawnPoints();
        StartCoroutine(WaitAndStartNextWave());
    }

    void Update()
    {
        if (zombiesRemaining <= 0 && !waveInProgress)
        {
            StartCoroutine(WaitAndStartNextWave());
        }
    }

    void CreateSpawnPoints()
    {
        spawnPoints = new Transform[numberOfSpawnPoints];
        for (int i = 0; i < numberOfSpawnPoints; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfSpawnPoints;
            Vector3 spawnPosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spawnRadius;
            spawnPosition += playerTransform.position;
            GameObject spawnPoint = new GameObject("SpawnPoint" + i);
            spawnPoint.transform.position = spawnPosition;
            spawnPoints[i] = spawnPoint.transform;
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
            waveText.text = "Wave " + currentWave;
            timerText.text = "Next wave in: " + Mathf.Ceil(timer).ToString() + "s";
            yield return null;
        }

        timerText.text = "";

        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        for (int i = 0; i < zombiesToSpawn; i++)
        {
            SpawnZombie();
            yield return new WaitForSeconds(spawnInterval);
        }

        waveInProgress = false;
    }

    void SpawnZombie()
    {
        Debug.Log("Spawning zombie");
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(zombiePrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
    }

    public void ZombieKilled()
    {
        zombiesRemaining--;
        Debug.Log("Zombie killed! Zombies remaining: " + zombiesRemaining);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
