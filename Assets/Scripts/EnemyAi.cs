using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float retreatSpeed = 2.5f;
    [SerializeField] private float detectionRange = 10f;

    [Header("Combat Logic")]
    [SerializeField] private int damage = 20;
    [SerializeField] private float attackCooldownTime = 2.0f; 
    [SerializeField] private float retreatDistance = 5f;      
    [SerializeField] private Vector2 attackRangeLimits = new Vector2(1.0f, 3.5f); 

    private Transform player;
    private Rigidbody2D rb;
    private Ability playerAbility; 
    private float currentAttackRange;
    private float cooldownTimer = 0f;

    private enum State { Idle, Chasing, Attacking, Retreating, Cooldown }
    [SerializeField] private State currentState = State.Idle;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null) playerAbility = player.GetComponent<Ability>();
        
        rb = GetComponent<Rigidbody2D>();
        RandomizeAttackRange();
    }

    void Update()
    {
        if (player == null) return;

        if (playerAbility != null && playerAbility.IsRewinding)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Idle:
                if (distanceToPlayer <= detectionRange) currentState = State.Chasing;
                break;

            case State.Chasing:
                MoveTowardsPlayer(chaseSpeed);
                if (distanceToPlayer <= currentAttackRange) currentState = State.Attacking;
                break;

            case State.Attacking:
                PerformAttack();
                currentState = State.Retreating;
                break;

            case State.Retreating:
                MoveAwayFromPlayer(retreatSpeed);
                if (distanceToPlayer >= retreatDistance)
                {
                    cooldownTimer = attackCooldownTime;
                    currentState = State.Cooldown;
                }
                break;

            case State.Cooldown:
                rb.velocity = new Vector2(0, rb.velocity.y);
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0)
                {
                    RandomizeAttackRange();
                    currentState = State.Chasing;
                }
                break;
        }

        if (Mathf.Abs(rb.velocity.x) > 0.1f) FlipSprite(rb.velocity.x);
    }

    void MoveTowardsPlayer(float speed)
    {
        float dirX = (player.position.x > transform.position.x) ? 1 : -1;
        rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
    }

    void MoveAwayFromPlayer(float speed)
    {
        float dirX = (player.position.x > transform.position.x) ? -1 : 1;
        rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
    }

    void PerformAttack()
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null) health.TakeDamage(damage);
    }

    void RandomizeAttackRange()
    {
        currentAttackRange = Random.Range(attackRangeLimits.x, attackRangeLimits.y);
    }

    void FlipSprite(float velX)
    {
        transform.localScale = new Vector3(velX > 0 ? Mathf.Abs(transform.localScale.x) : -Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void OnDestroy()
    {
        EnemySpawn spawner = FindFirstObjectByType<EnemySpawn>();
        if (spawner != null) spawner.EnemyDestroyed();
    }
}