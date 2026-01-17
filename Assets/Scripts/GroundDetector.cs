using UnityEngine;

public class GroundDetector: MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    
    public bool IsGrounded { get; private set; }
    
    void Update()
    {
        if (groundCheck != null)
        {
            IsGrounded = Physics2D.OverlapCircle(
                groundCheck.position, 
                groundCheckRadius, 
                groundLayer
            );
        }
    }
    
}