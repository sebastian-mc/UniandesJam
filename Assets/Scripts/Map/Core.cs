using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : Entity
{
    public Transform shell;

    private int crystals = 0;
    private bool shellOn;

    protected override void Start()
    {
        base.Start();
        shellOn = true;
        Crystal[] cry = FindObjectsOfType<Crystal>();
        for(int i = 0; i< cry.Length; i++)
        {
            crystals++;
            cry[i].OnDeath += CrystalDestroyed;
        }
    }

    void CrystalDestroyed()
    {
        crystals--;
        if(crystals == 0)
        {
            shellOn = false;
            StartCoroutine(ShellOff());
        }
    }

    public override void TakeDamage()
    {
        if (shellOn) return;
        base.TakeDamage();
    }

    public override void Die()
    {

        base.Die();
    }

    IEnumerator ShellOff()
    {
        Vector3 starting = shell.localPosition;
        Vector3 end = shell.localPosition + Vector3.up * 30;

        float currentTime = 0;
        float percent = 0;
        float interval = FindObjectOfType<Syncher>()._interval * 8;
        while (percent < 1)
        {
            shell.localPosition = Vector3.Lerp(starting, end, percent);

            currentTime += Time.deltaTime;
            percent = currentTime / interval;

            yield return null;
        }
    }
}
