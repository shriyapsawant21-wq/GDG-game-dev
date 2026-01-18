using UnityEngine;

public class EnemyAi
{
    [Header("Movement")]
    [SerializeField] private float BossSpeed=2f;
    [SerializeField] private float detectionRange=8f;
    [SerializeField] private float AttackRange=0.5f;
    

    private Transform Player;
    private bool PlayerDetected =false;
    private bool CanAttack=true;
    private Rigidbody2D rb;


    void Start()
    {
        Player=GameObject.FindGAmeObjectWithTag("Player")?.transform;
        rb=GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position,Player.position);

        if(distance<=detectionRange)
        {
            PlayerDetected=true;

        }

        if(PlayerDetected)
        {
            FollowPLayer();

            if(distance<=AttackRange&&CanAttack)
            {
                AttackPlayer();
            }
        }
    }

    void FollowPLayer()
    {
        
    }

    void AttackPlayer()
    {
        
    }


}
