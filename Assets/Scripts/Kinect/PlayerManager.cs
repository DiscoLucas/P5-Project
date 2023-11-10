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
    private float strafeInput;
    private Coroutine strafeRoutine;
    private bool strafeKeyDown = false;
    

    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls();
        //controls.Gameplay.Strafe.started += OnStrafeStarted;
        //controls.Gameplay.Strafe.canceled += OnStrafeCanceled;
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
            DebugMovement();
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
        rigidbody.AddForce(Input.GetAxis("Horizontal") * speedModifier,0, 0);
        rigidbody.MovePosition(transform.position + Vector3.forward * Time.deltaTime * movementSpeed);
    }
    
    /*void OnStrafeStarted(InputAction.CallbackContext context)
    {
        strafeKeyDown = true;
    }

    void OnStrafeCanceled(InputAction.CallbackContext context)
    {
        strafeKeyDown = false;
    }

    private void FixedUpdate()
    {
        if (strafeKeyDown)
        {
            rigidbody.AddForce(Vector3.right * strafeInput * movementSpeed);
        }
    }*/

    /*
    public void OnStrafe(InputAction.CallbackContext context)
    {
        
         strafeInput = context.ReadValue<float>();
         Debug.Log(strafeInput);
        //rigidbody.AddForce(strafeInput, 0, 0);
        
        if (strafeInput != 0f)
        {
            if (strafeRoutine == null)
            {
                strafeRoutine = StartCoroutine(ApplyStrafeForce());
            }
        }
        else
        {
            if (strafeRoutine != null)
            {
                StopCoroutine(strafeRoutine);
                strafeRoutine = null;
            }
        }
    }

    private IEnumerator ApplyStrafeForce()
    {
        while (true)
        {
            Debug.Log("Applying force");
            rigidbody.AddForce(strafeInput, 0, 0);

            yield return new WaitForSeconds(0.02f);
        }
    }
    /*
    private void FixedUpdate()
    {
        rigidbody.AddForce(strafeInput, 0, 0);
    }
    */

}
