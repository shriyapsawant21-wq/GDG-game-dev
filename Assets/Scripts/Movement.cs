using UnityEngine;

public class Movement: MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private GroundDetector groundDetector;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    

    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    
    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    
    [Header("Gravity")]
    [SerializeField] private float normalGravity = 2.5f;
    [SerializeField] private float jumpCutGravity = 6f;
    [SerializeField] private float fallGravity = 4f;
    
    private float coyoteCounter;
    private float jumpBufferCounter;

    [Header("Wall")]
    private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed=2f;
    private bool isWallJumping;
    private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime=0.2f;
    private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration=0.4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f,16f);

    [Header("flip")]
    private bool isFacingRight=true;
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (inputHandler == null) inputHandler = GetComponent<InputHandler>();
        if (groundDetector == null) groundDetector = GetComponent<GroundDetector>();
    }
    
    void Update()
    {
        HandleJumpInput();
        HandleCoyoteTime();
        HandleJumpBuffer();
        TryJump();
        VariableJump();
        WallSlide();
        WallJump();
        
        if(!isWallJumping)
        {
            Flip();
        }
    }
    
    void FixedUpdate()
    {

        if (inputHandler != null&&!isWallJumping)
        {
            rb.velocity = new Vector2(inputHandler.Horizontal * speed, rb.velocity.y);
        }
    }
    
    void HandleJumpInput()
    {
        if (inputHandler != null && inputHandler.JumpPressed)
        {
            jumpBufferCounter = jumpBufferTime;
        }
    }
    
    void HandleCoyoteTime()
    {
        if (groundDetector != null && groundDetector.IsGrounded)
            coyoteCounter = coyoteTime;
        else
            coyoteCounter -= Time.deltaTime;
    }
    
    void HandleJumpBuffer()
    {
        if (jumpBufferCounter > 0)
            jumpBufferCounter -= Time.deltaTime;
    }
    
    void TryJump()
    {
        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferCounter = 0f;
            coyoteCounter = 0f;
        }
    }
    
    void VariableJump()
    {
        if (rb.velocity.y < 0f)
        {
            rb.gravityScale = fallGravity;
        }
        else if (rb.velocity.y > 0f && 
                 inputHandler != null && 
                 !inputHandler.JumpHeld)
        {
            rb.gravityScale = jumpCutGravity;
        }
        else
        {
            rb.gravityScale = normalGravity;
        }
    }

    private bool IsWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position,0.2f,wallLayer);
    }

    private void WallSlide()
    {
        if(groundDetector==null||inputHandler==null)
        {
            return;
        }
        if(IsWall()&&!groundDetector.IsGrounded&&inputHandler.Horizontal!=0f)
        {
            isWallSliding=true;
            //Debug.Log("touching wall");
            rb.velocity= new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y,-wallSlidingSpeed,float.MaxValue));
        }
        else
        {
            isWallSliding=false;
            //Debug.Log("not touching wall");
        }

    }

    private void Flip()
    {
        if(inputHandler==null)
        {
            return;
        }
        if(isFacingRight&&inputHandler.Horizontal<0f||!isFacingRight&&inputHandler.Horizontal>0f)
        {
            isFacingRight=!isFacingRight;
            Vector3 localScale=transform.localScale;
            localScale.x*=-1f;
            transform.localScale=localScale;
        }
    }

    private void WallJump()
    {
        if(isWallSliding)
        {
            isWallJumping=false;
            wallJumpingDirection=-transform.localScale.x;
            wallJumpingCounter=wallJumpingTime;
            CancelInvoke(nameof(StopWallJumping));

        }
        else
        {
            wallJumpingCounter-=Time.deltaTime;
        }

        if(inputHandler.JumpPressed&&wallJumpingCounter>0f)
        {
            isWallJumping=true;
            rb.velocity= new Vector2(wallJumpingDirection*wallJumpingPower.x,wallJumpingPower.y);
            wallJumpingCounter=0f;

            if(transform.localScale.x!=wallJumpingDirection)
            {
                isFacingRight=!isFacingRight;
                Vector3 localScale=transform.localScale;
                localScale.x*=-1f;
                transform.localScale=localScale;
            }

            Invoke(nameof(StopWallJumping),wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping=false;
    }

}
