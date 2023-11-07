using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class PlayerManager : MonoBehaviour
{
    public GameObject BodySourceManager;
    private BodySourceManager bodySourceManager;

    //GameObject playerCenter;

    // Start is called before the first frame update
    void Start()
    {
        
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
    /*
    public PlayerTransform(Transform transform)
    {
        GameObject playerCenter = GameObject.FindWithTag("PlayerCenter");
        
        return;
    }*/

}
