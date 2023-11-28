using System;
using UnityEngine;

// Inspired by user Vazgriz https://www.youtube.com/watch?v=7vAHo2B1zLc
public class GliderController : MonoBehaviour
{

    [SerializeField] float maxThrust;
    [SerializeField] float throttleSpeed;
    [SerializeField] float gLimit;
    [SerializeField] float gLimitPitch;


    private Vector3 lastVelocity;

    [Header("Lift")]
    [Tooltip("Lift power is used to scale the lift force, tweaking this value will change the flight characteristics of the aircraft")]
    [SerializeField] float liftPower;
    [SerializeField] AnimationCurve liftAOACurve;
    [SerializeField] float inducedDrag;
    [SerializeField] AnimationCurve inducedDragCurve;
    [SerializeField] float rudderPower;
    [SerializeField] AnimationCurve rudderAOACurve;
    [SerializeField] AnimationCurve rudderInducedDragCurve;

    [Header("Steering")]
    [SerializeField] Vector3 turnSpeed;
    [SerializeField] Vector3 turnAcceleration;
    [SerializeField] AnimationCurve steeringCurve;

    [Header("Drag")]
    [SerializeField]
    AnimationCurve dragForward;
    [SerializeField]
    AnimationCurve dragBack;
    [SerializeField]
    AnimationCurve dragLeft;
    [SerializeField]
    AnimationCurve dragRight;
    [SerializeField]
    AnimationCurve dragTop;
    [SerializeField]
    AnimationCurve dragBottom;
    [SerializeField] Vector3 angularDrag;


    float throttleInput;
    Vector3 controlInput;
    
    public Rigidbody RB { get; private set; }
    public float Throttle { get; private set; }
    public Vector3 EffectiveInput { get; private set; }
    public Vector3 Velocity { get; private set; }
    public Vector3 LocalVelocity { get; private set; }
    public Vector3 LocalGForce { get; private set; }
    public Vector3 LocalAngularVelocity { get; private set; }
    public float AngleOfAttack { get; private set; }
    public float AngleOfAttackYaw { get; private set; }

    public void SetThrottleInput(float input) { throttleInput = input; }
    public void SetControlInput(Vector3 input) { controlInput = Vector3.ClampMagnitude(input, 1); }

    void Start()
    {
        RB = GetComponent<Rigidbody>();
    }

    void UpdateThrottle(float dt)
    {
        float target = 0;
        if (throttleInput > 0) target = 1;
        Throttle = Utilities.MoveTo(Throttle, target, throttleSpeed * Mathf.Abs(throttleInput), dt);
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        CalculateState(dt);
        CalculateAngleOfAttack();
        //CalculateGForce(dt);

        //controlInput = new Vector3(Input.GetAxis("Horizontal"), controlInput.y, Input.GetAxis("Vertical"));
        //throttleInput = Input.GetAxis("Throttle axis");
        SetControlInput(controlInput);
        SetThrottleInput(throttleInput);

        UpdateThrottle(dt);
        UpdateThrust();
        UpdateLift();
        UpdateSteering(dt);

        UpdateDrag();
        UpdateAngularDrag();
        CalculateState(dt);
    }

    private void CalculateState(float dt)
    {
        var invRotation = Quaternion.Inverse(RB.rotation);
        Velocity = RB.velocity;
        LocalVelocity = invRotation * Velocity;
        LocalAngularVelocity = invRotation * RB.angularVelocity;

        CalculateAngleOfAttack();
    }

    void CalculateAngleOfAttack()
    {
        if (LocalVelocity.sqrMagnitude < 0.1f)
        {
            AngleOfAttack = 0;
            AngleOfAttackYaw = 0;
            return;
        }
        AngleOfAttack = Mathf.Atan2(-LocalVelocity.y, LocalVelocity.z); // AoA measured on the pitch axis
        AngleOfAttackYaw = Mathf.Atan2(LocalVelocity.x, LocalVelocity.z); // AoA measured on the yaw axis
    }

    // G force is derived from the curent and previous velocity
    void CalculateGForce(float dt)
    {
        var invRotation = Quaternion.Inverse(RB.rotation);
        var acceleration = (Velocity - lastVelocity) / dt;
        LocalGForce = invRotation * acceleration;
        lastVelocity = Velocity;
    }

