using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    public int startingHitPoints;

    public int hitPoints { get; protected set; }
    protected bool dead;
    public GameObject deathFx;
    public event System.Action OnDeath;
    public Image[] hits;

    public enum State
    {
        Spawning,
        Active,
        Inactive,
        Dead
    }
    public State _state;

    protected virtual void Start()
    {
        hitPoints = startingHitPoints;
    }

    public virtual void TakeDamage()
    {
        hitPoints--;
        hits[hitPoints].enabled = false;
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
        GameObject death = Instantiate(deathFx);
        death.transform.position = transform.position;
        Destroy(death, 5f);
        Destroy(gameObject);
    }
}