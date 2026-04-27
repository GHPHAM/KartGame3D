/***************************************************************
*file: Fireball.cs
*author: Cole Harsch
*class: CS 4700 � Game Development
*assignment: final program
*date last modified: 4/18/26
*
*purpose: This defines the behavior for the Fireball projectile
*
****************************************************************/

using Entities;
using UnityEngine;

public class FireballBehaviour : ProjectileBase
{
    public float homingRange;
    
    // Behaviour -----------------------------------

    // look and set velocity toward target
    public override void setTarget(Transform t)
    {
        if (_target == null)
        {
            _rb.linearVelocity = transform.forward  * speed;
            return;
        }
        
        transform.LookAt(t);
        
        _rb.linearVelocity = (t.position - transform.position).normalized  * speed;
    }
}
