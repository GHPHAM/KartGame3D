/***************************************************************
*file: VehicleController.cs
*author: 
*class: CS 4700 – Game Development
*assignment: final program
*date last modified: 4/24/26
*
*purpose: This script manages the player's vehicle movement, including acceleration, 
*braking, steering, and leaning. It also handles input from the keyboard and applies 
*any active stat modifiers from powerups. It should be attached to the player's vehicle 
*GameObject, which must also have a Rigidbody component.
*
****************************************************************/
using JetBrains.Annotations;
using System.ComponentModel;
using Entities.Vehicle.Modifiable;
using Unity.Collections;
using Unity.Mathematics;
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

    [Tooltip("How much velocity is affected by your steering")]
    public float grip = .1f;

    [Tooltip("How much your grip is affected by drifting")]
    public float driftGripMultiplier = .1f;

    [Tooltip("How fast you change from drift grip to regular grip")]
    public float driftTransitionSpeed = .2f;

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
    private float  _grip;

    void Awake()
    {
        if(singleton == null) { singleton = this; }

        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX |
                          RigidbodyConstraints.FreezeRotationY |
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

        HandleGrip();
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

    private void HandleGrip()
    {
        _grip = Mathf.Lerp(
            _grip,
            _kb.shiftKey.isPressed ?
                grip * driftGripMultiplier :
                grip,
            driftTransitionSpeed
            );
    }

    // -- Throttle & natural deceleration -----------------------------------
    void HandleThrottle(float input, VehicleStatModifier currentStats)
    {
        if (input > 0f)
        {
            // Accelerate forward
            _currentSpeed += _grip * currentStats.accelerationForce * Time.fixedDeltaTime;
            _currentSpeed  = Mathf.Min(_currentSpeed, currentStats.maxSpeed);
        }
        else if (input < 0f)
        {
            // Subtract from speed - brakes first, then reverses naturally
            _currentSpeed -= _grip * currentStats.brakeForce * Time.fixedDeltaTime;
            _currentSpeed  = Mathf.Max(_currentSpeed, -currentStats.maxReverseSpeed);
        }
        else
        {
            // No input - coast to a stop
            if (_currentSpeed > 0f)
            {
                _currentSpeed -= _grip * currentStats.naturalDeceleration * Time.fixedDeltaTime;
                _currentSpeed  = Mathf.Max(_currentSpeed, 0f);
            }
            else if (_currentSpeed < 0f)
            {
                _currentSpeed += _grip * currentStats.naturalDeceleration * Time.fixedDeltaTime;
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
            currentEuler.x,
            currentEuler.y,
            _currentLeanAngle
        );
    }

    // -- Push Rigidbody along the local X axis -----------------------------
    void ApplyVelocity()
    {
        Vector3 horizontalVelocity = transform.forward * _currentSpeed;
        _rb.linearVelocity = new Vector3(
            horizontalVelocity.x,
            _rb.linearVelocity.y,
            horizontalVelocity.z
        ) * _grip + _rb.linearVelocity * (1 - _grip);
    }

    // -- Gizmo: red arrow = vehicle forward (local X) ----------------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }

    // -- FOV changing based on speed -----------------------------
    void UpdateFov(VehicleStatModifier currentStats)
    {
        float normalizedSpeed = Mathf.Clamp01(_currentSpeed / currentStats.maxSpeed);
        _cam.fieldOfView = _baseFov + normalizedSpeed * 20f;
    }

    // -- Manage Item Pickups ----------------------------------
    //function: CollectPowerUp
    //purpose: to apply the effects of a collected powerup to the player
    public void CollectPowerUp(PowerUpData data)
    {
        Debug.Log($"Collected PowerUp: {data.powerUpName}");

        switch (data.type)
        {
            case PowerUpType.SpeedBoost:
                Debug.Log($"Applying {data.powerUpName}");

                //build speed boost modifier
                VehicleStatModifier speedMod = new VehicleStatModifier.Builder().
                    maxSpeed(data.magnitude).
                    accelerationForce(data.magnitude/2).
                    Build();

                speedMod.identifier = data.powerUpName; //set identifier

                addModifier( speedMod );

                if ( data.lifetime == PowerUpLifetime.Temporary )
                {
                    StartCoroutine( removeModifierAfterTime(speedMod, data.duration, data.powerUpName));
                }
                break;

            case PowerUpType.DamageBoost:
                Debug.Log("Damage boost not implemented yet");
                break;

            case PowerUpType.Reflect:
                Debug.Log("Reflect not implemented yet");
                break;

            case PowerUpType.HealthRestore:
                Debug.Log("Health restore not implemented yet");
                break;
        }
    }

    //function: removeModifierAfterTime
    //purpose: to remove a stat modifier after a certain duration, used for temporary powerups
    private System.Collections.IEnumerator removeModifierAfterTime(VehicleStatModifier mod, float time, string powerUpName)
    {
        yield return new WaitForSeconds(time);
        removeModifier(mod);
        Debug.Log($"{powerUpName} expired");
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

        Debug.Log($"[DEBUG] Player took {damage} damage | HP: {health}/{maxHealth}");
        // todo
        // damage visual effect
        // hook up to UI
    }
}