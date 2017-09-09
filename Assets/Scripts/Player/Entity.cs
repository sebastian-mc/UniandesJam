using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int startingHitPoints;

    public int hitPoints { get; protected set; }
    protected bool dead;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        hitPoints = startingHitPoints;
    }

    public virtual void TakeHit(Vector3 hitpoint, Vector3 hitDirection)
    {
        TakeDamage();
    }

    public virtual void TakeDamage()
    {
        hitPoints--;

        if (hitPoints <= 0 && !dead)
        {
            Die();
        }
    }

    [ContextMenu("Self Destruct")]
    public virtual void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        Destroy(gameObject);
    }
}