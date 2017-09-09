using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Player: Entity
{
    public float moveSpeed = 5;
    public GameObject deathFx;

    private Rigidbody _rigidBody;

    void Awake()
    {
        //FindObjectOfType<Spawner>().OnNewWave += OnNewWave;
    }

    protected override void Start()
    {
        base.Start();
        _rigidBody = GetComponent<Rigidbody>();
    }

    void OnNewWave(int waveNumber)
    {
        hitPoints = startingHitPoints;
    }

    public void Move(Vector3 movement, Vector3 lookPoint)
    {
        _rigidBody.velocity = movement * moveSpeed;
        transform.LookAt(lookPoint);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    public override void Die()
    {
        //AudioManager.instance.PlaySound("PlayerDeath", transform.position);
        GameObject death = Instantiate(deathFx);
        death.transform.position = transform.position;
        Destroy(death, 5f);
        base.Die();
    }
}
