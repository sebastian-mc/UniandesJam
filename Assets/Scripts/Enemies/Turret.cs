using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Entity
{
    public Transform turretHead;
    public Transform turretHeadModel;
    public Transform muzzle;
    public GameObject projectile;
    public GameObject deathFx;
    public bool bandTurret;
    public Transform band;
    public float probability;

    private Transform _player;

    private string pattern;
    private int iPattern;
    private int iPosition;
    private int[] positions;

    protected override void Start()
    {
        base.Start();
        Player p = FindObjectOfType<Player>();
        p.OnDeath += OnCharacterDeath;
        _player = p.transform;
        _state = State.Spawning;
        FindObjectOfType<Syncher>().OnEigth += Shoot;
        if (bandTurret)
        {
            FindObjectOfType<Syncher>().OnBar += MoveBand;
        }
        CreatePattern();
        StartCoroutine(SpawningAnimation());
    }

    void CreatePattern()
    {
        pattern = "";
        for (int i = 0; i < 8; i++)
        {
            float chance = Random.value;
            if (chance <= probability)
            {
                pattern += "x";
            }
            else
            {
                pattern += "-";
            }
        }
        iPattern = 0;
        iPosition = 0;
        positions = new int[4] { 0, -5, 0, 5 };
        if (pattern == "--------")
        {
            CreatePattern();
        }
    }

    void FixedUpdate()
    {
        if (_state != State.Active) return;
        turretHead.LookAt(_player);
        turretHead.eulerAngles = Vector3.Scale(turretHead.eulerAngles, Vector3.up);
    }

    void Shoot()
    {
        if (_state != State.Active) return;
        if (pattern[iPattern] == 'x')
        {
            GameObject newProjectile = Instantiate(projectile);
            newProjectile.transform.position = muzzle.transform.position;
            newProjectile.transform.eulerAngles = muzzle.transform.eulerAngles;
            StartCoroutine(RecoilAnimation());
        }
        iPattern++;
        if (iPattern == 8)
        {
            iPattern = 0;
        }
    }

    void MoveBand()
    {
        if (_state != State.Active) return;
        StartCoroutine(MoveBandAnimation());
    }

    void OnCharacterDeath()
    {
        _state = State.Inactive;
    }

    IEnumerator SpawningAnimation()
    {
        turretHead.localEulerAngles = 90 * Vector3.right;
        transform.position = transform.position - (Vector3.up * 4);
        float currentTime = 0;
        float percent = 0;
        float interval = FindObjectOfType<Syncher>()._interval * 4;
        while (percent < 1)
        {
            turretHead.localEulerAngles = Vector3.right * Mathf.Lerp(90, 0, percent);
            transform.position = Vector3.Scale(transform.position, Vector3.one - Vector3.up) + Vector3.up * Mathf.Lerp(-4, 0, percent);
            currentTime += Time.deltaTime;
            percent = currentTime / interval;

            yield return null;
        }
        turretHead.localEulerAngles = Vector3.zero;
        transform.position = Vector3.Scale(transform.position, Vector3.one - Vector3.up);
        _state = State.Active;
    }

    IEnumerator RecoilAnimation()
    {
        turretHeadModel.localPosition = Vector3.forward * -0.4f;

        float currentTime = 0;
        float percent = 0;
        float interval = FindObjectOfType<Syncher>()._interval / 2f;
        while (percent < 1)
        {
            turretHeadModel.localPosition = Vector3.forward * Mathf.Lerp(-0.3f, 0, percent);

            currentTime += Time.deltaTime;
            percent = currentTime / interval;

            yield return null;
        }
        turretHeadModel.localPosition = Vector3.zero;
    }

    IEnumerator MoveBandAnimation()
    {
        int current = positions[iPosition];
        iPosition++;
        if (iPosition == 4) iPosition = 0;
        int next = positions[iPosition];

        float currentTime = 0;
        float percent = 0;
        float interval = FindObjectOfType<Syncher>()._interval * 8;
        while (percent < 1)
        {
            band.localPosition = Vector3.right * Mathf.Lerp(current, next, percent);

            currentTime += Time.deltaTime;
            percent = currentTime / interval;

            yield return null;
        }

        band.localPosition = Vector3.right * next;
    }

    public override void Die()
    {
        GameObject death = Instantiate(deathFx);
        death.transform.position = transform.position;
        FindObjectOfType<Syncher>().OnEigth -= Shoot;
        if (bandTurret)
        {
            FindObjectOfType<Syncher>().OnBar -= MoveBand;
        }
        Destroy(death, 5f);
        Destroy(gameObject);
    }
}
