/***************************************************************
*file: VehicleStats.cs
*author: Ryan Davies
*class: CS 4700 – Game Development
*assignment: final program
*date last modified: 4/18/26
*
*purpose: House Changes for Vehicle Base Stats for the Modifiable Vehicle Base
*
****************************************************************/

using UnityEngine;

namespace Entities.Vehicle.Modifiable
{
    
    [System.Serializable]
    public class VehicleStatsModifier : AttackerStatsModifier
    {
        // -------- Internals ----------------------------------------

        
        [Tooltip("Amount of ram damage dealt when ramming into enemy")]
        public int _ramDamage;

        [Tooltip("Minimum speed to deal ram damage")]
        public float _minRamSpeed;

        [Tooltip("Maximum speed player can reach")]
        public float _maxSpeed;

        [Tooltip("Maximum reverse speed player can reach")]
        public float _maxReverseSpeed;

        [Tooltip("Speed at which player Accelerates towards Maximum speed")]
        public float _accelerationForce;

        [Tooltip("Speed at which player Breaks")]
        public float _brakeForce;

        [Tooltip("Speed at which the player slows without providing input")]
        public float _naturalDeceleration;

        [Tooltip("Max turn speed")]
        public float _maxSteer;

        [Tooltip("Speed at which player's turing reaches the steerDegreese")]
        public float _handling;
        
        
        // -------- Get/Set ----------------------------------------
        
        public virtual int ramDamage
        {
            get => _ramDamage; 
            set => _ramDamage = value;
        }
        public virtual float minRamSpeed
        {
            get => _minRamSpeed; 
            set => _minRamSpeed = value;
        }
        public virtual float maxSpeed
        {
            get => _maxSpeed; 
            set => _maxSpeed = value;
        }
        public virtual float maxReverseSpeed 
        {
            get => _maxReverseSpeed; 
            set => _maxReverseSpeed = value;
        }
        public virtual float accelerationForce 
        {
            get => _accelerationForce; 
            set => _accelerationForce = value;
        }
        public virtual float brakeForce 
        {
            get => _brakeForce; 
            set => _brakeForce = value;
        }
        public virtual float naturalDeceleration 
        {
            get => _naturalDeceleration; 
            set => _naturalDeceleration = value;
        }
        public virtual float maxSteer 
        {
            get => _maxSteer; 
            set => _maxSteer = value;
        }
        public virtual float handling 
        {
            get => _handling; 
            set => _handling = value;
        }
        
        // -------- Constructors ----------------------------------------
        
        //copy
        public VehicleStatsModifier(VehicleStatsModifier other) : base(other)
        {
            if (other == null)
                return;
            
            _ramDamage = other.ramDamage;
            _minRamSpeed = other._minRamSpeed;
            _maxSpeed = other.maxSpeed;
            _maxReverseSpeed = other.maxReverseSpeed;
            _accelerationForce = other.accelerationForce;
            _brakeForce = other.brakeForce;
            _naturalDeceleration = other.naturalDeceleration;
            _maxSteer = other.maxSteer;
            _handling = other.handling;
        }
        
        public VehicleStatsModifier(
            EntityStatsModifier baseStats = null,
            int ramDamage = 0, 
            float minRamSpeed = 0, 
            float maxSpeed = 0, 
            float maxReverseSpeed = 0,
            float accelerationForce = 0,
            float brakeForce = 0,
            float naturalDeceleration = 0,
            float maxSteer = 0,
            float handling = 0
            ) : base(baseStats)
        {
            this.ramDamage = ramDamage;
            this.minRamSpeed = minRamSpeed;
            this.maxSpeed = maxSpeed;
            this.maxReverseSpeed = maxReverseSpeed;
            this.accelerationForce = accelerationForce;
            this.brakeForce = brakeForce;
            this.naturalDeceleration = naturalDeceleration;
            this.maxSteer = maxSteer;
            this.handling = handling;
        }
        
        public override EntityStatsModifier clone() => new VehicleStatsModifier(this);
        
        
        // -------- Methods ----------------------------------------

        
        public VehicleStatsModifier add(VehicleStatsModifier other)
        {
            add((EntityStatsModifier)other);
            ramDamage += other.ramDamage;
            minRamSpeed += other.minRamSpeed;
            maxSpeed += other.maxSpeed;
            maxReverseSpeed += other.maxReverseSpeed;
            accelerationForce += other.accelerationForce;
            brakeForce += other.brakeForce;
            naturalDeceleration += other.naturalDeceleration;
            maxSteer += other.maxSteer;
            handling += other.handling;
            return this;
        }

        public VehicleStatsModifier remove(VehicleStatsModifier other)
        {
            remove((EntityStatsModifier)other);
            ramDamage -= other.ramDamage;
            minRamSpeed -= other.minRamSpeed;
            maxSpeed -= other.maxSpeed;
            maxReverseSpeed -= other.maxReverseSpeed;
            accelerationForce -= other.accelerationForce;
            brakeForce -= other.brakeForce;
            naturalDeceleration -= other.naturalDeceleration;
            maxSteer -= other.maxSteer;
            handling -= other.handling;
            return this;
        }
        
        
    }
}