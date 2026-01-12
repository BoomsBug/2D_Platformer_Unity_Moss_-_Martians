using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public SpriteRenderer frontRenderer;
    public SpriteRenderer sideRenderer;
    public Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] private float movespeed = 5f;
    [SerializeField] private float acceleration = 15f;
    float horizontalMovement;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckY = 1.2f;
    [SerializeField] private float groundCheckX = 2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    [SerializeField] private float fallMultiplier = 2f;
    private bool isGrounded;
    private float coyoteCounter;
    private float jumpBufferCounter;
    private bool canDoubleJump = true;

    [Header("Wall")]
    [SerializeField] private Transform leftWallCheck;
    [SerializeField] private Transform rightWallCheck;
    [SerializeField] private float wallCheckW = 0.3f;
    [SerializeField] private float wallCheckH = 1.2f;
    [SerializeField] private float wallJumpX = 6f;
    [SerializeField] private float wallJumpY = 6f;
    private bool onLeftWall;
    private bool onRightWall;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 0.5f;
    private float attackTimer = 0f;
    [SerializeField] private Transform attackSprite;
    [SerializeField] private SpriteRenderer attackSpriteRenderer;
    [SerializeField] private float attackSwingDuration = 0.25f;
    [SerializeField] private float thrustDuration = 0.25f;
    [SerializeField] private float thrustDistance = 0.7f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSfx;

    [Header("Knockback")]
    [SerializeField] private float knockbackHorizontalForce = 8f;
    [SerializeField] private float knockbackVerticalForce = 4f;

    void Update()
    {
        Vector2 boxSize = new Vector2(groundCheckX, groundCheckY);
        isGrounded = Physics2D.OverlapBox(groundCheck.position, boxSize, 0f, groundLayer);

        onLeftWall = Physics2D.OverlapBox(leftWallCheck.position, new Vector2(wallCheckW, wallCheckH), 0f, groundLayer);
        onRightWall = Physics2D.OverlapBox(rightWallCheck.position, new Vector2(wallCheckW, wallCheckH), 0f, groundLayer);

        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
            canDoubleJump = true;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        // simple accel toward input speed (no momentum keep)
        float targetX = horizontalMovement * movespeed;
        float currentX = Mathf.MoveTowards(rb.linearVelocityX, targetX, acceleration * Time.deltaTime);
        rb.linearVelocity = new Vector2(currentX, rb.linearVelocityY);

        if (!isGrounded && rb.linearVelocityY < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        }

        if (horizontalMovement > 0.1f || horizontalMovement < -0.1f)
        {
            frontRenderer.enabled = false;
            sideRenderer.enabled = true;
            sideRenderer.flipX = horizontalMovement < 0;
            frontRenderer.flipX = horizontalMovement < 0;
        }
        else
        {
            frontRenderer.enabled = true;
            sideRenderer.enabled = false;
        }

        if (jumpBufferCounter > 0 && coyoteCounter > 0)
        {
            DoJump();
            jumpBufferCounter = 0;
        }
        else
        {
            if (jumpBufferCounter > 0)
                jumpBufferCounter -= Time.deltaTime;
        }

        attackTimer -= Time.deltaTime;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpBufferCounter = jumpBufferTime;

            if (!isGrounded && onLeftWall)
            {
                rb.linearVelocity = new Vector2(wallJumpX, wallJumpY);
                coyoteCounter = 0;
                jumpBufferCounter = 0;
                canDoubleJump = false;
            }
            else if (!isGrounded && onRightWall)
            {
                rb.linearVelocity = new Vector2(-wallJumpX, wallJumpY);
                coyoteCounter = 0;
                jumpBufferCounter = 0;
                canDoubleJump = false;
            }
            else if (coyoteCounter > 0)
            {
                DoJump();
                jumpBufferCounter = 0;
            }
            else if (canDoubleJump)
            {
                DoJump();
                canDoubleJump = false;
                jumpBufferCounter = 0;
            }
        }

        if (context.canceled && rb.linearVelocityY > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocityY * 0.5f);
        }
    }

    public void IncreaseDamage(int amount)
    {
        damage += amount;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (attackTimer > 0f) return;
            attackTimer = attackCooldown;

            if (audioSource != null && attackSfx != null)
            {
                audioSource.PlayOneShot(attackSfx);
            }

            if (UnityEngine.Random.value < 0.5f)
            {
                StartCoroutine(AttackSwingRoutine());
            }
            else
            {
                StartCoroutine(AttackThrustRoutine());
            }

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);
            foreach (Collider2D enemyCollider in hitEnemies)
            {
                if (enemyCollider.CompareTag("Enemy"))
                {
                    Enemy enemy = enemyCollider.GetComponent<Enemy>();
                    enemy.TakeDamage(damage);
                }
            }
        }
    }



    private IEnumerator AttackSwingRoutine()
    {
        attackSpriteRenderer.enabled = true;

        float elapsed = 0f;

        float startAngle;
        float endAngle;

        if (frontRenderer.flipX)
        {
            startAngle = 90;
            endAngle   = 225;
            attackSpriteRenderer.flipX = false;
        }
        else
        {
            startAngle = 270;
            endAngle   = 135;
            attackSpriteRenderer.flipX = true;
        }

        while (elapsed < attackSwingDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / attackSwingDuration;
            float angle = Mathf.Lerp(startAngle, endAngle, t);
            attackSprite.localRotation = Quaternion.Euler(0f, 0f, angle);
            yield return null;
        }

        attackSpriteRenderer.enabled = false;
    }
    private IEnumerator AttackThrustRoutine()
    {
    attackSpriteRenderer.enabled = true;

    float dir;
    float thrustAngle;

    if (frontRenderer.flipX)
    {
        dir = -1f;               
        attackSpriteRenderer.flipX = false;
        thrustAngle = 200f;                
    }
    else
    {
        dir = 1f;                       
        attackSpriteRenderer.flipX = true;  
        thrustAngle = 160f;                  
    }

    attackSprite.localRotation = Quaternion.Euler(0f, 0f, thrustAngle);

    Vector3 basePos = attackSprite.localPosition;
    basePos = new Vector3(Mathf.Abs(basePos.x) * dir, basePos.y, basePos.z);
    attackSprite.localPosition = basePos;

    Vector3 startPos = basePos;
    Vector3 endPos   = basePos + new Vector3(dir * thrustDistance, 0f, 0f);

    float elapsed = 0f;
    float half = thrustDuration * 0.5f;

    while (elapsed < thrustDuration)
    {
        elapsed += Time.deltaTime;

        float t = elapsed <= half ? (elapsed / half) : ((thrustDuration - elapsed) / half);
        t = Mathf.Clamp01(t);

        attackSprite.localPosition = Vector3.Lerp(startPos, endPos, t);
        yield return null;
    }

    attackSprite.localPosition = basePos;
    attackSpriteRenderer.enabled = false;
    }

    private void DoJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        coyoteCounter = 0;
    }
    public void ApplyKnockback(Vector2 hitSourcePosition)
    {
        Vector2 dir = ((Vector2)transform.position - hitSourcePosition).normalized;

        if (dir.y < 0f)
            dir.y = 0f;

        rb.linearVelocity = new Vector2(
            dir.x * knockbackHorizontalForce,
            knockbackVerticalForce
        );
    }
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Vector2 boxSize = new Vector2(groundCheckX, groundCheckY);
            Gizmos.DrawWireCube(groundCheck.position, boxSize);
        }

        Gizmos.color = Color.blue;
        if (leftWallCheck != null)
            Gizmos.DrawWireCube(leftWallCheck.position, new Vector2(wallCheckW, wallCheckH));
        if (rightWallCheck != null)
            Gizmos.DrawWireCube(rightWallCheck.position, new Vector2(wallCheckW, wallCheckH));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
