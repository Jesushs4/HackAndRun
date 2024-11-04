using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Demon
}

public class EnemyController : MonoBehaviour
{
    // Enemy Type
    public EnemyType enemyType = EnemyType.Demon;

    // Components
    private Transform groundDetection;
    private SpriteRenderer enemySprite;

    // Ground check
    private LayerMask layer;
    private float rayDistance = 1.5f;

    // Movement
    [SerializeField] private Vector2 direction = Vector2.right;
    [SerializeField] private float moveSpeed = 1;

    private void Awake()
    {
        groundDetection = transform.GetChild(0);
        enemySprite = GetComponentInChildren<SpriteRenderer>();
        layer = LayerMask.GetMask("Ground");
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
        EnemyMove();
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
                if (!GeneralDetection(rayDistance, Vector2.down) || GeneralDetection(rayDistance / 3, direction))
                {
                    direction = -direction;
                    enemySprite.flipX = !enemySprite.flipX;
                    groundDetection.localPosition = new Vector3(-groundDetection.localPosition.x, groundDetection.localPosition.y, groundDetection.localPosition.z);
                }


                break;
        }
        transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

    }

}
