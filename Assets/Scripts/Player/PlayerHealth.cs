using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{

    private bool canTakeDamage = true;
    private PlayerMovement playerMovement;
    private SpriteRenderer playerSprite;
    private Animator animator;
    [SerializeField] private Color hurtColor = Color.red;
    [SerializeField] private float blinkDuration = 1f;
    [SerializeField] private int blinkCount = 4;
    private Collider2D playerSpriteCollider;
    

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        playerSpriteCollider = GetComponentInChildren<SpriteRenderer>().GetComponent<Collider2D>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player falls into death barrier, he dies
        if (collision.CompareTag("DeathBarrier"))
        {
            GameManager.Instance.GameOver();
        }


    }

    /// <summary>
    /// Makes the player blink and waits for being able to take damage again
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayerHurt()
    {
        canTakeDamage = false;
        Color originalColor = playerSprite.color;

        for (int i = 0; i < blinkCount; i++)
        {
            playerSprite.color = hurtColor;
            yield return new WaitForSeconds(blinkDuration / (blinkCount * 2));

            playerSprite.color = originalColor;
            yield return new WaitForSeconds(blinkDuration / (blinkCount * 2));
        }
        yield return new WaitForSeconds(1f);
        canTakeDamage = true;
    }

    /// <summary>
    /// Death to player and game over
    /// </summary>
    /// <returns></returns>
    private IEnumerator Death()
    {
        animator.SetTrigger("isDead");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        GameManager.Instance.GameOver();
    }

    /// <summary>
    /// Takes damage from enemy
    /// </summary>
    public void TakeDamage()
    {
        if (!canTakeDamage || playerMovement.IsDashing) return;

        StartCoroutine(PlayerHurt());
        GameManager.Instance.Health--;
        AudioManager.Instance.Hurt();

        if (GameManager.Instance.Health <= 0)
        {
            StartCoroutine(Death());
        }
    }
}
