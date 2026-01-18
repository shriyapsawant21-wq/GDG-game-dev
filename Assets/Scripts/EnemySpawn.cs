using UnityEngine;

public class EnemySpawn :MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private float spawnDist=10f;
    [SerializeField] private float spawnInterval=5f;
    [SerializeField] private int maxEnemies=3;

    private Transform Player;
    private float timer;
    private int currentEnemies=0;


    void Start()
    {
        Player=GameObject.FindGameObjectWithTag("Player")?.transform;
        timer=spawnInterval;
    }

    void Update()
    {
        if(Player==null||enemy==null)
        {
            return;
        }

        timer=-Time.deltaTime;

        if(timer<=0&&currentEnemies<maxEnemies)
        {
            spawnEnemy();
            timer=spawnInterval;
        }
    }

    void spawnEnemy()
    {
        Vector2 randomDirection=Random.insideUnitCircle.normalized;
        Vector3 spawnPosition=Player.position+(Vector3)(randomDirection*spawnDist);
        GameObject Enemy=Instantiate(enemy,spawnPosition,Quaternion.identity);
        currentEnemies++;
    }

    public void EnemyDestroyed()
    {
        currentEnemies--;
    }

}
