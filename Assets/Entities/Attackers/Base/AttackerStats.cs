/***************************************************************
*file: EnemyBase.cs
*author: Cole Harsch
*class: CS 4700 � Game Development
*assignment: final program
*date last modified: 4/18/26
*
*purpose: This houses the base stats for all enimes and some basic mechanics
*
****************************************************************/

using Entities;
using UnityEngine;

public abstract class AttackerStats<T> : EntityStats<T> where T : AttackerStatsModifier
{
    [Header("Attacker")]

    private double lastAttackTime = 0;

    
    // Behaviour -----------------------------------


    protected void attemptAttack()
    {
        if(Time.timeAsDouble - lastAttackTime > currentStats.attackCooldown)
        {
            lastAttackTime = Time.timeAsDouble;
            attack();
        }
    }


    //handles attack, does nothing for base class
    protected virtual void attack()
    {
        return;
    }

    public override void die()
    {
        //enemy is dead here, temp destroy but maybe add cool effects here later
        Destroy(gameObject);
    }
}
