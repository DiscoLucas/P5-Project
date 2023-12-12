using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] float kinectVModifier;
    [SerializeField] float keyboardVModifier = 1400f;
    [SerializeField] float vrVModifier;
    float timePassed;
    //[SerializeField] private GameObject MainCameraNotVR;
    public enum InputType
    {
        Keyboard,
        VR,
        Kinect
    }
    public InputType selectedInput; //list for selecting the input type

    private void Awake()
    {
        // disable XR Rig if VR is not selected
        if (selectedInput != InputType.VR)
        {
            GameObject.FindWithTag("XR Rig").SetActive(false);
            GameObject.FindWithTag("MainCam").SetActive(true);

        }
        else
        {
            GameObject.FindWithTag("XR Rig").SetActive(true);
            GameObject.FindWithTag("MainCam").SetActive(false);
        }
    }

    /// <summary>
    /// Used for getting the horizontal input from the currently selected input type.
    /// </summary>
    /// <returns>A float based on either keyboard input or the position of the player</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public float GetHorizontalInput()
    {
        float horizontalAxis = 0f;

        

        switch (selectedInput)
        {
            case InputType.Keyboard:
                return Input.GetAxis("Horizontal");

            case InputType.VR:
                return VRInput() * vrVModifier;

            case InputType.Kinect:
                GameObject playerCenter = GameObject.FindWithTag("PlayerCenter");

                if (playerCenter != null)
                {
                    return KinectInput(playerCenter) * kinectVModifier;
                }
                else
                {
                    Debug.LogWarning("PlayerCenter not found");
                }
                break;
                
            default:
                throw new ArgumentOutOfRangeException();
        }

        return horizontalAxis;
    }

    

    private float VRInput()
    {
        float camPos = Camera.main.transform.localPosition.x;
        return camPos;
    }

    private float KinectInput(GameObject playerCenter)
    {
        Transform transform = playerCenter.transform;
        float xAxis = transform.localPosition.x * kinectVModifier;
        return xAxis;
    }
}
