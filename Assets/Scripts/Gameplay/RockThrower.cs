using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;

public class RockThrower : MonoBehaviour
{
    public float speed = 15f;
    public float horizontalSpeed = 10f;
    public float VelocAdd = 10f;
    [SerializeField] private GameObject MC;

    [SerializeField] private LayerMask collidLayer;

    [SerializeField] private Transform cam;
    public float deadzone = 1;
    public float camSense;

    float stunTimer = -1; //magic number lmfaoooo
    float deflateTime = -1;

    Rigidbody rb;
    Animator an;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        an = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (stunTimer > 0)
        {
            rb.AddForce(new Vector3(0, speed / 2), ForceMode.Impulse);
            //rb.velocity = new Vector3(0, speed/2);

            stunTimer -= Time.deltaTime;
        }
        else
        {
            rb.velocity = new Vector3(Input.GetAxis("Horizontal") * horizontalSpeed, 0, speed);
        }

        if(deflateTime > 0)
        {
            rb.AddForce(new Vector3(0, -100, 0), ForceMode.Impulse);
            deflateTime -= Time.deltaTime;
        }

        cameraTrack();
    }
    private void OnCollisionEnter(Collision other)
    {
        print("hit");
        stunTimer = 1;
        //animation part does not work >:(
        //an.enabled = true;
        //an.Play("Crash");        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            speed += VelocAdd;
            Debug.Log("Balls");
            deflateTime = 0.3f;
            //an.Play("Dive");
        }
    }

    private void cameraTrack()
    {
        //Vector3 camX = cam.transform.position;
        Debug.Log(cam.localPosition.x);
        float xAxis = cam.localPosition.x * camSense;
        if (xAxis > deadzone || xAxis < -deadzone)
        {
            rb.AddForce(xAxis * horizontalSpeed, 0, 0);
        }

    }
}
