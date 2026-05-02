using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;
using Random = System.Random;

public class RoomController : MonoBehaviour
{
    public List<EnemySpawn> enemySpawns;
    
    // internals --------------------------------------------

    protected List<EntityStatsBase> _inhabitants = new List<EntityStatsBase>();
    [SerializeField] protected List<RoomDoorway> _doorways = new  List<RoomDoorway>();
    
    public event System.Action<EntityStatsBase> onInhabitantCleared = delegate { };
    public event System.Action onRoomCleared = delegate { };

    
    // Behaviour --------------------------------------------

    public void onInhabitantDeath(EntityStatsBase inhabitant)
    {
        _inhabitants.Remove(inhabitant);
        onInhabitantCleared(inhabitant);
        if(_inhabitants.Count == 0)
            onRoomCleared();
    }
    
    public void addInhabitant(EntityStatsBase inhabitant)
    {
        _inhabitants.Add(inhabitant);
    }


    public bool isRoomCleared()
    {
        return _inhabitants.Count == 0;
    }
    
    
    public List<RoomDoorway> getPossibleEntrances()
    {
        var exits = new List<RoomDoorway>();

        foreach (var doorway in  _doorways)
        {
            if(doorway.isEntrance)
                exits.Add(doorway);
        }
        
        return exits;
    }
    
    public List<RoomDoorway> getPossibleExits()
    {
        var exits = new List<RoomDoorway>();

        foreach (var doorway in  _doorways)
        {
            if(doorway.isExit)
                exits.Add(doorway);
        }
        
        return exits;
    }

    public RoomDoorway popEntranceRandom()
    {
        List<RoomDoorway> entrances = getPossibleEntrances();

        RoomDoorway entrance = entrances[new Random().Next(0, entrances.Count - 1)];
        
        _doorways.Remove(entrance);
        return entrance;
    }
    
    public RoomDoorway popExitRandom()
    {
        List<RoomDoorway> exits = getPossibleExits();

        RoomDoorway exit = exits[new Random().Next(0, exits.Count - 1)];
        
        _doorways.Remove(exit);
        return exit;
    }
    
    
    public bool getOverrideRoomPrefab(out GameObject room)
    {
        return  getOverrideRoomPrefab(out room);
    }
    
    // Spawning -------------------------------------------

    public void prepRoom(Transform exit)
    {
        //get a random entrance
        var newEntrance = popEntranceRandom();

        //rotate the room
        transform.rotation = Quaternion.identity;
        transform.rotation = exit.transform.rotation * Quaternion.Euler(0, 180, 0) * Quaternion.Inverse(newEntrance.transform.rotation);
        //move the room
        transform.position = exit.transform.position + (transform.position - newEntrance.transform.position);
        
        //select an exit
        var newExit = popExitRandom();
        
        //remove all entrance doors from this room but the exit
        foreach (var entrance in getPossibleEntrances())
        {
            _doorways.Remove(entrance);
            Destroy(entrance.gameObject);
        }
        
        newExit.assignRoom(this);
        
        //spawn enemies
        foreach (EnemySpawn spawns in enemySpawns)
        {
            spawns.spawn(this);
        }
        
        //all done
    }
}
