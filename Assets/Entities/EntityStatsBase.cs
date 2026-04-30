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
        
        [Tooltip("Transform to target projectiles to when being fired at, defaults to this object's transform")]
        [SerializeField] protected Transform _hitTarget;
        [SerializeField] protected int _health;
        
        public event System.Action onDeath = delegate { };
        
        // ---- Get/Set ----------------------------------


        public Transform hitTarget
        {
            get{
                if (_hitTarget == null) return transform;
                return _hitTarget;
            }
            set{
                _hitTarget = value;
            }
        }

        public int health
        {
            get => _health;
            set
            {
                if(value < 0)
                {
                    onDeath();
                }
                
                _health = value;
            }
        }

        // ---- Behaviour ----------------------------------

        protected virtual void Awake()
        {
            if (_hitTarget == null)
                _hitTarget = transform;

            onDeath += die;
        }
        
        
        //damage the enitiy
        public virtual void damage(int damage)
        {
            health -= damage;
        }

        //entity death (health = 0)
        public virtual void die()
        {
            Debug.Log("Dying");
            Destroy(gameObject);
        }
    }
}