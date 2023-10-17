using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class KinectCamera : MonoBehaviour
{
    private KinectSensor kinectSensor;
    private ColorFrameReader colorFrameReader;
    private Texture2D colorTexture;
    private FrameDescription colorFrameDescription;

    public Material displayMaterial;

    void Start()
    {
        kinectSensor = KinectSensor.GetDefault();
        if (kinectSensor != null)
        {
            colorFrameReader = kinectSensor.ColorFrameSource.OpenReader();
            colorFrameDescription = kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba); // Add this line
            colorTexture = new Texture2D(colorFrameDescription.Width, colorFrameDescription.Height, TextureFormat.RGBA32, false);
            displayMaterial.mainTexture = colorTexture;
            kinectSensor.Open();
        }
    }

    void Update()
    {
        if (colorFrameReader != null)
        {
            using (ColorFrame colorFrame = colorFrameReader.AcquireLatestFrame())
            {
                if (colorFrame != null)
                {
                    byte[] colorData = new byte[colorFrameDescription.LengthInPixels * 4];
                    colorFrame.CopyConvertedFrameDataToArray(colorData, ColorImageFormat.Rgba);
                    colorTexture.LoadRawTextureData(colorData);
                    colorTexture.Apply();
                }
            }
        }
    }

    void OnDestroy()
    {
        if (colorFrameReader != null)
        {
            colorFrameReader.Dispose();
            colorFrameReader = null;
        }
        if (kinectSensor != null)
        {
            kinectSensor.Close();
            kinectSensor = null;
        }
    }
}
