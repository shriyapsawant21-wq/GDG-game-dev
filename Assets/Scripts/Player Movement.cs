using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerMovement: MonoBehaviour
{

    private Rigidbody2D rb;
    public float speed=5f;
    private float movementX;
    public float jump;
    public bool isJumping;


    void Start()
    {
        rb=GetComponent<Rigidbody2D>();

        if (rb==null)
        {
            Debug.LogError("Rigidbody not founf.");
        }
    }

    public void OnMove(InputValue value)
    {
        Vector2 inputVector=value.Get<Vector2>();

        movementX=inputVector.x;
    }

    void Update()
    {
        Jump();
    }

    void FixedUpdate()
    {
        Vector2 movement=new Vector2(movementX*speed,rb.linearVelocity.y);
        rb.linearVelocity=movement;
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(rb.linearVelocity.x,jump));
            isJumping=true;
        }
    }




    //variable jump height

    //apex modifiers

    //jump buffering

    //coyote time

    //clamped fall speed(dodge while falling)

    //edge detection

    //ledgecatching


}
