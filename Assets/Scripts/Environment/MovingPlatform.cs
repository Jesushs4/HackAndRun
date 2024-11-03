using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    // Variables
    [SerializeField] float platformWaitTime = 1f;
    [SerializeField] float platformDistance = 1f;
    // If true, platform moves vertically, if false, platform moves horizontally
    [SerializeField] bool platformUp = true;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 startPos;
    private bool movingForward = true;
    private bool isWaiting = false;

    private void Start()
    {
        // Saves the start position in order to come back after
        startPos = transform.position;
    }

    private void Update()
    {
        if (!isWaiting)
        {
            MovePlatform();
        }
    }

    /// <summary>
    /// Moves the platform in the direction given and controls if its going forward or backwards
    /// </summary>
    private void MovePlatform()
    {
        Vector3 direction = platformUp ? Vector3.up : Vector3.right;
        float currentDistance = Vector3.Distance(transform.position, startPos);

        if (movingForward)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
            if (currentDistance >= platformDistance)
            {
                StartCoroutine(WaitAndChangeDirection());
            }
        }
        else
        {
            transform.Translate(-direction * moveSpeed * Time.deltaTime);
            if (currentDistance >= platformDistance)
            {
                StartCoroutine(WaitAndChangeDirection());
            }
        }
    }

    /// <summary>
    /// Makes the platform wait a few seconds before moving into the opposite direction
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitAndChangeDirection()
    {
        isWaiting = true;
        yield return new WaitForSeconds(platformWaitTime);
        movingForward = !movingForward;
        startPos = transform.position;
        isWaiting = false;
    }
}
