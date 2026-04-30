/***************************************************************
*file: EnemyStats.cs
*author: Cole Harsch
*class: CS 4700 � Game Development
*assignment: final program
*date last modified: 4/18/26
*
*purpose: This houses the base stats for all entities (players and enemies), it is the
 * generic version of EntityStatsBase which is used for inheritance into other Entities
*
****************************************************************/

using System;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public abstract class EntityStats<T> : EntityStatsBase where T : EntityStatsModifier
{
    // ---- Generics ----------------------------------
    
    [Header("Base")]
    public T baseStats;
    public T currentStats;
    public List<T> modifiers = new List<T>();
    
    public T calculateStats()
    {
        //copy the stats as the T class
        T modifiedStats = baseStats.clone() as T;
        
        //add each T class using its add function
        //if it is one which this doesn't cover then this adds the stats we do cover
        foreach(T mod in modifiers)
        {
            modifiedStats.add(mod);
        }
        
        return modifiedStats;
    }

    public void addModifier(T modifier)
    {
        modifiers.Add(modifier);
    }
    
    public void removeModifier(T modifier)
    {
        modifiers.Remove(modifier);
    }
    
    // ---- Behaviour ----------------------------------

    
    protected virtual void Awake()
    {
        base.Awake();
        
        //calculate stats once every frame
        currentStats = calculateStats();
    }
    
    protected virtual void FixedUpdate()
    {
        //calculate stats once every frame
        currentStats = calculateStats();
    }
}
