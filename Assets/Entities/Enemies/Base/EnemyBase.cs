/***************************************************************
*file: EnemyBase.cs
*author: Cole Harsch
*class: CS 4700 – Game Development
*assignment: final program
*date last modified: 4/18/26
*
*purpose: This houses the base stats for all enimes and some basic mechanics
*
****************************************************************/

using UnityEngine;

public abstract class EnemyBase : EntityStats
{
    //public
    public int attackDamage = 2;
    public float attackCooldown = .5f;

    [SerializeField] private float _currentCooldown;

    // Setup -----------------------------------

    //setup internals
    protected virtual void Awake()
    {
        _currentCooldown = attackCooldown;
    }


    // Behaviour -----------------------------------

    protected void FixedUpdate()
    {
        cooldownAttack();
    }


    protected void cooldownAttack()
    {
        _currentCooldown += Time.deltaTime;

        if(_currentCooldown > attackCooldown)
        {
            _currentCooldown = 0;
            attack();
        }
    }


    //handles attack
    protected abstract void attack();

    public override void die()
    {
        //enemy is dead here, temp destroy but maybe add cool effects here later
        Destroy(gameObject);
    }
}
