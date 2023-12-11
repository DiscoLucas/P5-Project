using UnityEngine;

public class FOVcontroller : MonoBehaviour
{
    public Transform topLeftPoint;
    public Transform topRightPoint;
    public Transform bottomLeftPoint;
    public Transform bottomRightPoint;
    public Camera mainCamera;

    void Update()
    {
        GameObject headTracker = GameObject.FindWithTag("PlayerHead");
        if (headTracker != null)
        {
            // Update the camera position
            transform.position = headTracker.transform.position;
        }
        
        // Calculate distances from camera to each corner point
        float distanceTopLeft = Vector3.Distance(mainCamera.transform.position, topLeftPoint.position);
        float distanceTopRight = Vector3.Distance(mainCamera.transform.position, topRightPoint.position);
        float distanceBottomLeft = Vector3.Distance(mainCamera.transform.position, bottomLeftPoint.position);
        float distanceBottomRight = Vector3.Distance(mainCamera.transform.position, bottomRightPoint.position);

        // Calculate average distance
        float averageDistance = (distanceTopLeft + distanceTopRight + distanceBottomLeft + distanceBottomRight) / 4f;

        // Calculate FOV using trigonometry
        float horizontalFOV = 2 * Mathf.Atan((topRightPoint.position.x - topLeftPoint.position.x) / (2 * averageDistance)) * Mathf.Rad2Deg;
        float verticalFOV = 2 * Mathf.Atan((topRightPoint.position.y - bottomRightPoint.position.y) / (2 * averageDistance)) * Mathf.Rad2Deg;

        // Set the camera's FOV
        mainCamera.fieldOfView = Mathf.Max(horizontalFOV, verticalFOV);

        // Ensure FOV does not go out of bounds of the display representation
        // You may need to add additional logic based on your game's specifics
    }
}
