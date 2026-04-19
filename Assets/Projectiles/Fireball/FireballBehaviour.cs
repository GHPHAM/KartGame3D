/***************************************************************
*file: Fireball.cs
*author: Cole Harsch
*class: CS 4700 – Game Development
*assignment: final program
*date last modified: 4/18/26
*
*purpose: This defines the behavior for the Fireball projectile
*
****************************************************************/
using UnityEngine;

public class FireballBehaviour : ProjectileBase
{
    // Behaviour -----------------------------------

    // look and set velocity toward target
    public override void setTarget(Transform t)
    {
        transform.LookAt(t);

        _rb.linearVelocity = (t.position - transform.position).normalized  * speed;
    }
}
