using JetBrains.Annotations;
using System.ComponentModel;
using Entities.Vehicle.Modifiable;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Vehicle Controller - X-axis is forward (positive = front).
/// Uses the new Unity Input System.
///
/// W / Up Arrow    : Accelerate
/// S / Down Arrow  : Brake / Reverse
/// A / Left Arrow  : Steer Left
/// D / Right Arrow : Steer Right
///
/// LEAN SETUP:
///   Create a child GameObject under your vehicle root (e.g. "Body").
///   Put your mesh/visuals on that child.
///   Assign it to the "Body Transform" field in the Inspector.
///   The root Rigidbody stays flat; only the body visually tilts.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class VehicleController : ModifiableVehicleBase
{
    [Header("Lean / Body Tilt")]
    [Tooltip("Child GameObject that holds the visible mesh. Leave empty to lean the root (not recommended).")]
    public Transform bodyTransform;

    [Tooltip("Maximum lean angle in degrees when steering at full speed")]
    public float maxLeanAngle = 5f;

    [Tooltip("How quickly the body leans into / recovers from a turn (higher = snappier)")]
    public float leanSmoothing = 8f;
    
    // -- singleton ----------------------------------------------------------
    public static VehicleController singleton;

    // -- internals ----------------------------------------------------------
    private Rigidbody _rb;
    private float     _currentSpeed;
    private float     _currentSteerSpeed;
    private float     _currentLeanAngle;   // tracks smoothed lean value
    private Keyboard  _kb;

    private Camera _cam;
    private float  _baseFov;

    void Awake()
    {
        if(singleton == null) { singleton = this; }

        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationY |
                          RigidbodyConstraints.FreezeRotationZ;

        _kb = Keyboard.current;

        _cam     = GetComponentInChildren<Camera>();
        _baseFov = _cam.fieldOfView;

        // Fall back to the root transform if no body is assigned
        if (bodyTransform == null)
            bodyTransform = transform;
    }

    void FixedUpdate()
    {
        if (_kb == null) { _kb = Keyboard.current; return; }

        float throttleInput = GetThrottleInput();
        float steerInput    = GetSteerInput();

        //get a copy of the current stats through the get of vehicleStats
        VehicleStatModifier currentStats = vehicleStats;
        
        HandleThrottle(throttleInput, currentStats);
        HandleSteering(steerInput, currentStats);
        ApplyVelocity();
        ApplyLean(steerInput, currentStats);
        UpdateFov(currentStats);
    }

    // -- Input helpers ------------------------------------------------------

    float GetThrottleInput()
    {
        bool forward = _kb.wKey.isPressed || _kb.upArrowKey.isPressed;
        bool back    = _kb.sKey.isPressed || _kb.downArrowKey.isPressed;

        if (forward && !back)    return  1f;
        if (back    && !forward) return -1f;
        return 0f;
    }

    float GetSteerInput()
    {
        bool right = _kb.dKey.isPressed || _kb.rightArrowKey.isPressed;
        bool left  = _kb.aKey.isPressed || _kb.leftArrowKey.isPressed;

        if (right && !left)  return  1f;
        if (left  && !right) return -1f;
        return 0f;
    }

    // -- Throttle & natural deceleration -----------------------------------
    void HandleThrottle(float input, VehicleStatModifier currentStats)
    {
        if (input > 0f)
        {
            // Accelerate forward
            _currentSpeed += currentStats.accelerationForce * Time.fixedDeltaTime;
            _currentSpeed  = Mathf.Min(_currentSpeed, currentStats.maxSpeed);
        }
        else if (input < 0f)
        {
            // Subtract from speed - brakes first, then reverses naturally
            _currentSpeed -= currentStats.brakeForce * Time.fixedDeltaTime;
            _currentSpeed  = Mathf.Max(_currentSpeed, -currentStats.maxReverseSpeed);
        }
        else
        {
            // No input - coast to a stop
            if (_currentSpeed > 0f)
            {
                _currentSpeed -= currentStats.naturalDeceleration * Time.fixedDeltaTime;
                _currentSpeed  = Mathf.Max(_currentSpeed, 0f);
            }
            else if (_currentSpeed < 0f)
            {
                _currentSpeed += currentStats.naturalDeceleration * Time.fixedDeltaTime;
                _currentSpeed  = Mathf.Min(_currentSpeed, 0f);
            }
        }
    }

    // -- Steering ----------------------------------------------------------
    void HandleSteering(float input, VehicleStatModifier currentStats)
    {
        if (Mathf.Abs(_currentSpeed) < currentStats.minSpeedToSteer) return;

        float direction  = Mathf.Sign(_currentSpeed);
        float speedRatio = Mathf.Abs(_currentSpeed) / currentStats.maxSpeed;
        _currentSteerSpeed = Mathf.Lerp(_currentSteerSpeed, currentStats.maxSteer * input * direction, currentStats.handling);
        float turnAmount = _currentSteerSpeed * speedRatio * Time.fixedDeltaTime;

        transform.Rotate(Vector3.up, turnAmount, Space.World);
    }

    // -- Lean / body tilt --------------------------------------------------
    void ApplyLean(float steerInput, VehicleStatModifier currentStats)
    {
        // Scale lean by how fast we're going - no lean when nearly stopped
        float speedRatio  = Mathf.Abs(_currentSpeed) / currentStats.maxSpeed;

        // Lean LEFT when steering right (negative local Z = right tilt in Unity)
        // Flip sign so the top of the body rolls toward the inside of the turn
        float targetLean  = -steerInput * maxLeanAngle * speedRatio;

        // Smoothly interpolate toward the target lean angle
        _currentLeanAngle = Mathf.Lerp(
            _currentLeanAngle,
            targetLean,
            leanSmoothing * Time.fixedDeltaTime
        );

        // Apply only the Z (roll) component; keep X and Y from the body's own local rotation
        Vector3 currentEuler = bodyTransform.localEulerAngles;
        bodyTransform.localEulerAngles = new Vector3(
            _currentLeanAngle,
            currentEuler.y,
            currentEuler.z
        );
    }

    // -- Push Rigidbody along the local X axis -----------------------------
    void ApplyVelocity()
    {
        Vector3 horizontalVelocity = transform.right * _currentSpeed;
        _rb.linearVelocity = new Vector3(
            horizontalVelocity.x,
            _rb.linearVelocity.y,
            horizontalVelocity.z
        );
    }

    // -- Gizmo: red arrow = vehicle forward (local X) ----------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.right * 2f);
    }

    // -- FOV changing based on speed -----------------------------
    void UpdateFov(VehicleStatModifier currentStats)
    {
        float normalizedSpeed = Mathf.Clamp01(_currentSpeed / currentStats.maxSpeed);
        _cam.fieldOfView = _baseFov + normalizedSpeed * 20f;
    }


    // -- End game when player dies -----------------------------
    public override void die()
    {
        // todo 
        // actually have a game over
        Debug.Log("Player Is Deadzo");
    }

    public override void damage(int damage)
    {
        base.damage(damage);

        // todo 
        // damage visual effect
    }
}