using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnDist = 12f;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private int maxEnemies = 3;

    private Transform player;
    private float timer;
    private int currentEnemies = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        timer = spawnInterval;
    }

    void Update()
    {
        if (player == null || enemyPrefab == null) return;
        timer -= Time.deltaTime;

        if (timer <= 0 && currentEnemies < maxEnemies)
        {
            SpawnEnemy();
            timer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = (Vector2)player.position + (Random.insideUnitCircle.normalized * spawnDist);
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentEnemies++;
    }

    public void EnemyDestroyed()
    {
        currentEnemies--;
        if (currentEnemies < 0) currentEnemies = 0;
    }
}