using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    
    [SerializeField]
    GliderController plane;
    [SerializeField]

    Vector3 controlInput;

    void Start() {
        
    }



    public void SetThrottleInput(InputAction.CallbackContext context) {
        if (plane == null) return;
        

        plane.SetThrottleInput(context.ReadValue<float>());
    }

    public void OnRollPitchInput(InputAction.CallbackContext context) {
        if (plane == null) return;

        var input = context.ReadValue<Vector2>();
        controlInput = new Vector3(input.y, controlInput.y, -input.x);
        Debug.Log(controlInput);
    }

    public void OnYawInput(InputAction.CallbackContext context) {
        if (plane == null) return;

        var input = context.ReadValue<float>();
        controlInput = new Vector3(controlInput.x, input, controlInput.z);
    }





    void Update() {
        if (plane == null) return;

        plane.SetControlInput(controlInput);
    }
}
