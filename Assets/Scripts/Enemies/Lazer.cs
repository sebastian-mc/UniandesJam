using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : Entity
{
    public Transform lazer1;
    public Transform lazer2;
    public Transform model;
    public bool moving;
    public float moveAmount;

    private BoxCollider boxCollider;
    private LineRenderer beam;
    private Material beamMaterial;
    private bool shooting;
    private Vector3 startingPos1 = new Vector3(0, -2, 24);
    private Vector3 startingPos2 = new Vector3(0, -2, -24);
    private Vector3 pos1 = new Vector3(0, 0, 26);
    private Vector3 pos2 = new Vector3(0, 0, -26);

    protected override void Start()
    {
        base.Start();
        _state = State.Spawning;
        shooting = false;
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        beam = GetComponent<LineRenderer>();
        beam.endWidth = beam.startWidth = 0;
        beam.SetPosition(0, new Vector3(0, 5.2f, -24));
        beam.SetPosition(1, new Vector3(0, 5.2f, 24));
        beamMaterial = beam.material;
        FindObjectOfType<Syncher>().OnBar += Shoot;

        StartCoroutine(SpawningAnimation());
    }

    void Shoot()
    {
        if (_state == State.Active && !shooting)
        {
            shooting = true;
            StartCoroutine(ShootBeam());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            other.GetComponent<Entity>().TakeDamage();
        }
    }

    IEnumerator ShootBeam()
    {
        float interval = FindObjectOfType<Syncher>()._interval;
        beam.endWidth = beam.startWidth = 2;
        yield return new WaitForSeconds(interval * 2f);

        float currentTime = 0;
        float percent = 0;
        interval = interval * 14f;
        while (percent < 1)
        {
            boxCollider.enabled = true;
            beam.endWidth = beam.startWidth = Mathf.Lerp(2, 100, Mathf.Clamp(percent * 1.95f, 0, 1));
            beamMaterial.mainTextureOffset = Vector2.right * Mathf.Lerp(0, 20, percent);

            currentTime += Time.deltaTime;
            percent = currentTime / interval;

            yield return null;
        }
        beam.startWidth = beam.endWidth = 0;
        boxCollider.enabled = false;
        StartCoroutine(DespawningAnimation());
    }

    IEnumerator SpawningAnimation()
    {
        lazer1.localPosition = startingPos1;
        lazer1.localEulerAngles = Vector3.right * 180;
        lazer2.localPosition = startingPos2;
        lazer2.localEulerAngles = Vector3.right * -180;

        float currentTime = 0;
        float percent = 0;
        float interval = FindObjectOfType<Syncher>()._interval * 8;
        while (percent < 1)
        {
            lazer1.position = Vector3.Lerp(startingPos1, pos1, percent);
            lazer1.localEulerAngles = Vector3.right * Mathf.Lerp(180, 0, percent);
            lazer2.position = Vector3.Lerp(startingPos2, pos2, percent);
            lazer2.localEulerAngles = Vector3.right * Mathf.Lerp(-180, 0, percent);

            currentTime += Time.deltaTime;
            percent = currentTime / interval;

            yield return null;
        }
        lazer1.position = pos1;
        lazer1.localEulerAngles = Vector3.zero;
        lazer2.position = pos2;
        lazer2.localEulerAngles = Vector3.zero;
        _state = State.Active;
    }

    IEnumerator DespawningAnimation()
    {
        lazer1.localPosition = pos1;
        lazer1.localEulerAngles = Vector3.zero;
        lazer2.localPosition = pos2;
        lazer2.localEulerAngles = Vector3.zero;

        float currentTime = 0;
        float percent = 0;
        float interval = FindObjectOfType<Syncher>()._interval * 8;
        while (percent < 1)
        {
            lazer1.position = Vector3.Lerp(pos1, startingPos1, percent);
            lazer1.localEulerAngles = Vector3.right * Mathf.Lerp(0, 180, percent);
            lazer2.position = Vector3.Lerp(pos2, startingPos2, percent);
            lazer2.localEulerAngles = Vector3.right * Mathf.Lerp(0, -180, percent);

            currentTime += Time.deltaTime;
            percent = currentTime / interval;

            yield return null;
        }
        Destroy(gameObject);
    }
}
