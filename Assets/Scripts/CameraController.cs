using System;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //public Transform headTracker; // The object representing the user's head position. NOT currently used
    public Transform physicalDisplay; // The object representing the physical display.
    public float FOVModifier = 4;
    public float fallbackFOV = 60;

    private void Start()
    {
        if (physicalDisplay == null)
        {
            Camera.main.fieldOfView = fallbackFOV;
            this.enabled = false;
        }
        CalculateInitialFOV();
    }

    private void CalculateInitialFOV()
    {
        if (physicalDisplay != null)
        {
            // Get the size of the physical display
            Vector3 displaySize = physicalDisplay.localScale;

            // Calculate initial FOV
            float aspectRatio = displaySize.x / displaySize.y;
            Camera.main.fieldOfView = FOVModifier * Mathf.Atan(displaySize.y / (2 * transform.position.z)) * Mathf.Rad2Deg;
        }
    }

    void Update()
    {
        GameObject headTracker = GameObject.FindWithTag("PlayerHead");

        if (headTracker != null)
        {

            // Get the head position and display position.
            Vector3 headPosition = headTracker.transform.position;
            Vector3 displayPosition = physicalDisplay.position;

            // Calculate the camera's position relative to the display and head position.
            Vector3 cameraPosition = displayPosition + (headPosition - displayPosition);

            // Update the camera position
            transform.position = cameraPosition;

            float distanceToDisplay = Vector3.Distance(transform.position, displayPosition);

            // Calculate real FOV
            //float playerFOV = 4f * Mathf.Atan((physicalDisplay.localScale.x * 0.5f) / distanceToDisplay) * Mathf.Rad2Deg;

            // update camera to mathc real FOV
            CalculateInitialFOV();
            transform.LookAt(displayPosition);


        }
        
    }
}
