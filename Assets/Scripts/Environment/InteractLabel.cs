using DG.Tweening;
using UnityEngine;

public class InteractLabel : MonoBehaviour
{
    private LayerMask playerLayer;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float interactRange = 0.85f;

    private void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (IsPlayerInRange())
        {
            FadeIn();
        } else
        {
            FadeOut();
        }
    }

    /// <summary>
    /// Checks if player is in range to activate the interact
    /// </summary>
    /// <returns></returns>
    private bool IsPlayerInRange()
    {
        return Physics2D.OverlapCircle(transform.position, interactRange, playerLayer);
    }

    /// <summary>
    /// Fades the label in
    /// </summary>
    private void FadeIn()
    {
        if (spriteRenderer.color.a < 1)
        {
            spriteRenderer.DOKill();
            spriteRenderer.DOFade(1f, 0.5f);
        }
    }

    /// <summary>
    /// Fades the label out
    /// </summary>
    private void FadeOut()
    {
        if (spriteRenderer.color.a > 0)
        {
            spriteRenderer.DOKill();
            spriteRenderer.DOFade(0f, 0.5f);
        }
    }
}
