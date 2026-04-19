/***************************************************************
*file: EnemyBase.cs
*author: Cole Harsch
*class: CS 4700 – Game Development
*assignment: final program
*date last modified: 4/18/26
*
*purpose: This houses the base stats for all entities (players and enemies)
*
****************************************************************/
using UnityEngine;

public abstract class EntityStats : MonoBehaviour
{
    [Header("Base")]

    // public
    public int maxHealth = 10;

    // internals
    [SerializeField] protected int _health;


    // Get/Set ----------------------------------

    public int health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (health <= 0)
                die();
        }
    }

    // manage internals ----------------------

    //damage the enitiy
    public virtual void damage(int damage)
    {
        health -= damage;
    }

    //entity death (health = 0)
    public abstract void die();
}
