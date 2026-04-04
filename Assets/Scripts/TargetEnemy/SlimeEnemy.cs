using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SlimeEnemy : EnemyBase
{
    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 1.5f;
    [SerializeField] private float patrolTime = 1f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpCooldown = 1.5f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform edgeCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;

    private float patrolTimer;
    private float jumpTimer;

    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        patrolTimer = patrolTime;
    }

    protected override void Update()
    {
        base.Update();

        CheckGround();

        if (isPlayerDetected)
        {
            HandleChase();
        }
        else
        {
            Patrol();
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f, groundLayer);
    }

    // 🔥 ПРОВЕРКА КРАЯ ПЛАТФОРМЫ
    private bool IsGroundAhead()
    {
        return Physics2D.Raycast(edgeCheck.position, Vector2.down, 0.3f, groundLayer);
    }

    private void Patrol()
    {
        patrolTimer -= Time.deltaTime;

        // если впереди нет земли — разворачиваемся
        if (!IsGroundAhead() && isGrounded)
        {
            Flip();
            patrolTimer = patrolTime;
        }

        rb.linearVelocity = new Vector2(facingDirection * patrolSpeed, rb.linearVelocity.y);

        if (patrolTimer <= 0)
        {
            Flip();
            patrolTimer = patrolTime;
        }
    }

    private void HandleChase()
    {
        if (player == null) return;

        float dir = Mathf.Sign(player.position.x - transform.position.x);

        // разворот к игроку
        if (dir != facingDirection)
        {
            Flip();
        }

        jumpTimer -= Time.deltaTime;

        if (jumpTimer <= 0 && isGrounded)
        {
            rb.linearVelocity = new Vector2(dir * moveSpeed, jumpForce);
            jumpTimer = jumpCooldown;
        }
    }

    private void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 0.2f);
        }

        if (edgeCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + Vector3.down * 0.3f);
        }
    }
}