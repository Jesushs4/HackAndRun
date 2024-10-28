using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] float parallaxSpeed;

    private Transform cameraPosition;

    private Vector3 lastCameraPosition;

    private void Start()
    {
        cameraPosition = Camera.main.transform;
        lastCameraPosition = cameraPosition.position;
    }

    private void LateUpdate()
    {
        // Calculate difference of movement
        Vector3 backgroundMove = cameraPosition.position - lastCameraPosition;
        // Move with speed
        transform.position += new Vector3(backgroundMove.x * parallaxSpeed, backgroundMove.y * parallaxSpeed, 0);
        // Update the lastCameraPosition
        lastCameraPosition = cameraPosition.position;
    }

}
