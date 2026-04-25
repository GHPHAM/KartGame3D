/***************************************************************
*file: PowerUpData.cs
*author: Ryan Davies
*class: CS 4700 – Game Development
*assignment: final program
*date last modified: 4/24/26
*
*purpose: Stores data for powerups that can be applied to the player vehicle.
*This includes the type of powerup, how long it lasts, and how strong it is. 
*
****************************************************************/
using UnityEngine;

public enum PowerUpLifetime
{
    Temporary,
    WholeRun,
    Permanent
}

public enum PowerUpType
{
    Reflect,
    SpeedBoost,
    DamageBoost,
    HealthRestore
}

[CreateAssetMenu(fileName = "PowerUp", menuName = "ScriptableObjects/PowerUp")]
public class PowerUpData : ScriptableObject
{
    public string powerUpName;
    public PowerUpLifetime lifetime;
    public PowerUpType type;

    public float duration;
    public float magnitude;
    public bool isStackable;
}