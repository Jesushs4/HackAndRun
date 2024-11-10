using System.Collections;
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

    private bool isHacking;
    private bool isCrouching;

    private Transform currentPlatform;
    private Vector3 lastPlatformPosition;
    
    public bool IsDashing { get => isDashing; set => isDashing = value; }

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
        
        if (!CanMove())
        {
            return;
        }
        Crouch();
        StartHacking();
        FlipPlayer();
        Jump();
        StartDash();
        JumpAnimation();
        Attack();


    }

    private void FixedUpdate()
    {
        if (!CanMove())
        {
            return;
        }
        MoveWithPlatform();

        MovePlayer();
        
        
    }

    /// <summary>
    /// Checks if the player is inside the hack panel
    /// </summary>
    /// <returns></returns>
    private bool IsPlayerInsideHack()
    {
        return Physics2D.OverlapCircle(transform.position, 0.5f, screenLayer);
    }

    /// <summary>
    /// Animation for using the hack panel
    /// </summary>
    /// <returns></returns>
    private IEnumerator UseAnimation()
    {
        animator.SetTrigger("isUsing");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isHacking = true;
        GameManager.Instance.HackMinigame();
    }

    /// <summary>
    /// Checks if the player is trying to hack the panel
    /// </summary>
    private void StartHacking()
    {
        if (IsPlayerInsideHack() && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(UseAnimation());
        }
    }

    /// <summary>
    /// Moves the player transform with the platform he is in
    /// </summary>
    private void MoveWithPlatform()
    {
        if (currentPlatform != null)
        {
            Vector3 platformMovement = currentPlatform.position - lastPlatformPosition;
            transform.position += platformMovement;
            lastPlatformPosition = currentPlatform.position;
        }
    }

    /// <summary>
    /// Movement of the player
    /// </summary>
    private void MovePlayer()
    {
        if (!isCrouching)
        {
            hAxis = Input.GetAxisRaw("Horizontal");
            if (hAxis != 0)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }

            rb.velocity = new Vector2(hAxis * moveSpeed, rb.velocity.y);
        } 
    }

    /// <summary>
    /// Jump if touching ground, jumps higher if holds key or jumps shorter if release the key
    /// </summary>
    private void Jump()
    {
        
        if (Input.GetButtonDown("Jump") && IsGrounded() && !isCrouching)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            AudioManager.Instance.Jump();
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
    /// Checks if the player tried and can dash
    /// </summary>
    private void StartDash()
    {
        if (canDash && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }
    }

    /// <summary>
    /// Allows the player to dash to right or left, with an specified distance and cooldown
    /// </summary>
    /// <returns></returns>
    private IEnumerator Dash()
    {
        canDash = false;
        IsDashing = true;
        animator.SetTrigger("Dash");
        //AudioManager.Instance.Dash();

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(transform.localScale.x * dashPower, 0);


        yield return new WaitForSeconds(dashTime);

        rb.gravityScale = originalGravity;
        IsDashing = false;
        

        yield return new WaitForSeconds(dashCooldown);
        playerSprite.color = Color.yellow;
        yield return new WaitForSeconds(0.15f);
        playerSprite.color = Color.white;
        canDash = true;
    }

    /// <summary>
    /// Checks if the player can move and stops velocity if needed
    /// </summary>
    /// <returns></returns>
    private bool CanMove()
    {
        if (IsDashing)
        {
            return false;
        }
        if (isHacking || GameManager.Instance.Health <= 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return false;
        }
 
        return true;
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            animator.SetTrigger("Attack");
        }
    }

    private void Crouch()
    {
        if (Input.GetKey(KeyCode.S) && IsGrounded())
        {
            isCrouching = true;
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetBool("isCrouching", isCrouching);
            return;
        }
        if (Input.GetKeyUp(KeyCode.S)) {
            isCrouching = false;
            animator.SetBool("isCrouching", isCrouching);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Orb"))
        {
            canDash = true;
        }
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
