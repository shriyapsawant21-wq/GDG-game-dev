using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float wallCheckDist = 0.8f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Combat")]
    [SerializeField] private Vector2 attackRangeLimits = new Vector2(1.2f, 3.5f);
    [SerializeField] private float retreatDistance = 5f;
    [SerializeField] private float cooldownTime = 1.5f;

    private Transform player;
    private Rigidbody2D rb;
    private float currentAttackRange;
    private float cooldownTimer;
    private Ability playerAbility;

    private enum State { Chasing, Attacking, Retreating, Cooldown }
    private State currentState = State.Chasing;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerAbility = player.GetComponent<Ability>();
        rb = GetComponent<Rigidbody2D>();
        RandomizeRange();
    }

    void Update()
    {
        if (playerAbility != null && playerAbility.IsRewinding)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        float dist = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Chasing:
                MoveAndJump(dist);
                if (dist <= currentAttackRange) currentState = State.Attacking;
                break;

            case State.Attacking:
                player.GetComponent<PlayerHealth>()?.TakeDamage(20);
                currentState = State.Retreating;
                break;

            case State.Retreating:
                MoveAway();
                if (dist >= retreatDistance) { currentState = State.Cooldown; cooldownTimer = cooldownTime; }
                break;

            case State.Cooldown:
                rb.velocity = new Vector2(0, rb.velocity.y);
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0) { RandomizeRange(); currentState = State.Chasing; }
                break;
        }
    }

    void MoveAndJump(float dist)
    {
        float dirX = player.position.x > transform.position.x ? 1 : -1;
        rb.velocity = new Vector2(dirX * chaseSpeed, rb.velocity.y);

        bool hittingWall = Physics2D.Raycast(transform.position, new Vector2(dirX, 0), wallCheckDist, groundLayer);
        bool isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.2f, groundLayer);
        
        if (isGrounded && (hittingWall || player.position.y > transform.position.y + 2f))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        
        transform.localScale = new Vector3(dirX > 0 ? Mathf.Abs(transform.localScale.x) : -Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void MoveAway()
    {
        float dirX = player.position.x > transform.position.x ? -1 : 1;
        rb.velocity = new Vector2(dirX * chaseSpeed, rb.velocity.y);
    }

    void RandomizeRange() => currentAttackRange = Random.Range(attackRangeLimits.x, attackRangeLimits.y);

    private void OnDestroy()
    {
        FindObjectOfType<EnemySpawn>()?.EnemyDestroyed();
    }
}