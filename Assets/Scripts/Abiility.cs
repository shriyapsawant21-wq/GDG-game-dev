using UnityEngine;
using System.Collections.Generic;

public class Ability : MonoBehaviour
{
    [SerializeField] private KeyCode abilityKey = KeyCode.LeftShift;
    [SerializeField] private float rewindDuration = 3f;

    private List<Vector3> playerPositions = new List<Vector3>();
    private Dictionary<Transform, List<Vector3>> enemyHistories = new Dictionary<Transform, List<Vector3>>();

    private Rigidbody2D rb;
    private bool isRewinding = false;
    public bool IsRewinding => isRewinding;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Time.timeScale == 0 && !isRewinding) return;

        if (Input.GetKeyDown(abilityKey)) StartRewind();
        if (Input.GetKeyUp(abilityKey)) StopRewind();
    }

    void StartRewind()
    {
        isRewinding = true;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
    }

    void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false; 
    }

    void FixedUpdate()
    {
        if (isRewinding) DoRewind();
        else RecordData();
    }

    void RecordData()
    {
        playerPositions.Insert(0, transform.position);
        if (playerPositions.Count > MaxFrames()) playerPositions.RemoveAt(playerPositions.Count - 1);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;
            
            if (!enemyHistories.ContainsKey(enemy.transform))
                enemyHistories.Add(enemy.transform, new List<Vector3>());

            enemyHistories[enemy.transform].Insert(0, enemy.transform.position);

            if (enemyHistories[enemy.transform].Count > MaxFrames())
                enemyHistories[enemy.transform].RemoveAt(enemyHistories[enemy.transform].Count - 1);
        }
    }

    void DoRewind()
    {
        if (playerPositions.Count > 0)
        {
            transform.position = playerPositions[0];
            playerPositions.RemoveAt(0);
        }
        else
        {
            StopRewind(); 
        }

        foreach (var history in enemyHistories)
        {
            if (history.Key != null && history.Value.Count > 0)
            {
                history.Key.position = history.Value[0];
                history.Value.RemoveAt(0);
            }
        }
    }

    int MaxFrames() => Mathf.RoundToInt(rewindDuration / Time.fixedDeltaTime);
}