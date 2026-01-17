using UnityEngine;

public class Ability : MonoBehaviour
{
    [SerializeField] private KeyCode abilityKey = KeyCode.LeftShift;
    [SerializeField] private GameObject playerFuture;
    [SerializeField] private float opacity = 0.5f;

    private GameObject player;
    private Vector3 futureSpawnPosition;
    private bool abilityActive = false;
    private SpriteRenderer originalSprite;
    private Color originalColor;

    void Start()
    {
        originalSprite = GetComponent<SpriteRenderer>();
        if (originalSprite != null)
        {
            originalColor = originalSprite.color;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(abilityKey) && !abilityActive)
        {
            ActivateFuture();
        }
        
        if (Input.GetKeyUp(abilityKey) && abilityActive)
        {
            DeactivateFuture();
        }
    }

    void ActivateFuture()
    {
        abilityActive = true;
        futureSpawnPosition = transform.position;

        if (playerFuture != null)
        {
            player = Instantiate(playerFuture, futureSpawnPosition, transform.rotation);

            GetComponent<Movement>().enabled = false;
            
            if (originalSprite != null)
            {
                Color transparentColor = originalColor;
                transparentColor.a = opacity * 0.3f;
                originalSprite.color = transparentColor;
            }

            Movement futureMovement = player.GetComponent<Movement>();
            if (futureMovement != null)
            {
                futureMovement.enabled = true;
                
                InputHandler futureInput = player.GetComponent<InputHandler>();
                if (futureInput == null)
                {
                    futureInput = player.AddComponent<InputHandler>();
                }
                futureMovement.inputHandler = futureInput;
            }

            SpriteRenderer futureSprite = player.GetComponent<SpriteRenderer>();
            if (futureSprite != null)
            {
                Color futureColor = futureSprite.color;
                futureColor.a = opacity;
                futureSprite.color = futureColor;
            }

            Collider2D playerCollider = GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                playerCollider.isTrigger = true;
            }
        }
    }

    void DeactivateFuture()
    {
        abilityActive = false;

        if (player != null)
        {
            transform.position = player.transform.position;
            Destroy(player);
            player = null;
        }

        GetComponent<Movement>().enabled = true;
        
        if (originalSprite != null)
        {
            originalSprite.color = originalColor;
        }

        Collider2D playerCollider = GetComponent<Collider2D>();
        if (playerCollider != null)
        {
            playerCollider.isTrigger = false;
        }
    }
}