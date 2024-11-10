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
        }
    }

    private IEnumerator PickOrb()
    {
        spriteRenderer.enabled = false;
        circleCollider.enabled = false;
        yield return new WaitForSeconds(2f);
        spriteRenderer.enabled = true;
        circleCollider.enabled = true;
    }
}
