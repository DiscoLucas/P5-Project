using UnityEngine;
using Windows.Kinect;

public class FaceTest2 : MonoBehaviour
{
    private KinectSensor kinectSensor;
    private BodyFrameReader bodyFrameReader;
    private Body[] bodies = null;

    public GameObject objectToRotate;

    void Start()
    {
        kinectSensor = KinectSensor.GetDefault();

        if (kinectSensor != null)
        {
            bodyFrameReader = kinectSensor.BodyFrameSource.OpenReader();
            if (!kinectSensor.IsOpen)
            {
                kinectSensor.Open();
            }
        }
    }

    void Update()
    {
        if (bodyFrameReader != null)
        {
            using (var frame = bodyFrameReader.AcquireLatestFrame())
            {
                if (frame != null)
                {
                    if (bodies == null)
                    {
                        bodies = new Body[kinectSensor.BodyFrameSource.BodyCount];
                    }

                    frame.GetAndRefreshBodyData(bodies);

                    foreach (var body in bodies)
                    {
                        if (body.IsTracked)
                        {
                            var rotation = body.FaceFrameSource.FaceFrameResults.RelativeRotation;
                            if (objectToRotate != null)
                            {
                                objectToRotate.transform.localRotation = Quaternion.Euler(rotation.Pitch, rotation.Yaw, rotation.Roll);
                            }
                        }
                    }
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        if (bodyFrameReader != null)
        {
            bodyFrameReader.Dispose();
        }

        if (kinectSensor != null)
        {
            if (kinectSensor.IsOpen)
            {
                kinectSensor.Close();
            }
        }
    }
}
