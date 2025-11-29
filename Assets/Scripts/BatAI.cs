using UnityEngine;

public class Bat : MonoBehaviour
{
    public enum MovementType { Horizontal, Vertical }

    [Header("Movement Settings")]
    [SerializeField] private MovementType movementType = MovementType.Horizontal;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private LayerMask turnMarkerLayer;

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool movingPositive = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        SetInitialDirection();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & turnMarkerLayer.value) != 0)
        {
            FlipDirection();
        }
    }

    private void SetInitialDirection()
    {
        switch (movementType)
        {
            case MovementType.Horizontal:
                moveDirection = movingPositive ? Vector2.right : Vector2.left;
                if (spriteRenderer != null)
                    spriteRenderer.flipX = moveDirection.x > 0;
                break;

            case MovementType.Vertical:
                moveDirection = movingPositive ? Vector2.up : Vector2.down;
                break;
        }
    }

    private void FlipDirection()
    {
        movingPositive = !movingPositive;
        moveDirection *= -1;
        if (movementType == MovementType.Horizontal && spriteRenderer != null)
            spriteRenderer.flipX = moveDirection.x > 0;
    }
}