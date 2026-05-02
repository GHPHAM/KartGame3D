using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomDoorway : MonoBehaviour
{
    public bool isEntrance;
    public bool isExit;
    
    [Tooltip("Spot at which the room aligns to, defaults to this transform")]
    public Transform anchor;
    
    [Tooltip("List of possible rooms which can spawn after this does")]
    [SerializeField] List<GameObject> nextRooms;
    
    // Internals -------------------------------------------------------------
        
    protected RoomController _controller;

    // Behaviour -------------------------------------------------------------

    private void Awake()
    {
        if(anchor == null)
            anchor = transform;
    }

    public virtual void assignRoom(RoomController room)
    {
        _controller = room;
        room.onRoomCleared += onRoomClear;
    }

    public void onRoomClear()
    {
        spawnRoomAsync();
        RoomTracker.singleton.roomCleared();
    }

    protected void openDoor()
    {
        //play animation to open door
    }

    async protected void spawnRoomAsync()
    {
        //if this is not being overrided create it from the doorway's spesefications
        if (!RoomTracker.singleton.getOverrideRoomPrefab(out GameObject room))
        {
            room = nextRooms[Random.Range(0, nextRooms.Count)];
        }
        
        if (room == null)
        {
            Debug.LogError("Room object is null.");
            return;
        }
        
        Debug.Log("Spawning room...");
        //spawn the room attached at the correct position
        var op = InstantiateAsync(room);

        await op;
        
        Debug.Log("Prepping room...");
        
        //spawn the room attached at the correct position
        if (!op.Result[0].TryGetComponent(out RoomController controller))
        {
            Debug.LogError("Room controller is null.");
            return;
        }
        
        controller.prepRoom(anchor);
        
        Debug.Log("Room Spawned");
        openDoor();
    }
}
