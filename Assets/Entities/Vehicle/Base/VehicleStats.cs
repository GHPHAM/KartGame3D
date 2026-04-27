/***************************************************************
*file: VehicleStats.cs
*author: Ryan Davies
*class: CS 4700 – Game Development
*assignment: final program
*date last modified: 4/18/26
*
*purpose: This keeps track of a vehicles current stats
 * (mostly done through inheritance and VehicleStatsModifier class)
*
****************************************************************/

using System.Collections.Generic;
using Entities.Vehicle.Modifiable;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class VehicleStats<T> : AttackerStats<T> where T : VehicleStatsModifier
{
    public override void die()
    {
        Destroy(gameObject);
    }
}