using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFlashLight : MonoBehaviour
{
    public Transform target;  // The hand or wherever you want the flashlight to look at.
    public float rotationSpeed = 5f;  // The speed of rotation, adjust to your liking.

    private Quaternion targetRotation;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("Flashlight's target not set!");
            enabled = false;
            return;
        }
        targetRotation = target.rotation;
    }

    private void Update()
    {

        // Interpolate rotation towards the updated target rotation.
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        OnPlayerRotate();
    }

    // Call this method when the player or camera rotates.
    public void OnPlayerRotate()
    {
        targetRotation = target.rotation;
    }
}
