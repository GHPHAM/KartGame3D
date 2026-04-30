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

        [Tooltip("How many points the player earns for killing this entity")]
        public int _scoreValue;


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
        public virtual int scoreValue
        {
            get => _scoreValue;
            set => _scoreValue = value;
        }



        // -------- Constructors ----------------------------------------



        //copy
        public AttackerStatsModifier(AttackerStatsModifier other) : base(other)
        {
            if (other == null)
                return;

            _attackDamage = other.attackDamage;
            _attackCooldown = other._attackCooldown;
            _scoreValue = other._scoreValue;
        }

        public AttackerStatsModifier(
            EntityStatsModifier baseStats = null,
            int attackDamage = 0,
            float attackCooldown = 10,
            int scoreValue = 0
            ) : base(baseStats)
        {
            this.attackDamage = attackDamage;
            this.attackCooldown = attackCooldown;
            this.scoreValue = scoreValue;
        }

        public override EntityStatsModifier clone() => new AttackerStatsModifier(this);

        // -------- Methods ----------------------------------------


        public AttackerStatsModifier add(AttackerStatsModifier other)
        {
            add((EntityStatsModifier)other);
            attackDamage += other.attackDamage;
            attackCooldown += other.attackCooldown;
            scoreValue += other.scoreValue;

            return this;
        }

        public AttackerStatsModifier remove(AttackerStatsModifier other)
        {
            remove((EntityStatsModifier)other);
            attackDamage -= other.attackDamage;
            attackCooldown -= other.attackCooldown;
            scoreValue -= other.scoreValue;
            return this;
        }




    }
}