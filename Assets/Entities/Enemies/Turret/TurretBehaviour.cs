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
    [Header("Turret")]

    //public
    public GameObject projectile;
    public Transform shootPoint;

    [Tooltip("Part of the body which follows the player")]
    public Transform head;


    //internals
    protected Transform _target;


    // Setup -----------------------------------
    override protected void Awake()
    {
        //base setup
        base.Awake();

        //setup internals
        _target = VehicleController.singleton.transform;
    }


    // Behaviour -----------------------------------


    //follow target with head
    private void Update()
    {
        if(_target != null)
        {
            head.LookAt(_target);
        }
    }


    //spawn attack projectile
    protected override void attack()
    {
        GameObject instance = 
            Instantiate(projectile, shootPoint.position, shootPoint.rotation);

        instance.gameObject.layer = gameObject.layer;

        instance.GetComponent<ProjectileBase>().setTarget(_target);
    }
}
