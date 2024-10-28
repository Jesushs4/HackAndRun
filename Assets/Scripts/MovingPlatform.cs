using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float platformWaitTime = 1f;
    [SerializeField] float platformDistance = 1f;
    [SerializeField] bool platformUp = true;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 startPos;
    private bool movingForward = true;
    private bool isWaiting = false;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (!isWaiting)
        {
            MovePlatform();
        }
    }

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

    private IEnumerator WaitAndChangeDirection()
    {
        isWaiting = true;
        yield return new WaitForSeconds(platformWaitTime);
        movingForward = !movingForward;
        startPos = transform.position;
        isWaiting = false;
    }
}
