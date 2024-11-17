using System.Collections;
using UnityEngine;

public enum EnemyType
{
    Demon,
    Cyborg
}

public class EnemyController : MonoBehaviour
{
    // Enemy Type
    public EnemyType enemyType = EnemyType.Demon;

    // Components
    private Transform groundDetection;
    private SpriteRenderer enemySprite;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider;

    // Ground check
    private LayerMask layer;
    private LayerMask playerLayer;
    private float rayDistance = 1.5f;

    // Movement
    [SerializeField] private Vector2 direction = Vector2.right;
    [SerializeField] private float moveSpeed = 1;

    [SerializeField] private int health = 3;
    [SerializeField] private Color hurtColor = Color.red;
    [SerializeField] private float blinkDuration = 1f;
    [SerializeField] private int blinkCount = 4;
    [SerializeField] private bool canPatrol = true;
    private bool isHurt = false;
    private bool isShooting = false;
    private bool canWalk = true;
    private bool isDead = false;
    private ShootController shootController;

    private void Awake()
    {
        groundDetection = transform.GetChild(0);
        enemySprite = GetComponentInChildren<SpriteRenderer>();
        layer = LayerMask.GetMask("Ground");
        animator = GetComponentInChildren<Animator>();
        shootController = GetComponentInChildren<ShootController>();
        playerLayer = LayerMask.GetMask("Player");
        animator.SetBool("canMove", canPatrol);
        capsuleCollider = GetComponentInChildren<CapsuleCollider2D>();
    }

    /// <summary>
    /// Detects enemies collisions floor, ceil and floor
    /// </summary>
    /// <param name="length">Length of the ray</param>
    /// <param name="dir">Direction of the ray</param>
    /// <returns></returns>
    private bool GeneralDetection(float length, Vector2 dir)
    {
        return Physics2D.Raycast(groundDetection.position, dir, length, layer);
    }



    private void Update()
    {
        if (!isHurt && !isDead)
        {
            EnemyMove();
        }

        if (health <= 0 && !isDead)
        {
            Death();
        }
    }

    /// <summary>
    /// Manages the enemy death
    /// </summary>
    private void Death()
    {
        animator.SetTrigger("Dead");

        isDead = true;
        canPatrol = false;
        canWalk = false;
        capsuleCollider.enabled = false;
        AudioManager.Instance.EnemyDeath();
        //StartCoroutine(WaitForDeathAnimation());
    }

    /// <summary>
    /// Waits for the death animation to destroy the enemy entity
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForDeathAnimation()
    {
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }


    /// <summary>
    /// Manage the enemy movement
    /// </summary>
    private void EnemyMove()
        {
        // If not detect ground or detect a wall change direction

        switch (enemyType)
        {
            case EnemyType.Demon:
                if (!GeneralDetection(rayDistance/2, Vector2.down) || GeneralDetection(rayDistance / 5, direction))
                {
                    FlipDirection();
                }
                transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;


                break;
            case EnemyType.Cyborg:
                if (PlayerInSight(direction))
                {
                    if (!isShooting && !isHurt)
                    ShootAnimation();
                }

                if (canWalk && canPatrol)
                {
                    transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
                }

                if (!GeneralDetection(rayDistance, Vector2.down) || GeneralDetection(rayDistance / 3, direction))
                {
                    FlipDirection();
                }
                break;
        }

        

    }

    /// <summary>
    /// Checks if player is in enemy sight with 3 raycasts from different height
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private bool PlayerInSight(Vector2 dir)
    {
        float[] heights = new float[] { 0f, 1f, -1f };

        foreach (float height in heights)
        {
            Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y + height);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, dir, 5f, LayerMask.GetMask("Player", "Ground"));

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        animator.SetBool("isShooting", false);
        isShooting = false;
        canWalk = true;

        return false;
    }

    /// <summary>
    /// Starts shoot animation
    /// </summary>
    /// <returns></returns>
    private void ShootAnimation()
    {
        isShooting = true;
        animator.SetBool("isShooting", true);
        canWalk = false;
        
    }



    /// <summary>
    /// Flips direction of the enemy
    /// </summary>
    private void FlipDirection()
    {
        direction *= -1;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

    }

    /// <summary>
    /// Enemy hurt with blink
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnemyHurt()
    {
        isHurt = true;
        Color originalColor = enemySprite.color;
        animator.SetBool("isShooting", false);
        animator.SetBool("canMove", false);
        for (int i = 0; i < blinkCount; i++)
        {
            enemySprite.color = hurtColor;
            yield return new WaitForSeconds(blinkDuration / (blinkCount * 2));

            enemySprite.color = originalColor;
            yield return new WaitForSeconds(blinkDuration / (blinkCount * 2));
        }
        isHurt = false;
        animator.SetBool("isShooting", true);
        animator.SetBool("canMove", canPatrol);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            health--;
            AudioManager.Instance.EnemyHit();
            if (!isHurt)
            {
                StartCoroutine(EnemyHurt());
            }
        }

        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponentInParent<IDamageable>().TakeDamage();
        }
    }

}
