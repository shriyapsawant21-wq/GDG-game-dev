using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [Header("Movement")]
    public Transform PointA;
    public Transform PointB;
    public float movespeed=2f;

    private Vector3 nextPosition;

    void Start()
    {
        nextPosition=PointB.position;
    }

    void Update()
    {
        transform.position=Vector3.MoveTowards(transform.position,nextPosition,movespeed*Time.deltaTime);

        if(transform.position==nextPosition)
        {
            nextPosition=(nextPosition==PointA.position)?PointB.position:PointA.position;
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent=transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent=null;
        }
    }
}
