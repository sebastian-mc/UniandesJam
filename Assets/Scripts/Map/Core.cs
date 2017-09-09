using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Core : Entity
{
    public Transform shell;
    public Image healthBar;

    public GameObject endScreen;
    public Text endText;

    private int crystals = 0;

    private enum CoreState
    {
        ShellOn,
        ShellOff,
        Won,
        Lost
    }
    private CoreState coreState = CoreState.ShellOn;

    protected override void Start()
    {
        base.Start();
        FindObjectOfType<Player>().OnDeath += OnCharDeath;
        healthBar.gameObject.SetActive(false);
        Crystal[] cry = FindObjectsOfType<Crystal>();
        for (int i = 0; i < cry.Length; i++)
        {
            crystals++;
            cry[i].OnDeath += CrystalDestroyed;
        }

        endScreen.gameObject.SetActive(false);
    }

    void CrystalDestroyed()
    {
        crystals--;
        if (crystals == 0)
        {
            healthBar.gameObject.SetActive(true);
            coreState = CoreState.ShellOff;
            StartCoroutine(ShellOff());
        }
    }

    public override void TakeDamage()
    {
        if (coreState == CoreState.ShellOn) return;
        base.TakeDamage();
    }

    public override void Die()
    {
        coreState = CoreState.Won;
        endScreen.gameObject.SetActive(true);

        base.Die();
    }

    void OnCharDeath()
    {
        coreState = CoreState.Lost;
        endScreen.gameObject.SetActive(true);
        endText.text = "Wasted!";
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
