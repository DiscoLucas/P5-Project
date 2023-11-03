using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Rendering;
using UnityEngine;

public class RockThrower : MonoBehaviour
{
    float speed = 4f;
    float horizontalSpeed = 5f;

    float stunTimer = -1; //magic number lmfaoooo
    float stunDirection = 0;

    Rigidbody rb;
    Animator an;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        an = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stunTimer > 0)
        {
            rb.velocity = new Vector3(stunDirection, 0, speed/2);
            print(stunDirection);

            stunTimer -= Time.deltaTime;
        }
        else
        {
            an.enabled = false;
            
            print(stunDirection);
            rb.velocity = new Vector3(Input.GetAxis("Horizontal") * horizontalSpeed, 0, speed);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        print("hit");
        stunTimer = 0.15f;
        stunDirection = 15;

        //animation part does not work >:(
        an.enabled = true;
        an.Play("Crash");
    }
}
