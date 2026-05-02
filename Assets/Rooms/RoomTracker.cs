using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomTracker : MonoBehaviour
{
    public Dictionary<int, GameObject> overrideRooms = new Dictionary<int, GameObject>();
    protected GameObject startingRoom;
    
    public static RoomTracker singleton;
    
    // Get/Set ------------------------------------------------------------------
    
    protected int _roomsCleared = 1;

    public int roomsCleared
    {
        get;
    }
    
    
    // Behaviour ------------------------------------------------------------------

    private void Awake()
    {
        if(singleton == null)
            singleton = this;
    }

    public bool getOverrideRoomPrefab(out GameObject room)
    {
        if (overrideRooms.ContainsKey(_roomsCleared))
        {
            room = overrideRooms[roomsCleared];
            return true;
        }
        room = null;
        
        return false;
    }

    public void roomCleared()
    {
        _roomsCleared++;
    }
}
