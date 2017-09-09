using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTurret : Entity
{
    public Turret turret;

    public override void TakeDamage()
    {
        turret.TakeDamage();
    }
}
