using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    private bool canTakeDamage = true;

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
        if (collision.CompareTag("Enemy") && canTakeDamage)
        {
            StartCoroutine(PlayerHurt());
            GameManager.Instance.Health--;
        }
    }

    private IEnumerator PlayerHurt()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(1f);
        canTakeDamage = true;
    }
}
