using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using UnityEngine.InputSystem;
using System;

public class PlayerManager : MonoBehaviour
{

    public Rigidbody rigidbody;
    public GameObject ship;
    public float deadzone = 1;
    public float movementSpeed;
    public bool useKinect;

    Controls controls;
    private float strafeInput;
    

    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls();
        //controls.Gameplay.Strafe.performed += OnStrafe;
        controls.Gameplay.Enable();

        if (rigidbody == null) { gameObject.AddComponent<Rigidbody>(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (useKinect == true)
        {
            Debug.Log("Using kinect movement");
            KinectMovement();
        }
        else
        {
            //DebugMovement();
            //OnStrafe();
        }
    }

    void KinectMovement()
    {
        GameObject playerCenter = GameObject.FindWithTag("PlayerCenter");

        if (playerCenter != null)
        {
            Transform transform = playerCenter.transform;
            float xAxis = transform.position.x;
            if (xAxis > deadzone || xAxis < -deadzone)
            {
                rigidbody.AddForce(xAxis, 0, 0);
            }

        }
    }

    void DebugMovement()
    {
        
    }

    /*
    public void OnStrafe(InputAction.CallbackContext context)
    {
        strafeInput = context.ReadValue<float>();
        Debug.Log(strafeInput);
        //rigidbody.AddForce(strafeInput * movementSpeed, 0, 0);
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(strafeInput, 0, 0);
    }
    */

}
