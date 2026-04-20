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

using Entities.Vehicle.Modifiable;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class VehicleBase : EntityStats
{
    public virtual VehicleStatModifier vehicleStats { get; }

    public override void die()
    {
        Destroy(gameObject);
    }
}