using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    [Header("Controlers")]
    [SerializeField] private FadeController fadeController;

    private float stepTimer = 0f;
    [SerializeField] private float stepInterval = 0.20f; // частота шагов (можно менять)
    private bool wasGroundedLastFrame = false;


    private Animator HorseAnimation;
    private Rigidbody2D rb;
    private PlayerSFX sfx;
    private PlayerParticles particles;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private int jumpCount;
    private float moveInput;
    private bool isFacingRight = true;
    private bool canMove = true;
    private bool movementLocked = false;
    private float wallJumpTimer;
    private bool isDead = false;
    private bool isTransitioning = false;


    public bool CanMove
    {
        get => canMove;
        set => canMove = value;
    }


    private IEnumerator Start()
    {
        FadeController fade = Object.FindFirstObjectByType<FadeController>();
        if (fade != null)
            yield return StartCoroutine(fade.FadeIn());
    }
    void Awake()
    {
        sfx = GetComponent<PlayerSFX>();
        rb = GetComponent<Rigidbody2D>();
        HorseAnimation = GetComponent<Animator>();
        particles = GetComponent<PlayerParticles>();
    }

    void Update()
    {

        if (isDead || movementLocked) return;

        if (!canMove)
        {
            wallJumpTimer -= Time.deltaTime;
            if (wallJumpTimer <= 0)
                canMove = true;
        }

        moveInput = Input.GetAxisRaw("Horizontal");
        CheckGround();
        CheckWall();

        HandleWallSlide();
        FlipCheck();
        UpdateAnimations();
        HandleStepSounds();
        HandleRunDust();

        if (isGrounded)
        {
            jumpCount = 0;
        }

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
        FlipCheck();
    }

    void FixedUpdate()
    {
        if (isDead || movementLocked) return;
        if (canMove)
        {
            Vector2 velocity = rb.linearVelocity;
            velocity.x = moveInput * moveSpeed;
            rb.linearVelocity = new Vector2(velocity.x, rb.linearVelocity.y);
        }
    }

    private void UpdateAnimations()
    {
        HorseAnimation.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
        HorseAnimation.SetBool("IsGrounded", isGrounded);
        HorseAnimation.SetBool("IsWallSliding", isWallSliding);
        HorseAnimation.SetFloat("YVelocity", rb.linearVelocity.y);
        HorseAnimation.SetBool("IsDead", isDead);
    }

    private void Jump()
    {
        if (isDead) return;
        Vector2 velocity = rb.linearVelocity;
        velocity.y = jumpForce;
        rb.linearVelocity = velocity;

        sfx?.PlayJump();
    }

    private void WallJump()
    {
        if (isDead) return;
        // Определяем, в какую сторону смотреть после прыжка
        int wallDir = isFacingRight ? -1 : 1;

        // Задаём скорость отталкивания
        rb.linearVelocity = new Vector2(wallDir * wallJumpHorizontalForce, wallJumpVerticalForce);

        sfx?.PlayJump();

        // Небольшой антиприлипательный таймер
        canMove = false;
        wallJumpTimer = wallJumpControlDelay;

        // Разворачиваем спрайт в сторону прыжка
        if (isFacingRight && wallDir < 0 || !isFacingRight && wallDir > 0)
            Flip();

        // Сброс состояния скольжения
        isWallSliding = false;
    }


    private void HandleWallSlide()
    {
        if (isTouchingWall && !isGrounded)
        {
            isWallSliding = true;

            sfx.StartWallSlide();

            if (wallStickTimer < wallStickTime)
            {
                wallStickTimer += Time.deltaTime;
            
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, 0));

                
            }
            else
            {
                
                if (rb.linearVelocity.y < -wallSlideSpeed)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, -wallSlideSpeed);
                }
            }
        }
        else
        {
            sfx.StopWallSlide();

            wallStickTimer = 0f;
            isWallSliding = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Danger") && !isDead)
        {
            Die();
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("LevelExit") && !isDead && !isTransitioning)
        {
            HorseAnimation.SetBool("IsGrounded", false);
            HorseAnimation.SetFloat("YVelocity", 0f);
            HorseAnimation.SetBool("IsWallSliding", false);
            HorseAnimation.SetFloat("Speed", 0f);
            isTransitioning = true;
            movementLocked = true;
            rb.linearVelocity = Vector2.zero;
            StartCoroutine(LoadNextLevel());
        }
    }
    private void Die()
    {
        if (isDead) return;
        isDead = true;

        sfx?.PlayDeath();
        movementLocked = true;
        canMove = false;
        rb.gravityScale = 1;
        rb.linearVelocity = Vector2.zero;

        if (HorseAnimation != null)
        {
            HorseAnimation.SetBool("IsGrounded", false);
            HorseAnimation.SetFloat("YVelocity", 0f);
            HorseAnimation.SetBool("IsWallSliding", false);
            HorseAnimation.SetFloat("Speed", 0f);

            //Устанавливаем триггер смерти
            HorseAnimation.ResetTrigger("IsDead"); // на всякий случай
            HorseAnimation.SetTrigger("IsDead");
            HorseAnimation.Play("PlayerDeadLol", 0, 0f);

            StartCoroutine(PlayDeathAndRestart());
        }


    }

    private IEnumerator PlayDeathAndRestart()
    {
        yield return null;

        AnimatorClipInfo[] clipInfo = HorseAnimation.GetCurrentAnimatorClipInfo(0);
        float clipLength = 1f;
        if (clipInfo.Length > 0)
            clipLength = clipInfo[0].clip.length;


        FadeController fade = Object.FindFirstObjectByType<FadeController>(); //тут новый код из другого скрипта

        if (fade != null)
            StartCoroutine(fade.FadeOut());

        yield return new WaitForSeconds(clipLength + 1f);




        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private IEnumerator LoadNextLevel()
    {
        if (fadeController != null)
            yield return StartCoroutine(fadeController.FadeOut());
        else
            Debug.LogWarning("FadeController не назначен на игроке!");

        yield return new WaitForSeconds(0.5f);

        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            Debug.Log("Это был последний уровень!");
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
        if (isDead) return;
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
    private void HandleStepSounds()
    {
        // ---- 1. Звук приземления ----
        if (isGrounded && !wasGroundedLastFrame)
        {
            sfx?.PlayRunStep();      // момент касания земли
            stepTimer = stepInterval; // чтобы сразу не играли два звука подряд
        }

        // ---- 2. Звуки шага во время движения ----
        if (isGrounded && !isDead && Mathf.Abs(rb.linearVelocity.x) > 0.1f && !isWallSliding)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                sfx?.PlayRunStep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            // сброс таймера если игрок остановился
            stepTimer = stepInterval;
        }

        wasGroundedLastFrame = isGrounded;
    }

    void HandleRunDust()
    {
        bool shouldDust = isGrounded && Mathf.Abs(rb.linearVelocity.x) > 0.1f; // есть движение

        particles?.EnableRunDust(shouldDust);
        

    }


}
