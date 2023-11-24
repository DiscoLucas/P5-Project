using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;

public class PlayerManager : MonoBehaviour
{

    public Rigidbody rigidbody;
    public float deadzone = 1;
    public float movementSpeed;
    public float speedModifier;
    public bool useKinect;

    Controls controls;
    

    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls();
        controls.Gameplay.Enable();

        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (useKinect == true)
        {
            Debug.Log("Using kinect movement");
            KinectMovement();
        }
        else
        {
            DebugMovement();
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
                rigidbody.AddForce(xAxis, 0, 0); // TODO: change add force to affect zAxis instead
            }

        }
    }

    void DebugMovement()
    {
        rigidbody.AddForce(0, 0, -Input.GetAxis("Horizontal") * speedModifier);
        rigidbody.MovePosition(transform.position + Vector3.forward * Time.deltaTime * movementSpeed);
    }

}
