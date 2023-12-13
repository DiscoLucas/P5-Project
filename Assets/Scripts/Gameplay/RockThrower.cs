using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using UnityEditor.Animations;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.SceneManagement;

public class RockThrower : MonoBehaviour
{
    public float health = 3;
    public float speed = 15f;
    public float horizontalSpeed = 10f;
    public float VelocAdd = 10f;
    //[SerializeField] private GameObject MC;

    [SerializeField] private LayerMask collidLayer;

    [SerializeField] private Transform cam;
    public float deadzone = 1;
    public float camSense;

    float stunTimer = -1; //magic number lmfaoooo
    public float deflateTime = -1;
    public float deflateStrenght = -3.5f;
    private Vector3 startPos;
    private Vector3 lastPos;
    //private float xDiff;
    private float zDiff;

    Rigidbody rb;
    public Animator an;

    InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        lastPos = startPos;
        rb = GetComponent<Rigidbody>();
        an = GetComponent<Animator>();
        //rb.velocity = new Vector3(speed/4, 0, 0); //start with a constant speed

        inputManager = gameObject.GetComponent<InputManager>();

        FindObjectOfType<AudioManager>().Play("Wind");
        FindObjectOfType<AudioManager>().Play("Propella");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        zDiff = lastPos.z - transform.position.z;
        lastPos = transform.position;
        an.SetFloat("Steer", zDiff);
        
        if (health <= 0)
        {
            health = 3;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //transform.position = startPos;
        }
        if (stunTimer > 0)
        {
            rb.AddForce(new Vector3(0, speed / 2), ForceMode.Impulse);
            //rb.velocity = new Vector3(0, speed/2);
            stunTimer -= Time.deltaTime;
        }
        else
        {
            
            float horizontalMovement = -inputManager.GetHorizontalInput();
            rb.AddForce(speed, 0, horizontalMovement);
        }
        if (deflateTime > 0)
        {
            transform.position += new Vector3(0, deflateStrenght*Time.deltaTime, 0);
            //rb.AddForce(new Vector3(0, deflateStrenght, 0), ForceMode.Impulse);
            deflateTime -= Time.deltaTime;
        }

        cameraTrack();

        if (zDiff < deadzone)
        {
            //FindObjectOfType<AudioManager>().Play("Wood Creak");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        print("hit");
        //stunTimer = 1;
        //animation part does not work >:(
        //an.enabled = true;
        //an.Play("Crash");
        /*
        if (collision.gameObject.layer == 7)
        {
            var direction = Vector3.Reflect(rb.velocity, collision.contacts[0].normal);
            rb.AddForce(direction, ForceMode.Impulse);
        }*/
    }
 

    private void cameraTrack()
    {
        //Vector3 camX = cam.transform.position;
        //Debug.Log(-cam.localPosition.x);
        float xAxis = -cam.localPosition.x * camSense;
        if (xAxis > deadzone || xAxis < -deadzone)
        {
            rb.AddForce(0, 0, xAxis * horizontalSpeed);
        }

    }
}
