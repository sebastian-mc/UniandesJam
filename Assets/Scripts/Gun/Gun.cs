using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject projectile;
    public Transform gunHolder;

    private bool _shooting;
    private int i = 1;

    void Start()
    {
        FindObjectOfType<Syncher>().OnQuarter += Shoot;
    }

    public void Aim(Vector3 lookPoint)
    {
        gunHolder.transform.LookAt(lookPoint);
        gunHolder.transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    public void Shooting(bool shoot)
    {
        _shooting = shoot;
    }

    void Shoot()
    {
        if (_shooting)
        {
            GameObject newProjectile = Instantiate(projectile);
            newProjectile.transform.position = gunHolder.transform.position;
            newProjectile.transform.eulerAngles = gunHolder.transform.eulerAngles;
        }
    }
}
