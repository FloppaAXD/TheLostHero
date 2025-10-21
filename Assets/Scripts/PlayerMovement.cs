using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float groundCheckDistance = 0.1f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheckPoint;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        CheckGround();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
            print("1");
        }
    }

    void FixedUpdate()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveInput * moveSpeed;
        rb.linearVelocity = velocity;
    }

    private void Jump()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.y = jumpForce;
        rb.linearVelocity = velocity;
    }

    private void CheckGround()
    {
        Vector2 checkPosition = groundCheckPoint
            ? (Vector2)groundCheckPoint.position
            : (Vector2)transform.position;

        isGrounded = Physics2D.Raycast(checkPosition, Vector2.down, groundCheckDistance, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        Vector3 checkPos = groundCheckPoint ? groundCheckPoint.position : transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(checkPos, checkPos + Vector3.down * groundCheckDistance);
    }
}