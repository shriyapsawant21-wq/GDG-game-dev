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
    public float jumpCutMultplier=0.5f;

    //ground
    public Transform groundCheck;
    public float groundCheckRadies=0.2f;
    public LayerMAsk groundLayer;

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
        
    }

    void FixedUpdate()
    {
        rb.velocity=new Vector2(moveInput*moveSpeed, rb.velocity.y);
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
            rb.velocity= new vector2(rb.velocity.x,jumForce);
            jumpBufferCounter=0f;
            coyoteCounter=0f;
        }
    }

    void HandleJumpCut()
    {
        if(!jumpHeld && rb.velocity.y>0f)
        {
            rb.velocity=new Vector2(rb.velocity.x,rbvelocity.y*jumpCutMultplier);
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
            coyoteCounter-=coyoteTime.deltaTime;
        }
    }

    void HandleJumpBuffer()
    {jumpBufferCounter-=coyoteTime.deltaTime;
        
    }

    void GroundCheck()
    {
        isGrounded=Physics2D.OverlapCircle(groundCheck.position,groundCheckRadies,groundLayer);
    }





    //variable jump height

    //apex modifiers

    //jump buffering

    //coyote time

    //clamped fall speed(dodge while falling)

    //edge detection

    //ledgecatching


}
