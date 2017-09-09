using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject projectile;

    private bool _shooting;
    private int i = 1;

    void Start()
    {
        GameObject.FindObjectOfType<Syncher>().OnQuarter += Shoot;
    }

    public void Aim(Vector3 lookPoint)
    {
        transform.LookAt(lookPoint);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    public void Shooting(bool shoot)
    {
        _shooting = shoot;
    }

    void Shoot()
    {
        if(_shooting)
        {
            GameObject newProjectile = Instantiate(projectile);
            newProjectile.transform.position = transform.position;
            newProjectile.transform.eulerAngles = transform.eulerAngles;
        }
    }
}
