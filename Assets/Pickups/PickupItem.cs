/***************************************************************
*file: PickupItem.cs
*author: Ryan Davies
*class: CS 4700 – Game Development
*assignment: final program
*date last modified: 4/24/26
*
*purpose: This script is attached to pickup items in the game world. 
*When the player collides with a pickup, it applies the corresponding 
*powerup effect to the player's vehicle and then destroys itself.
*
****************************************************************/
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private PowerUpData data; //put the actual data in the inspector

    // function: OnTriggerEnter
    // purpose: when a player vehicle collides with this pickup, apply the powerup effect to it and destroy the pickup
    private void OnTriggerEnter(Collider other)
    {
        VehicleController controller = other.GetComponent<VehicleController>();
        if (controller == null || !controller.CompareTag("Player")) return; // make sure it's the player

        controller.CollectPowerUp(data);

        Destroy(gameObject);
    }
}