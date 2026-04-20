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
    public class VehicleStatModifier
    {
        // -------- Identifier ----------------------------------------
        
        //used for Equals
        public string identifier; 
        
        // -------- Internals ----------------------------------------


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

        [Tooltip("Minimum speed required to steer")]
        public float _minSpeedToSteer;

        [Tooltip("Speed at which player's turing reaches the steerDegreese")]
        public float _handling;
        
        // -------- Get/Set ----------------------------------------
        
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
        public virtual float minSpeedToSteer 
        {
            get => _minSpeedToSteer; 
            set => _minSpeedToSteer = value;
        }
        public virtual float handling 
        {
            get => _handling; 
            set => _handling = value;
        }
        
        // -------- Constructor ----------------------------------------
        
        //copy
        public VehicleStatModifier(VehicleStatModifier other)
        {
            _maxSpeed = other.maxSpeed;
            _maxReverseSpeed = other.maxReverseSpeed;
            _accelerationForce = other.accelerationForce;
            _brakeForce = other.brakeForce;
            _naturalDeceleration = other.naturalDeceleration;
            _maxSteer = other.maxSteer;
            _minSpeedToSteer = other.minSpeedToSteer;
            _handling = other.handling;
        }
        
        public VehicleStatModifier(
            float maxSpeed = 0, 
            float maxReverseSpeed = 0,
            float accelerationForce = 0,
            float brakeForce = 0,
            float naturalDeceleration = 0,
            float maxSteer = 0,
            float minSpeedToSteer = 0,
            float handling = 0
            )
        {
            this.maxSpeed = maxSpeed;
            this.maxReverseSpeed = maxReverseSpeed;
            this.accelerationForce = accelerationForce;
            this.brakeForce = brakeForce;
            this.naturalDeceleration = naturalDeceleration;
            this.maxSteer = maxSteer;
            this.minSpeedToSteer = minSpeedToSteer;
            this.handling = handling;
        }
        
        public VehicleStatModifier add(VehicleStatModifier other)
        {
            maxSpeed += other.maxSpeed;
            maxReverseSpeed += other.maxReverseSpeed;
            accelerationForce += other.accelerationForce;
            brakeForce += other.brakeForce;
            naturalDeceleration += other.naturalDeceleration;
            maxSteer += other.maxSteer;
            minSpeedToSteer += other.minSpeedToSteer;
            handling += other.handling;
            return this;
        }

        public VehicleStatModifier remove(VehicleStatModifier other)
        {
            maxSpeed -= other.maxSpeed;
            maxReverseSpeed -= other.maxReverseSpeed;
            accelerationForce -= other.accelerationForce;
            brakeForce -= other.brakeForce;
            naturalDeceleration -= other.naturalDeceleration;
            maxSteer -= other.maxSteer;
            minSpeedToSteer -= other.minSpeedToSteer;
            handling -= other.handling;
            return this;
        }

        public class Builder
        {
            float _maxSpeed = 0;
            float _maxReverseSpeed = 0;
            float _accelerationForce = 0;
            float _brakeForce = 0;
            float _naturalDeceleration = 0;
            float _steerSpeed = 0;
            float _minSpeedToSteer = 0;
            float _handling = 0;
            
            public Builder(
                float maxSpeed = 0, 
                float maxReverseSpeed = 0,
                float accelerationForce = 0,
                float brakeForce = 0,
                float naturalDeceleration = 0,
                float steerSpeed = 0,
                float minSpeedToSteer = 0,
                float handling = 0
            )
            {
                _maxSpeed = maxSpeed;
                _maxReverseSpeed = maxReverseSpeed;
                _accelerationForce = accelerationForce;
                _brakeForce = brakeForce;
                _naturalDeceleration = naturalDeceleration;
                _steerSpeed = steerSpeed;
                _minSpeedToSteer = minSpeedToSteer;
                _handling = handling;
            }

            public Builder maxSpeed(float maxSpeed)
            {
                _maxSpeed = maxSpeed;
                return this;
            }

            public Builder maxReverseSpeed(float maxReverseSpeed)
            {
                _maxReverseSpeed = maxReverseSpeed;
                return this;
            }

            public Builder accelerationForce(float accelerationForce)
            {
                _accelerationForce = accelerationForce;
                return this;
            }

            public Builder brakeForce(float brakeForce)
            {
                _brakeForce = brakeForce;
                return this;
            }

            public Builder naturalDeceleration(float naturalDeceleration)
            {
                _naturalDeceleration = naturalDeceleration;
                return this;
            }

            public Builder steerSpeed(float steerSpeed)
            {
                _steerSpeed = steerSpeed;
                return this;
            }

            public Builder minSpeedToSteer(float minSpeedToSteer)
            {
                _minSpeedToSteer= minSpeedToSteer;
                return this;
            }

            public Builder handling(float handling)
            {
                _handling = handling;
                return this;
            }

            public VehicleStatModifier Build()
            {
                return new VehicleStatModifier(
                        _maxSpeed,
                        _maxReverseSpeed,
                        _accelerationForce,
                        _brakeForce,
                        _naturalDeceleration,
                        _steerSpeed,
                        _minSpeedToSteer,
                        _handling
                    );
            }
        }
        
        
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            
            return obj is VehicleStatModifier other && identifier.Equals(other.identifier);
        }
        
        
    }
}