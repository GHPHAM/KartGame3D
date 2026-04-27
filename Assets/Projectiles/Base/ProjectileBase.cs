/***************************************************************
*file: ProjectileBase.cs
*author: Cole Harsch
*class: CS 4700 � Game Development
*assignment: final program
*date last modified: 4/18/26
*
*purpose: This houses the base stats and behavior for all projectiles
*
****************************************************************/

using Entities;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBase : MonoBehaviour
{
    //public
    public int damage = 1;
    public int speed = 1;
    public float lifetime = 10f;

    //internals
    [Tooltip("Target for the projectile, in case any sub classes need to track")]
    protected Transform _target;
    protected Rigidbody _rb;
    protected float _age = 0f;


    // Setup -----------------------------------
    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public virtual void setTarget(Transform t)
    {
        _target = t;
    }

    // Behaviour -----------------------------------
    
    private void Update()
    {
        _age += Time.deltaTime;

        if (_age > lifetime) {
            timeout();
        }
    }


    //handles collision for trigger projectiles
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EntityStatsBase>(out var stats))
        {
            onHitEntity(stats);
        }
        else
        {
            onHitWall(other);
        }
    }


    //handles collision for collider projectiles
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<EntityStatsBase>(out var stats))
        {
            onHitEntity(stats);
        }
        else
        {
            onHitWall(collision.collider);
        }
    }


    //called when the projectile hits an entity
    protected virtual void onHitEntity(EntityStatsBase entity)
    {
        entity.damage(damage);
        Destroy(gameObject);
    }

    //called when the projectile hits a wall
    protected virtual void onHitWall(Collider other)
    {
        Destroy(gameObject);
    }

    //called for the destruction of the projectile on colission, used for effects
    protected virtual void destroy()
    {
        Destroy(gameObject);
    }

    //called for the destruction of the projectile on age, used for effects
    protected virtual void timeout()
    {
        destroy();
    }
}
