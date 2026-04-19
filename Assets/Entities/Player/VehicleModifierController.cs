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
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[System.Serializable]
public class VehicleModifierController : MonoBehaviour
{
    private VehicleController vehicleController;
    private VehicleStats currentStats;

    private void Awake()
    {
        //get the vehicle controller and stats components from the parent objects
        vehicleController = GetComponent<VehicleController>();

        currentStats = vehicleController.getBaseStatsCopy();
    }


    // ---- Get/Set ----------------------------------

    // function: getCurrentStats
    // purpose: returns the current vehicle stats, including any active modifiers
    public VehicleStats getCurrentStats()
    {
        return currentStats;
    }

    // function: updateStats
    // purpose: updates the vehicle stats based on the currently active modifiers
    public void updateStats()
    {
        VehicleStats newStats = currentStats.clone();
        //todo: loop through the active modifiers and apply their effects to the vehicle stats


    }
}