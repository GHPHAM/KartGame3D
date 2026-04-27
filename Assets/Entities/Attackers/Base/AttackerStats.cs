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

    [SerializeField] private float _currentCooldown;

    // Setup -----------------------------------

    //setup internals
    protected override void Awake()
    {
        base.Awake();
        _currentCooldown = currentStats.attackCooldown;
    }


    // Behaviour -----------------------------------

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        cooldownAttack();
    }


    protected void cooldownAttack()
    {
        _currentCooldown += Time.fixedDeltaTime;

        if(_currentCooldown > currentStats.attackCooldown)
        {
            _currentCooldown = 0;
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
