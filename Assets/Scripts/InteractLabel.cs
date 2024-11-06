using DG.Tweening;
using UnityEngine;

public class InteractLabel : MonoBehaviour
{
    private LayerMask playerLayer;
    private SpriteRenderer spriteRenderer;

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

    private bool IsPlayerInRange()
    {
        return Physics2D.OverlapCircle(transform.position, 0.85f, playerLayer);
    }

    private void FadeIn()
    {
        if (spriteRenderer.color.a < 1)
        {
            spriteRenderer.DOKill();
            spriteRenderer.DOFade(1f, 0.5f);
        }
    }

    private void FadeOut()
    {
        if (spriteRenderer.color.a > 0)
        {
            spriteRenderer.DOKill();
            spriteRenderer.DOFade(0f, 0.5f);
        }
    }
}
