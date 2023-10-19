using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Kinect;
using Microsoft.Kinect.Face;
using Windows.Kinect;
using System;
using System.ComponentModel;
using System.Windows;
using System.Globalization;

public class StolenFromMS : MonoBehaviour
{
    private const double FaceRotationIncrementInDegrees = 5.0;
    private KinectSensor kinectSensor = null;
    /// <summary>
    /// Coordinate mapper to map one type of point to another
    /// </summary>
    private CoordinateMapper coordinateMapper = null;

    /// <summary>
    /// Reader for body frames
    /// </summary>
    private BodyFrameReader bodyFrameReader = null;

    /// <summary>
    /// Array to store bodies
    /// </summary>
    private Body[] bodies = null;

    /// <summary>
    /// Number of bodies tracked
    /// </summary>
    private int bodyCount;

    /// <summary>
    /// Face frame sources
    /// </summary>
    private FaceFrameSource[] faceFrameSources = null;

    /// <summary>
    /// Face frame readers
    /// </summary>
    private FaceFrameReader[] faceFrameReaders = null;

    /// <summary>
    /// Storage for face frame results
    /// </summary>
    private FaceFrameResult[] faceFrameResults = null;

    private void Start()
    {
        kinectSensor = KinectSensor.GetDefault();
        coordinateMapper = kinectSensor.CoordinateMapper;
        FrameDescription frameDescription = kinectSensor.ColorFrameSource.FrameDescription;
        bodyFrameReader = kinectSensor.BodyFrameSource.OpenReader();
        bodyFrameReader.FrameArrived += Reader_BodyFrameArrived;
        bodyCount = kinectSensor.BodyFrameSource.BodyCount;
        bodies = new Body[bodyCount];

        // specify the required face frame results
        FaceFrameFeatures faceFrameFeatures =
            FaceFrameFeatures.BoundingBoxInColorSpace
            | FaceFrameFeatures.PointsInColorSpace
            | FaceFrameFeatures.RotationOrientation
            | FaceFrameFeatures.FaceEngagement
            | FaceFrameFeatures.Glasses
            | FaceFrameFeatures.Happy
            | FaceFrameFeatures.LeftEyeClosed
            | FaceFrameFeatures.RightEyeClosed
            | FaceFrameFeatures.LookingAway
            | FaceFrameFeatures.MouthMoved
            | FaceFrameFeatures.MouthOpen;
        
        // create a face frame source + reader to track each face in the FOV
        faceFrameSources = new FaceFrameSource[bodyCount];
        faceFrameReaders = new FaceFrameReader[bodyCount];
        for (int i = 0; i < bodyCount; i++)
        {
            // create the face frame source with the required face frame features and an initial tracking Id of 0
            faceFrameSources[i] = new FaceFrameSource(kinectSensor, 0, faceFrameFeatures);

            // open the corresponding reader
            faceFrameReaders[i] = faceFrameSources[i].OpenReader();
        }

        // allocate storage to store face frame results for each face in the FOV
        faceFrameResults = new FaceFrameResult[bodyCount];

        kinectSensor.IsAvailableChanged += Sensor_IsAvailableChanged;

        kinectSensor.Open();
        Debug.Log("Is kinect sensor available: " + kinectSensor.IsAvailable);
        
    }

    /// <summary>
    /// INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Converts rotation quaternion to Euler angles 
    /// And then maps them to a specified range of values to control the refresh rate
    /// </summary>
    /// <param name="rotQuaternion">face rotation quaternion</param>
    /// <param name="pitch">rotation about the X-axis</param>
    /// <param name="yaw">rotation about the Y-axis</param>
    /// <param name="roll">rotation about the Z-axis</param>
    private static void ExtractFaceRotationInDegrees(Windows.Kinect.Vector4 rotQuaternion, out int pitch, out int yaw, out int roll)
    {
        double x = rotQuaternion.X;
        double y = rotQuaternion.Y;
        double z = rotQuaternion.Z;
        double w = rotQuaternion.W;

        // convert face rotation quaternion to Euler angles in degrees
        double yawD, pitchD, rollD;
        pitchD = Math.Atan2(2 * ((y * z) + (w * x)), (w * w) - (x * x) - (y * y) + (z * z)) / Math.PI * 180.0;
        yawD = Math.Asin(2 * ((w * y) - (x * z))) / Math.PI * 180.0;
        rollD = Math.Atan2(2 * ((x * y) + (w * z)), (w * w) + (x * x) - (y * y) - (z * z)) / Math.PI * 180.0;

        // clamp the values to a multiple of the specified increment to control the refresh rate
        double increment = FaceRotationIncrementInDegrees;
        pitch = (int)(Math.Floor((pitchD + ((increment / 2.0) * (pitchD > 0 ? 1.0 : -1.0))) / increment) * increment);
        yaw = (int)(Math.Floor((yawD + ((increment / 2.0) * (yawD > 0 ? 1.0 : -1.0))) / increment) * increment);
        roll = (int)(Math.Floor((rollD + ((increment / 2.0) * (rollD > 0 ? 1.0 : -1.0))) / increment) * increment);
    }

    /// <summary>
    /// Execute start up tasks
    /// </summary>
    /// <param name="sender">object sending the event</param>
    /// <param name="e">event arguments</param>
    private void Loaded()
    {
        for (int i = 0; i < this.bodyCount; i++)
        {
            if (this.faceFrameReaders[i] != null)
            {
                // wire handler for face frame arrival
                this.faceFrameReaders[i].FrameArrived += this.Reader_FaceFrameArrived;
            }
        }

        if (this.bodyFrameReader != null)
        {
            // wire handler for body frame arrival
            this.bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;
        }
    }

