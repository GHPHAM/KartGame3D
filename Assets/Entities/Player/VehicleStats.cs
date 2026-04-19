/***************************************************************
*file: VehicleStats.cs
*author: Ryan Davies
*class: CS 4700 – Game Development
*assignment: final program
*date last modified: 4/18/26
*
*purpose: This keeps track of the player's current stats
*
****************************************************************/
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class VehicleStats
{
    // These fields mirror the ones in VehicleController, but are used to store
    [HideInInspector] public float maxSpeed;
    [HideInInspector] public float maxReverseSpeed;
    [HideInInspector] public float accelerationForce;
    [HideInInspector] public float brakeForce;
    [HideInInspector] public float steerSpeed;
    [HideInInspector] public float handling;
    [HideInInspector] public float maxLeanAngle;
    [HideInInspector] public float leanSmoothing;

    // function: getCurrentStats
    // purpose: returns the current stats of the player's vehicle
    public VehicleStats getCurrentStats()
    {
        return this;
    }

    // function: clone
    // purpose: creates a copy of the current VehicleStats object
    public VehicleStats clone()
    {
        return new VehicleStats
        {
            maxSpeed = this.maxSpeed,
            maxReverseSpeed = this.maxReverseSpeed,
            accelerationForce = this.accelerationForce,
            brakeForce = this.brakeForce,
            steerSpeed = this.steerSpeed,
            handling = this.handling,
            maxLeanAngle = this.maxLeanAngle,
            leanSmoothing = this.leanSmoothing
        };
    }
}