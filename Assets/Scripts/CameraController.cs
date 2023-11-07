using UnityEngine;

public class CameraController : MonoBehaviour
{
    //public Transform headTracker; // The object representing the user's head position. NOT currently used
    public Transform physicalDisplay; // The object representing the physical display.

    void Update()
    {
        GameObject headTracker = GameObject.FindWithTag("PlayerHead");

        if (headTracker != null)
        {
            //Transform transform = headTracker.transform;

            // Get the head position and display position.
            Vector3 headPosition = headTracker.transform.position;
            Vector3 displayPosition = physicalDisplay.position;

            // Calculate the camera's position relative to the display and head position.
            Vector3 cameraPosition = displayPosition + (headPosition - displayPosition);

            // Update the camera position
            transform.position = cameraPosition;

            float distanceToDisplay = Vector3.Distance(transform.position, displayPosition);

            // Calculate real FOV
            float playerFOV = 2f * Mathf.Atan((physicalDisplay.localScale.x * 0.5f) / distanceToDisplay) * Mathf.Rad2Deg;

            // update camera to mathc real FOV
            Camera.main.fieldOfView = playerFOV;
            transform.LookAt(displayPosition);


        }
        
    }
}
