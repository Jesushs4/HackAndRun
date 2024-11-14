using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DashOrb : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(PickOrb());
            StartCoroutine(PlayerBlink(collision.gameObject.GetComponentInChildren<SpriteRenderer>()));
        }
    }

    /// <summary>
    /// Disable orb and respawns after time
    /// </summary>
    /// <returns></returns>
    private IEnumerator PickOrb()
    {
        AudioManager.Instance.OrbPickUp();
        spriteRenderer.enabled = false;
        circleCollider.enabled = false;
        yield return new WaitForSeconds(2f);
        spriteRenderer.enabled = true;
        circleCollider.enabled = true;
    }

    /// <summary>
    /// Makes the player blink after resetting dash
    /// </summary>
    /// <param name="spriteRenderer"></param>
    /// <returns></returns>
    private IEnumerator PlayerBlink(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.color = Color.yellow;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;

    }
}
