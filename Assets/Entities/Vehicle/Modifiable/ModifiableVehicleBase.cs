/***************************************************************
*file: VehicleModifierController.cs
*author: Ryan Davies
*class: CS 4700 – Game Development
*assignment: final program
*date last modified: 4/18/26
*
*purpose: This manages the player's current vehicle stats, including any
*modifiers from pickups or powerups. It should be attached to the same
*GameObject as the VehicleController and VehicleStats components.
*
****************************************************************/

using System.Collections.Generic;
using Entities.Vehicle.Modifiable;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class ModifiableVehicleBase : VehicleBase
{
    public override VehicleStatModifier vehicleStats
    {
        get
        {
            VehicleStatModifier modifiedStats = new  VehicleStatModifier(baseStats);
            
            foreach(VehicleStatModifier mod in modifiers)
            {
                modifiedStats.add(mod);
            }
            
            return modifiedStats;
        }
    }
    
    public VehicleStatModifier baseStats;

    // ---- Get/Set ----------------------------------

    [SerializeField] List<VehicleStatModifier> modifiers = new List<VehicleStatModifier>();
    
    public void addModifier(VehicleStatModifier modifier)
    {
        modifiers.Add(modifier);
    }
    
    public void removeModifier(VehicleStatModifier modifier)
    {
        modifiers.Remove(modifier);
    }
    
    // function: updateStats
    // purpose: updates the vehicle stats based on the currently active modifiers
    public void updateStats()
    {
        
    }
}