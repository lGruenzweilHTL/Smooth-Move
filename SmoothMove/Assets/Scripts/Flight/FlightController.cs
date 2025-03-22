using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FlightController : MonoBehaviour {
    /// <summary>
    /// The mode of input used for a specific input data.
    /// </summary>
    public enum InputMode {
        /// <summary>
        /// This input is disabled and will not be checked for.
        /// </summary>
        Disabled,

        /// <summary>
        /// This input will be checked using an axis.
        /// </summary>
        Axis,

        /// <summary>
        /// This input will be checked for using buttons in the Input System.
        /// </summary>
        Buttons,

        /// <summary>
        /// This input will be checked for using KeyCodes.
        /// </summary>
        Keys
    }

    [Serializable]
    public struct InputData {
        public InputMode mode;

        [Tooltip("In the context of rotations it is the speed of the rotation; In the context of throttle it is the time (in seconds) it takes to reach max speed")]
        public float speed;

        [Tooltip("Use the raw input or a smoothed value for the axis? Implemented using GetAxisRaw and GetAxis")]
        public bool useRawAxis;

        public string axisName;
        public bool invert;

        public string positiveButtonName, negativeButtonName;
        public KeyCode positiveKey, negativeKey;

        /// <summary>
        /// Gets the current value of the input based on the mode.
        /// Sensitivity is not included in this calculation, as this struct is not responsible for updating the values, only for handling inputs.
        /// </summary>
        /// <returns>The input value from -1 to 1</returns>
        public float GetValue() {
            return mode switch {
                InputMode.Disabled => 0f,
                InputMode.Axis => (invert ? -1f : 1f) * (useRawAxis ? Input.GetAxisRaw(axisName) : Input.GetAxis(axisName)),
                InputMode.Buttons => Input.GetButton(positiveButtonName) ? 1f : Input.GetButton(negativeButtonName) ? -1f : 0f,
                InputMode.Keys => Input.GetKey(positiveKey) ? 1f : Input.GetKey(negativeKey) ? -1f : 0f,
                _ => throw new ArgumentOutOfRangeException(nameof(mode), "Unknown input mode: " + mode)
            };
        }
    }

    private struct Inputs {
        public float Throttle; // -1 to 1
        public float Roll, Pitch, Yaw; // -1 to 1
    }

    [SerializeField] private float maxSpeed;
    [SerializeField] private AnimationCurve accelerationCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [Space(20f), SerializeField] private InputData throttle, pitch, roll, yaw;

    private float currentAcceleration;
    private Rigidbody rb;

    private Inputs currentInputs;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void Reset() {
        // Reset will most often be called in edit mode, so we can't assume that rb has been assigned
        rb = GetComponent<Rigidbody>();

        // Reset rigidbody values
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.drag = 0f;
        rb.angularDrag = 0f;
        rb.mass = 1f;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.ResetCenterOfMass();
        rb.ResetInertiaTensor();
    }

    private void Update() {
        currentInputs.Throttle = throttle.GetValue();
        currentInputs.Roll = roll.GetValue();
        currentInputs.Pitch = pitch.GetValue();
        currentInputs.Yaw = yaw.GetValue();
    }

    private void FixedUpdate() {
        // Rotate transform
        transform.Rotate(
            currentInputs.Pitch,
            currentInputs.Yaw,
            currentInputs.Roll,
            Space.Self);

        // Apply throttle
        currentAcceleration = Mathf.Clamp(currentAcceleration + currentInputs.Throttle * Time.fixedDeltaTime / throttle.speed, 0f, 1f);

        float speed = maxSpeed * accelerationCurve.Evaluate(currentAcceleration);
        rb.velocity = transform.forward * speed;
    }
}