using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    float timePassed;
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
        }
        else
        {
            GameObject.FindWithTag("XR Rig").SetActive(true);
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
                VRInput();
                break;

            case InputType.Kinect:
                KinectInput();
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

    private float KinectInput()
    {
        GameObject playerCenter = GameObject.FindWithTag("PlayerCenter");

        if (playerCenter != null)
        {
            Transform transform = playerCenter.transform;
            float xAxis = transform.localPosition.x;
            
            return xAxis;
        }
        else
        {
            timePassed += Time.deltaTime;
            if (timePassed > 1f)
            {
                Debug.Log("No player center found. Using fallback input 0f");
            }
        }
        
    
        return 0f;
    }
}
