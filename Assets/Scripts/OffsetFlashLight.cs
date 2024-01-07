using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFlashLight : MonoBehaviour
{
    public Transform target;  // The hand or wherever you want the flashlight to look at.
    public float rotationSpeed = 5f;  // The speed of rotation, adjust to your liking.
    public float delay = 0.2f;        // Delay in seconds before following the target rotation
    
    private Quaternion targetRotation;
    private Quaternion delayedRotation;


    private FlashLight flash;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("Flashlight's target not set!");
            enabled = false;
            return;
        }
        targetRotation = target.rotation;
        delayedRotation = targetRotation;
        flash = GetComponent<FlashLight>();
    }

    private void Update()
    {
        if(flash.isPickedUp){
// Update the delayed rotation with a smooth interpolation towards the target rotation
        delayedRotation = Quaternion.Lerp(delayedRotation, targetRotation, Time.deltaTime / delay);

        // Rotate the flashlight towards the delayed rotation, creating the delay effect
        transform.rotation = Quaternion.Lerp(transform.rotation, delayedRotation, Time.deltaTime * rotationSpeed);

        OnPlayerRotate();
        }
        else
        return;
        
    }

    // Call this method when the player or camera rotates.
    public void OnPlayerRotate()
    {
        targetRotation = target.rotation;
    }
}
