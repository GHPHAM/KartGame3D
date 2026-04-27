using UnityEngine;

namespace Entities
{
    [System.Serializable]
    public class AttackerStatsModifier : EntityStatsModifier
    {
        
        // -------- Internals ----------------------------------------

        
        [Tooltip("How much damage attacks do")]
        public int _attackDamage;

        [Tooltip("How much time it takes to attack again after attacking")]
        public float _attackCooldown;
        
        
        // -------- Get/Set ----------------------------------------
        
        
        public virtual int attackDamage
        {
            get => _attackDamage; 
            set => _attackDamage = value;
        }
        public virtual float attackCooldown
        {
            get => _attackCooldown; 
            set => _attackCooldown = value;
        }
        
        
        
        // -------- Constructors ----------------------------------------
        
        
        
        //copy
        public AttackerStatsModifier(AttackerStatsModifier other) : base(other)
        {
            if (other == null)
                return;
            
            _attackDamage = other.attackDamage;
            _attackCooldown = other._attackCooldown;
        }
        
        public AttackerStatsModifier(
            EntityStatsModifier baseStats = null,
            int attackDamage = 0, 
            float attackCooldown = 10
            ) : base(baseStats)
        {
            this.attackDamage = attackDamage;
            this.attackCooldown = attackCooldown;
        }
        
        public override EntityStatsModifier clone() => new AttackerStatsModifier(this);
        
        // -------- Methods ----------------------------------------

        
        public AttackerStatsModifier add(AttackerStatsModifier other)
        {
            add((EntityStatsModifier)other);
            attackDamage += other.attackDamage;
            attackCooldown += other.attackCooldown;
            
            return this;
        }

        public AttackerStatsModifier remove(AttackerStatsModifier other)
        {
            remove((EntityStatsModifier)other);
            attackDamage -= other.attackDamage;
            attackCooldown -= other.attackCooldown;
            return this;
        }
        
        
        
        
    }
}