    private void OnApplicationQuit()
    {
        for (int i = 0;i < bodyCount; i++)
        {
            if (faceFrameReaders[i] != null)
            {
                faceFrameReaders[i].Dispose();
                faceFrameReaders[i] = null;
            }

            if (faceFrameSources[i] != null)
            {
                //faceFrameSources[i].Dispose();
                faceFrameSources[i] = null;
            }
        }

        if (bodyFrameReader != null)
        { 
            bodyFrameReader.Dispose();
            bodyFrameReader = null;
        }
        if (kinectSensor != null)
        {
            kinectSensor.Close();
            kinectSensor = null;
        }
    }

    /// <summary>
    /// Handles the face frame data arriving from the sensor
    /// </summary>
    /// <param name="sender">object sending the event</param>
    /// <param name="e">event arguments</param>
    private void Reader_FaceFrameArrived(object sender, FaceFrameArrivedEventArgs e)
    {
        using (FaceFrame faceFrame = e.FrameReference.AcquireFrame())
        {
            if (faceFrame != null)
            {
                // get the index of the face source from the face source array
                int index = this.GetFaceSourceIndex(faceFrame.FaceFrameSource);

                // check if this face frame has valid face frame results
                /*if (this.ValidateFaceBoxAndPoints(faceFrame.FaceFrameResult))
                {
                    // store this face frame result to draw later
                    this.faceFrameResults[index] = faceFrame.FaceFrameResult;
                }
                else
                {
                    // indicates that the latest face frame result from this reader is invalid
                    this.faceFrameResults[index] = null;
                }*/
            }
        }
    }
    /// <summary>
    /// Returns the index of the face frame source
    /// </summary>
    /// <param name="faceFrameSource">the face frame source</param>
    /// <returns>the index of the face source in the face source array</returns>
    private int GetFaceSourceIndex(FaceFrameSource faceFrameSource)
    {
        int index = -1;

        for (int i = 0; i < this.bodyCount; i++)
        {
            if (this.faceFrameSources[i] == faceFrameSource)
            {
                index = i;
                break;
            }
        }

        return index;
    }

    /// <summary>
    /// Handles the body frame data arriving from the sensor
    /// </summary>
    /// <param name="sender">object sending the event</param>
    /// <param name="e">event arguments</param>
    private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
    {
        using (var bodyFrame = e.FrameReference.AcquireFrame())
        {
            if (bodyFrame != null)
            {
                // update body data
                bodyFrame.GetAndRefreshBodyData(this.bodies);

                
                {
                    // draw the dark background
                    

                    bool drawFaceResult = false;

                    // iterate through each face source
                    for (int i = 0; i < this.bodyCount; i++)
                    {
                        // check if a valid face is tracked in this face source
                        if (this.faceFrameSources[i].IsTrackingIdValid)
                        {
                            // check if we have valid face frame results
                            if (this.faceFrameResults[i] != null)
                            {
                                // draw face frame results
                                Debug.Log(faceFrameResults);

                                if (!drawFaceResult)
                                {
                                    drawFaceResult = true;
                                }
                            }
                        }
                        else
                        {
                            // check if the corresponding body is tracked 
                            if (this.bodies[i].IsTracked)
                            {
                                // update the face frame source to track this body
                                this.faceFrameSources[i].TrackingId = this.bodies[i].TrackingId;
                            }
                        }
                    }

                    if (!drawFaceResult)
                    {
                        // if no faces were drawn then this indicates one of the following:
                        // a body was not tracked 
                        // a body was tracked but the corresponding face was not tracked
                        // a body and the corresponding face was tracked though the face box or the face points were not valid
                        Debug.Log("No bodies or faces are tracked");
                    }

                }
            }
        }
    }

    /// <summary>
    /// Draws face frame results
    /// </summary>
    /// <param name="faceIndex">the index of the face frame corresponding to a specific body in the FOV</param>
    /// <param name="faceResult">container of all face frame results</param>
    /// <param name="drawingContext">drawing context to render to</param>
    private void DrawFaceFrameResults(int faceIndex, FaceFrameResult faceResult)
    {


        string faceText = string.Empty;

        // extract each face property information and store it in faceText
        if (faceResult.FaceProperties != null)
        {
            foreach (var item in faceResult.FaceProperties)
            {
                faceText += item.Key.ToString() + " : ";

                // consider a "maybe" as a "no" to restrict 
                // the detection result refresh rate
                if (item.Value == DetectionResult.Maybe)
                {
                    faceText += DetectionResult.No + "\n";
                }
                else
                {
                    faceText += item.Value.ToString() + "\n";
                }
            }
        }

        // extract face rotation in degrees as Euler angles
        if (faceResult.FaceRotationQuaternion != null)
        {
            int pitch, yaw, roll;
            ExtractFaceRotationInDegrees(faceResult.FaceRotationQuaternion, out pitch, out yaw, out roll);
            faceText += "FaceYaw : " + yaw + "\n" +
                        "FacePitch : " + pitch + "\n" +
                        "FacenRoll : " + roll + "\n";
        }

        // render the face property and face rotation information
        Debug.Log(faceText);
        
    }


    /// <summary>
    /// Handles the event which the sensor becomes unavailable (E.g. paused, closed, unplugged).
    /// </summary>
    /// <param name="sender">object sending the event</param>
    /// <param name="e">event arguments</param>
    private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
    {
        if (this.kinectSensor != null)
        {
            // on failure, set the status text
            Debug.Log("Kinect sensor is unavailable");
        }
    }
}
