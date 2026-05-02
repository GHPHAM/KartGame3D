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
    [Header("Stats")]
    public int damage = 1;
    public int speed = 1;
    public float lifetime = 10f;
    
    [Header("Auto Targeting")]
    public float autoTargetRange = 10f;
    public float autoTargetMaxAngle = 10f;
    public LayerMask targetableLayers;
    public LayerMask collidableLayers;

    //internals
    [Header("Internals")]
    [SerializeField] protected double ownerGracePeriod = .5f;
    protected Transform _target;
    protected Transform _owner;
    protected Rigidbody _rb;
    protected double _spawnTime;
    protected float _age = 0f;
    


    // Setup -----------------------------------
    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _spawnTime = Time.time;
    }

    public virtual void setup(Transform owner, Transform target)
    {
        setOwner(owner);
        setTarget(target);
    }
    
    public virtual void setOwner(Transform owner)
    {
        _owner = owner;
    }
    
    public virtual void setTarget(Transform target)
    {
        if (target == null)
        {
            target = findTarget();
        }

        _target = target;
    }

    // Behaviour -----------------------------------
    
    private void Update()
    {
        _age += Time.deltaTime;

        if (_age > lifetime) {
            timeout();
        }
    }


    public Transform findTarget()
    {
        Transform candidate = null;
        float candidateAngle = autoTargetMaxAngle;
        
        foreach(Collider col in
        Physics.OverlapSphere(transform.position, autoTargetRange, targetableLayers))
        {
            //check if this is an entity
            if (!col.gameObject.TryGetComponent<EntityStatsBase>(out var stats))
                continue;

            Transform target = stats.hitTarget;
            
            //skip if we are targeting the owner
            if (col.transform.IsChildOf(_owner))
                continue;
            
            //get angle
            Vector3 direction = (target.position - transform.position).normalized; 
            
            float angle = Vector3.Angle(transform.forward, direction);

            //if this is withing the cone angle this is a possible target and we habe line of sight
            if (angle < candidateAngle &&
                Physics.Raycast(transform.position, direction, out RaycastHit hit, autoTargetRange,
                    collidableLayers) &&
                hit.collider == col)
            {
                //we have line of sight
                candidate = target;
                candidateAngle = angle;
            }
        }

        
        return candidate;
    }

    //handles collision for trigger projectiles
    private void OnTriggerEnter(Collider other)
    {
        //if the owner is being hit, and it is still in grace time don't do anything
        if (_owner != null && other.transform.IsChildOf(_owner) && _spawnTime + ownerGracePeriod > Time.time)
        {
            return;
        }
        
        if (other.TryGetComponent<EntityStatsBase>(out var stats))
        {
            onHitEntity(stats);
        }
    }


    //handles collision for collider projectiles
    private void OnCollisionEnter(Collision collision)
    {
        //if the owner is being hit, and it is still in grace time don't do anything
        if (_owner != null && collision.transform.IsChildOf(_owner) && _spawnTime + ownerGracePeriod > Time.time)
        {
            return;
        }
        
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
