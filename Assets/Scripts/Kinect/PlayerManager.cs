using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;

public class PlayerManager : MonoBehaviour
{

    public Rigidbody rb;
    public float deadzone = 1;

    [Header("speed variables")]
    public float movementSpeed;
    public float longitudinalSpeed;
    public float diveSpeedMulitplier = 2f;
    public float speedModifier;
    [Tooltip("How quickly the ship responds to pitch and roll input")]
    public float responsiveness = 15f;

    private float roll;
    private float pitch;

    public bool useKinect;

    private float responseModifier // adjust responsiveness based on mass
    {
        get
        {
            return (rb.mass / 10f) * responsiveness;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //transform.position = waypoints[waypointIndex].transform.position;
        
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

        // TODO: add resting force to make the ship return to upright when not moving
        //float relativeVelocity = rigidbody.velocity.z;
        //rigidbody.AddRelativeTorque(Vector3.right * relativeVelocity);

        // Adjust lift force based on pitch angle
        /*float pitchAngle = Mathf.Abs(transform.eulerAngles.z);
        float pitchFactor = Mathf.Clamp01(pitchAngle / 90f); // may need to be adjusted
        float  currentLift = lift * (1f - pitchFactor);
        rb.AddForce(Vector3.up * currentLift, ForceMode.Acceleration);*/

        rb.AddForce(transform.forward * longitudinalSpeed);

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
                rb.AddForce(xAxis, 0, 0); // TODO: change add force to affect zAxis instead
            }

        }
    }

    void DebugMovement()
    {
        rb.AddForce(0, 0, -Input.GetAxis("Horizontal") * responseModifier);
        
        //rb.AddTorque(transform.right * -Input.GetAxis("Horizontal") * responseModifier); //roll thing
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.AddForce(Vector3.right * rb.velocity.magnitude * diveSpeedMulitplier, ForceMode.Impulse);
    }
}