    Vector3 CalculateLift(float angleOfAttack, Vector3 rightAxis, float liftPower, AnimationCurve aoaCurve, AnimationCurve inducedDragCurve)
    {
        // Calculate lift
        var liftVelocity = Vector3.ProjectOnPlane(LocalVelocity, rightAxis);
        var v2 = liftVelocity.sqrMagnitude;

        // calculate lift coefficient depending on the angle of attack and velocity
        var liftCoefficient = aoaCurve.Evaluate(angleOfAttack * Mathf.Rad2Deg);
        var liftForce = v2 * liftCoefficient * liftPower;

        // lift is perpendicular to the velocity and the right axis
        var liftDirection = Vector3.Cross(liftVelocity.normalized, rightAxis);
        var lift = liftDirection * liftForce;

        // Calculate induced drag
        // induced drag varies with square of the lift coefficient
        var dragForce = liftCoefficient * liftCoefficient * this.inducedDrag;
        var dragDirection = -liftVelocity.normalized;
        var inducedDrag = dragDirection * v2 * dragForce;

        return lift + inducedDrag; // return the sum of the force vectors
    }

    float CalculateSteering(float dt, float angularVelocity, float targetVelocity, float acceleration)
    {
        var error = targetVelocity - angularVelocity;
        var accel = acceleration * dt;
        return Mathf.Clamp(error, -accel, accel);
    }

    void UpdateThrust()
    {
        RB.AddRelativeForce(Throttle * maxThrust * Vector3.forward);
    }

    

    void UpdateLift()
    {
        if (LocalVelocity.sqrMagnitude < 1f) return;

        var liftForce = CalculateLift(AngleOfAttack, Vector3.right, liftPower, liftAOACurve, inducedDragCurve);

        var yawForce = CalculateLift(AngleOfAttackYaw, Vector3.up, rudderPower, rudderAOACurve, rudderInducedDragCurve);

        RB.AddRelativeForce(liftForce);
        RB.AddRelativeForce(yawForce);
    }

    void UpdateSteering(float dt)
    {
        var speed = Mathf.Max(0, LocalVelocity.z);
        var steeringPower = steeringCurve.Evaluate(speed); // adjusts the steering power based on the speed of the aircraft to make low speed steering more responsive

        var targetAV = Vector3.Scale(controlInput, turnSpeed * steeringPower);
        var av = LocalAngularVelocity * Mathf.Rad2Deg;

        var correction = new Vector3(
            CalculateSteering(dt, av.x, targetAV.x, turnAcceleration.x * steeringPower),
            CalculateSteering(dt, av.y, targetAV.y, turnAcceleration.y * steeringPower),
            CalculateSteering(dt, av.z, targetAV.z, turnAcceleration.z * steeringPower));
        
        RB.AddRelativeTorque(correction * Mathf.Deg2Rad, ForceMode.VelocityChange);

    }

    void UpdateDrag()
    {
        var lv = LocalVelocity;
        var lv2 = lv.sqrMagnitude;

        // calculate drag coefficient depending on the angle of attack and velocity
        var coefficient = Utilities.Scale6(
            lv.normalized,
            dragRight.Evaluate(Mathf.Abs(lv.x)), dragLeft.Evaluate(Mathf.Abs(lv.x)),
            dragTop.Evaluate(Mathf.Abs(lv.y)), dragBottom.Evaluate(Mathf.Abs(lv.y)),
            dragForward.Evaluate(Mathf.Abs(lv.z)), dragBack.Evaluate(Mathf.Abs(lv.z))
            );
        var drag = coefficient.magnitude * lv2 * -lv.normalized; // drag is the opposite direction of velocity

        RB.AddRelativeForce(drag);
    }

    void UpdateAngularDrag()
    {
        var av = LocalAngularVelocity;

        var drag = av.sqrMagnitude * -av.normalized; // drag is the opposite direction of angular velocity

        RB.AddRelativeTorque(Vector3.Scale(drag, angularDrag), ForceMode.Acceleration);
    }

    Vector3 CalculateGForce(Vector3 angularVelocity, Vector3 velocity)
    {
        return Vector3.Cross(angularVelocity, velocity);
    }
    
}

