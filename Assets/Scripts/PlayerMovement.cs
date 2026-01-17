using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movement")]
    public float speed = 5f;
    private float movementX;

    [Header("Jump")]
    public float jumpForce = 12f;
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool isGrounded;
    private float coyoteCounter;
    private float jumpBufferCounter;
    private bool jumpHeld;

    [Header("Gravity")]
    public float normalGravity = 2.5f;
    public float jumpCutGravity = 6f;
    public float fallGravity = 4f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Rigidbody2D not founf.");
    }

    void Update()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
            jumpHeld = true;
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpHeld = false;
        }
        
        GroundCheck();
        HandleCoyoteTime();
        HandleJumpBuffer();
        TryJump();
        VariableJump();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movementX * speed, rb.velocity.y);
    }

    void TryJump()
    {
        if (jumpBufferCounter > 0f && (isGrounded || coyoteCounter > 0f))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
        }
    }

    void HandleCoyoteTime()
    {
        if (isGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;
    }

    void HandleJumpBuffer()
    {
        if (jumpBufferCounter > 0)
            jumpBufferCounter -= Time.deltaTime;
    }

    void GroundCheck()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            );
        }
    }

    void VariableJump()
    {
        if (rb.velocity.y < 0f)
        {
            rb.gravityScale = fallGravity;
        }
        else if (rb.velocity.y > 0f && !jumpHeld)
        {
            rb.gravityScale = jumpCutGravity;
        }
        else
        {
            rb.gravityScale = normalGravity;
        }
    }
    
}