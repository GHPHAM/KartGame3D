using UnityEngine;

namespace Entities
{
    [System.Serializable]
    public class EntityStatsModifier
    {
        // -------- Identifier ----------------------------------------
        
        
        //used for Equals
        public string identifier; 
        
        
        // -------- Internals ----------------------------------------
        
        
        [Tooltip("Maximum health player can reach")]
        public int _maxHealth;
        
        public virtual int maxHealth
        {
            get => _maxHealth; 
            set => _maxHealth = value;
        }

        
        // -------- Constructors ----------------------------------------
        
        public EntityStatsModifier(EntityStatsModifier other)
        {
            if (other == null)
                return;
            
            this.maxHealth = other.maxHealth;
        }
        
        public EntityStatsModifier(int maxHealth)
        {
            this.maxHealth = maxHealth;
        }

        public virtual EntityStatsModifier clone()
        {
            return new EntityStatsModifier(this);
        }
        
        // -------- Methods ----------------------------------------
        
        
        public EntityStatsModifier add(EntityStatsModifier other)
        {
            maxHealth += other.maxHealth;
            
            return this;
        }

        public EntityStatsModifier remove(EntityStatsModifier other)
        {
            maxHealth -= other.maxHealth;
            
            return this;
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            
            return obj is EntityStatsModifier other && identifier.Equals(other.identifier);
        }
    }
}