/***************************************************************
*file: TurretBehaviour.cs
*author: Cole Harsch
*class: CS 4700 – Game Development
*assignment: final program
*date last modified: 4/18/26
*
*purpose: This handles the behaviour of the turret enemy
*
****************************************************************/
using UnityEngine;

public class TurretBehaviour : EnemyBase
{
    //public
    public GameObject projectile;
    public GameObject target;

    //internals


    // Setup -----------------------------------
    override protected void Awake()
    {
        //base setup
        base.Awake();

        //setup internals
        target = VehicleController.singleton.gameObject;
    }


    // Behaviour -----------------------------------
    protected override void attack()
    {
         Instantiate(projectile);
    }
}
