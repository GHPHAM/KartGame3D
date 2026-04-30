using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<RoomDoorway> doorways;
 
    // internals --------------------------------------------

    protected List<EntityStatsBase> _inhabitants;
    public event System.Action<EntityStatsBase> onInhabitantCleared = delegate { };
    public event System.Action onRoomCleared = delegate { };

    
    // Behaviour --------------------------------------------


    public void onInhabitantDeath(EntityStatsBase inhabitant)
    {
        _inhabitants.Remove(inhabitant);

        onInhabitantCleared(inhabitant);
    }

    public bool isRoomCleared()
    {
        return _inhabitants.Count == 0;
    }
    
    
    public List<RoomDoorway> getEntrances()
    {
        var exits = new List<RoomDoorway>();

        foreach (var doorway in  doorways)
        {
            if(doorway.isEntrance)
                exits.Add(doorway);
        }
        
        return exits;
    }
    
    public List<RoomDoorway> getExits()
    {
        var exits = new List<RoomDoorway>();

        foreach (var doorway in  doorways)
        {
            if(doorway.isExit)
                exits.Add(doorway);
        }
        
        return exits;
    }
}
