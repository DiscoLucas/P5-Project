using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class PlayerManager : MonoBehaviour
{

    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        if (rigidbody == null) { gameObject.AddComponent<Rigidbody>(); }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject playerCenter = GameObject.FindWithTag("PlayerCenter");

        if (playerCenter != null )
        {
            Transform transform = playerCenter.transform;
            
            
        }
    }


}
