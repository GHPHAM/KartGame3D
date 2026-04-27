/***************************************************************
 *file: EnemyStatsBase.cs
 *author: Cole Harsch
 *class: CS 4700 � Game Development
 *assignment: final program
 *date last modified: 4/26/26
 *
 *purpose: This is the Base class for EntityStats, it houses the sharded logic of all Entities
 * and is used to hook onto the derived classes for generic calls
 *
 ****************************************************************/

using UnityEngine;

namespace Entities
{
    public abstract class EntityStatsBase : MonoBehaviour
    {
        // ---- Internals ----------------------------------
        
        
        [SerializeField] protected int _health;
        
        
        // ---- Get/Set ----------------------------------


        public int health
        {
            get => _health;
            set => _health = value;
        }
        
        // ---- Behaviour ----------------------------------
        
        
        //damage the enitiy
        public virtual void damage(int damage)
        {
            _health -= damage;
        }

        //entity death (health = 0)
        public abstract void die();
    }
}