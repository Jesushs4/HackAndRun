using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    private bool canTakeDamage = true;
    private PlayerMovement playerMovement;
    private SpriteRenderer playerSprite;
    private Animator animator;
    [SerializeField] private Color hurtColor = Color.red;
    [SerializeField] private float blinkDuration = 1f;
    [SerializeField] private int blinkCount = 4;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player falls into death barrier, he dies
        if (collision.CompareTag("DeathBarrier"))
        {
            GameManager.Instance.GameOver();
        }


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // If player collisions with an enemy he loses 1 HP
        if (collision.CompareTag("Enemy") && canTakeDamage && !playerMovement.IsDashing)
        {
            StartCoroutine(PlayerHurt());
            GameManager.Instance.Health--;
            AudioManager.Instance.Hurt();

            if (GameManager.Instance.Health <= 0)
            {
                StartCoroutine(Death());
            }
        }
    }

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

    private IEnumerator Death()
    {
        animator.SetTrigger("isDead");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        GameManager.Instance.GameOver();
    }
}
