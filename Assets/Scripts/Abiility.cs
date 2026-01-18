using UnityEngine;
using System.Collections.Generic;

public class Ability : MonoBehaviour
{
    [SerializeField] private KeyCode abilityKey = KeyCode.LeftShift;
    [SerializeField] private float rewindDuration = 3f;
    
    private List<Vector3> positions = new List<Vector3>();
    private bool isRewinding = false;

    // Public property so EnemyAi can see it
    public bool IsRewinding => isRewinding;
    
    void Update()
    {
        if (Input.GetKeyDown(abilityKey)) StartRewind();
        if (Input.GetKeyUp(abilityKey)) StopRewind();
    }
    
    void FixedUpdate()
    {
        if (isRewinding && positions.Count > 0)
        {
            transform.position = positions[0];
            positions.RemoveAt(0);
        }
        else if (!isRewinding)
        {
            positions.Insert(0, transform.position);
            if (positions.Count > Mathf.RoundToInt(rewindDuration / Time.fixedDeltaTime))
                positions.RemoveAt(positions.Count - 1);
        }
    }
    
    void StartRewind() { isRewinding = true; }
    void StopRewind() { isRewinding = false; }
}