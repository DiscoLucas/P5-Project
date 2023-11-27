using UnityEngine;

public class GliderController : MonoBehaviour
{
    // Adjust these values based on your aircraft and preferences
    public float liftCoefficient = 0.1f;
    public float dragCoefficient = 0.01f;
    public float pitchSpeed = 5f;
    public float rollSpeed = 3f;
    [Tooltip("How quickly the aircraft responds to pitch and roll input")]
    public float responsiveness = 15;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Input for pitch and roll
        float pitchInput = Input.GetAxis("Vertical");
        float rollInput = Input.GetAxis("Horizontal");

        // Apply pitch and roll forces
        Vector3 pitchTorque = pitchInput * pitchSpeed * transform.right;
        Vector3 rollTorque = rollInput * rollSpeed * transform.forward;
        rb.AddTorque(pitchTorque + rollTorque);

        // Calculate lift and drag forces based on pitch and angle of attack
        float angleOfAttack = Vector3.Angle(transform.up, rb.velocity.normalized);
        float liftForce = liftCoefficient * angleOfAttack * rb.velocity.sqrMagnitude;
        float dragForce = dragCoefficient * rb.velocity.sqrMagnitude;

        // Apply lift and drag forces
        Vector3 liftVector = transform.up * liftForce;
        Vector3 dragVector = -rb.velocity.normalized * dragForce;
        rb.AddForce(liftVector + dragVector);
    }
}

