using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Kinect.Face;
using Windows.Kinect;

public class FaceTest : MonoBehaviour
{

    private KinectSensor sensor;
    private FaceFrameSource faceFrameSource;
    private FaceFrameReader faceFrameReader;
    private HighDefinitionFaceFrame HDFrame;


    // Start is called before the first frame update
    void Start()
    {
        // Initialize Kinect sensor
        sensor = KinectSensor.GetDefault();

        if (sensor != null)
        {
            // Initialize face tracking
            faceFrameSource = FaceFrameSource.Create(sensor, 0, FaceFrameFeatures.RotationOrientation);
            faceFrameReader = faceFrameSource.OpenReader();
            sensor.Open();
        }
        else
        {
            Debug.Log("Face tracking was not initialized!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (faceFrameReader != null)
        {
            //Debug.Log("1. faceFrameReader is not null");
            using (var frame = faceFrameReader.AcquireLatestFrame())
            {
                Debug.Log(faceFrameSource.TrackingId);
                if (frame != null && faceFrameSource.TrackingId != 0)
                {
                    // Get face rotation data
                    FaceFrameResult result = frame.FaceFrameResult;
                    Debug.Log(result);

                    if (result != null)
                    {
                        Debug.Log("4. result is not null");
                        // Access rotation data
                        Debug.Log("here");
                        Debug.Log("X is: " + result.FaceRotationQuaternion.X + 
                            ", Y is: " + result.FaceRotationQuaternion.Y +
                            ", Z is: " + result.FaceRotationQuaternion.Z);
                        Debug.Log(result.FaceRotationQuaternion.ToString());

                    }
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (sensor != null) 
        { 
            sensor.Close(); 
        }
    }
}
