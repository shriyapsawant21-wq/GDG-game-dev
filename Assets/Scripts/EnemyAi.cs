using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float BossSpeed=2f;
    [SerializeField] private float detectionRange=8f;
    [SerializeField] private float AttackRange=0.5f;
    
    [Header("Attack")]
    [SerializeField] private int damage=20;
    [SerializeField] private float AttackCooldown=1f;

    [Header("Spawning")]
    [SerializeField] private float SpawnDist=10f;
    [SerializeField] private GameObject Enemy;

    private Transform Player;
    private bool PlayerDetected =false;
    private bool CanAttack=true;
    private Rigidbody2D rb;


    void Start()
    {
        Player=GameObject.FindGameObjectWithTag("Player")?.transform;
        rb=GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(Player==null)
        {
            return;
        }
        float distance = Vector2.Distance(transform.position,Player.position);

        if(distance<=detectionRange)
        {
            PlayerDetected=true;

        }

        if(PlayerDetected)
        {
            FollowPlayer();

            if(distance<=AttackRange&&CanAttack)
            {
                AttackPlayer();
            }
        }
    }

    void FollowPlayer()
    {
        if(Player==null||rb==null)
        {
            return;
        }

        Vector2 direction=(Player.position-transform.position).normalized;
        rb.velocity=direction*BossSpeed;

        if(direction.x>0)
        {
            transform.localScale=new Vector3(Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        }
        else if(direction.x<0)
        {
            transform.localScale=new Vector3(-Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        }
    }

    void AttackPlayer()
    {
        PlayerHealth playerhealth=Player.GetComponent<PlayerHealth>();

        if(playerhealth!=null)
        {
            playerhealth.TakeDamage(damage);
        }

        CanAttack=false;
        Invoke(nameof(ResetAttack),AttackCooldown);
    }

    void ResetAttack()
    {
        CanAttack=true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player")&&CanAttack)
        {
            AttackPlayer();
        }
    }

    
}
