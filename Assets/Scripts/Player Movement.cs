using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerMovement: MonoBehaviour
{

    private Rigidbody2D rb;
    //movement
    public float speed=5f;
    private float movementX;

    //jump
    public float jumpForce;
    public float coyoteTime=0.1f;
    public float jumpBufferTime=0.1f;
    public float jumpCutMultiplier=0.5f;

    //ground
    public Transform groundCheck;
    public float groundCheckRadius=0.2f;
    public LayerMask groundLayer;

    private bool isGrounded;
    private float coyoteCounter;
    private float jumpBufferCounter;
    private bool jumpHeld;



    void Start()
    {
        rb=GetComponent<Rigidbody2D>();

        if (rb==null)
        {
            Debug.LogError("Rigidbody not founf.");
        }
    }

    void Update()
    {
        GroundCheck();
        HandleCoyotetime();
        HandleJumpBuffer();
        TryJump();
        HandleJumpCut();
    }

    void FixedUpdate()
    {
        rb.linearVelocity=new Vector2(movementX*speed, rb.linearVelocity.y);
    }

    public void OnMove(InputValue value)
    {
        movementX=value.Get<Vector2>().x;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpBufferCounter=jumpBufferTime;
            jumpHeld=true;
        }
        else
        {
            jumpHeld=false;
        }
    }


    void TryJump()
    {
        if(jumpBufferCounter>0f && coyoteCounter>0f)
        {
            rb.linearVelocity= new Vector2(rb.linearVelocity.x,jumpForce);
            jumpBufferCounter=0f;
            coyoteCounter=0f;
        }
    }

    void HandleJumpCut()
    {
        if(!jumpHeld && rb.linearVelocity.y>0f)
        {
            rb.linearVelocity=new Vector2(rb.linearVelocity.x,rb.linearVelocity.y*jumpCutMultiplier);
        }
    }

    void HandleCoyotetime()
    {
        if(isGrounded)
        {
            coyoteCounter=coyoteTime;
        }
        else
        {
            coyoteCounter-=Time.deltaTime;
        }
    }

    void HandleJumpBuffer()
    {jumpBufferCounter-=Time.deltaTime;
        
    }

    void GroundCheck()
    {
        isGrounded=Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);
    }





    //variable jump height

    //apex modifiers

    //jump buffering

    //coyote time

    //clamped fall speed(dodge while falling)

    //edge detection

    //ledgecatching


}
