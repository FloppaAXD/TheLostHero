using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float wallJumpHorizontalForce = 8f; 
    [SerializeField] private float wallJumpVerticalForce = 12f;  
    [SerializeField] private float wallJumpControlDelay = 0.2f;  


    [SerializeField] private int maxJumps = 1; //может быть прыжков будет больше

    [Header("Ground & Wall Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float wallCheckDistance = 0.3f;

    [Header("Wall Slide Settings")]
    [SerializeField] private float wallSlideSpeed = 1.5f;     
    [SerializeField] private float wallStickTime = 0.2f;      
    private float wallStickTimer;                             

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private int jumpCount;
    private float moveInput;
    private bool isFacingRight = true;
    private bool canMove = true;
    private float wallJumpTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove)
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0)
                canMove = true;
        }

        moveInput = Input.GetAxisRaw("Horizontal");
        CheckGround();
        CheckWall();

        if (isGrounded)
        {
            jumpCount = 0;
        }

        // Прыжки
        if (Input.GetButtonDown("Jump"))
        {
            if (isTouchingWall && !isGrounded)
            {
                WallJump();
            }
            else if (isGrounded || jumpCount < maxJumps - 1)
            {
                Jump();
                jumpCount++;
            }
        }

        HandleWallSlide();
        FlipCheck();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Vector2 velocity = rb.linearVelocity;
            velocity.x = moveInput * moveSpeed;
            rb.linearVelocity = new Vector2(velocity.x, rb.linearVelocity.y);
        }
    }

    private void Jump()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.y = jumpForce;
        rb.linearVelocity = velocity;
    }

    private void WallJump()
    {
        int wallDir = isFacingRight ? -1 : 1;
        Vector2 jumpDir = new Vector2(wallDir, 1).normalized;
        rb.linearVelocity = jumpDir * wallJumpVerticalForce;

        canMove = false;
        wallJumpTimer = wallJumpControlDelay;
    }


    private void HandleWallSlide()
    {
        // Проверяем, можно ли скользить
        if (isTouchingWall && !isGrounded && moveInput != 0)
        {
            // Время “зависания” перед скольжением
            if (wallStickTimer < wallStickTime)
            {
                wallStickTimer += Time.deltaTime;
                // Временно “подвешиваем” игрока
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, 0));
                isWallSliding = false;
            }
            else
            {
                // Начинаем скольжение вниз
                isWallSliding = true;
                if (rb.linearVelocity.y < -wallSlideSpeed)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
                }
            }
        }
        else
        {
            // Сбрасываем таймер и состояние
            wallStickTimer = 0f;
            isWallSliding = false;
        }
    }

    private void CheckGround()
    {
        Vector2 checkPos = groundCheckPoint ? (Vector2)groundCheckPoint.position : (Vector2)transform.position;
        Vector2 boxSize = new Vector2(0.8f, 0.1f);
        isGrounded = Physics2D.BoxCast(checkPos, boxSize, 0f, Vector2.down, groundCheckDistance, groundLayer);
    }

    private void CheckWall()
    {
        Vector2 checkPos = wallCheckPoint ? (Vector2)wallCheckPoint.position : (Vector2)transform.position;
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        isTouchingWall = Physics2D.Raycast(checkPos, direction, wallCheckDistance, groundLayer);
    }

    private void FlipCheck()
    {
        if (moveInput > 0 && !isFacingRight)
            Flip();
        else if (moveInput < 0 && isFacingRight)
            Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheckPoint)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(groundCheckPoint.position, new Vector3(0.8f, 0.1f, 0f));
        }

        if (wallCheckPoint)
        {
            Gizmos.color = Color.cyan;
            Vector3 dir = isFacingRight ? Vector3.right : Vector3.left;
            Gizmos.DrawLine(wallCheckPoint.position, wallCheckPoint.position + dir * wallCheckDistance);
        }
    }
}
