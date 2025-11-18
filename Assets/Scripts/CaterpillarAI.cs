using UnityEngine;

public class Caterpillar : MonoBehaviour
{
    public enum SurfaceType { Floor, Ceiling, LeftWall, RightWall }
    [Header("Movement Settings")]
    [SerializeField] private SurfaceType surfaceType = SurfaceType.Floor;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private LayerMask turnMarkerLayer;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool movingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        SetInitialDirection();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & turnMarkerLayer) != 0)
        {
            FlipDirection();
        }
    }

    private void FlipDirection()
    {
        movingRight = !movingRight;
        moveDirection *= -1;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void SetInitialDirection()
    {
        switch (surfaceType)
        {
            case SurfaceType.Floor:
                moveDirection = movingRight ? Vector2.right : Vector2.left;
                transform.rotation = Quaternion.identity;
                break;

            case SurfaceType.Ceiling:
                moveDirection = movingRight ? Vector2.right : Vector2.left;
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;

            case SurfaceType.RightWall:
                moveDirection = movingRight ? Vector2.up : Vector2.down;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;

            case SurfaceType.LeftWall:
                moveDirection = movingRight ? Vector2.down : Vector2.up;
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
        }
    }
}
