using System;
using Entities;
using UnityEngine;

public class EnterRangeDoor : RoomDoorway
{
    bool hasBeenOpened = false;
    
    public override void assignRoom(RoomController owner)
    {
        //only assign variable
        _controller = owner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(hasBeenOpened)
            return;
        
        if (other.TryGetComponent(out EntityStatsBase entity))
        {
            if (entity is VehicleController)
            {
                hasBeenOpened = true;
                spawnRoomAsync();
            }
        }
    }
}
