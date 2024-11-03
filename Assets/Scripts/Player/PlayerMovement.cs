using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float hAxis;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float jumpForce = 4f;
    private bool facingRight = true;

    private Rigidbody2D rb;
    private Transform playerFeet;
    private LayerMask groundLayer;
    private LayerMask screenLayer;
    private Animator animator;
    private SpriteRenderer playerSprite;

    private bool isDashing;
    private bool canDash = true;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float dashPower = 12f;

    // Cinemachine shake variables
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private float shakeAmplitude = 1f;
    [SerializeField] private float shakeFrequency = 1f;
    [SerializeField] private float shakeDuration = 0.1f;

    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        playerFeet = gameObject.transform.GetChild(1);
        groundLayer = LayerMask.GetMask("Ground");
        screenLayer = LayerMask.GetMask("Screen");
        playerSprite = GetComponentInChildren<SpriteRenderer>();

    }

    void Update()
    {

        if (isDashing)
        {
            return;
        }

        if (IsPlayerInside() && Input.GetKeyDown(KeyCode.E))
        {
            //Time.timeScale = 0f;
            StartCoroutine(UseAnimation());
            
        }

        FlipPlayer();
        Jump();
        hAxis = Input.GetAxisRaw("Horizontal");

        if (canDash && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }

        JumpAnimation();

    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        MovePlayer();

        if (currentPlatform != null)
        {
            Vector3 platformMovement = currentPlatform.position - lastPlatformPosition;
            transform.position += platformMovement;
            lastPlatformPosition = currentPlatform.position;
        }
    }

    private bool IsPlayerInside()
    {

        return Physics2D.OverlapCircle(transform.position, 0.5f, screenLayer);
    }

    private IEnumerator UseAnimation()
    {
        animator.Play("Use");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        animator.Play("Idle");
        GameManager.Instance.WinGame();
    }

    /// <summary>
    /// Movement of the player
    /// </summary>
    private void MovePlayer()
    {
        if (hAxis != 0)
        {
            animator.SetBool("isRunning", true);
        } else
        {
            animator.SetBool("isRunning", false);
        }

        rb.velocity = new Vector2(hAxis * moveSpeed, rb.velocity.y);
        
    }

    /// <summary>
    /// Jump if touching ground, jumps higher if holds key or jumps shorter if release the key
    /// </summary>
    private void Jump()
    {
        
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    /// <summary>
    /// Manages the jumping animation
    /// </summary>
    private void JumpAnimation()
    {
        if (IsGrounded())
        {
            animator.SetBool("isGrounded", true);
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }
    }

    /// <summary>
    /// Checks if player is touching ground
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded() {
        return Physics2D.OverlapCircle(playerFeet.position, 0.1f, groundLayer);
    }

    /// <summary>
    /// Flips player sprite if running in opposite direction
    /// </summary>
    private void FlipPlayer()
    {
        if (facingRight && hAxis < 0 || !facingRight && hAxis > 0)
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    /// <summary>
    /// Allows the player to dash to right or left, with an specified distance and cooldown
    /// </summary>
    /// <returns></returns>
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        animator.SetBool("isDashing", true);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0);


        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = originalGravity;
        isDashing = false;
        animator.SetBool("isDashing", false);
        yield return new WaitForSeconds(dashCooldown);
        playerSprite.color = Color.yellow;
        yield return new WaitForSeconds(0.15f);
        playerSprite.color = Color.white;
        canDash = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Makes the player move with the platform
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            currentPlatform = collision.transform;
            lastPlatformPosition = currentPlatform.position;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Makes the player move with the platform
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            currentPlatform = null;
        }
    }
}
