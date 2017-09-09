using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syncher : MonoBehaviour
{
    public int tempo;
    public float _interval { get; private set; }

    public System.Action OnBar;
    public System.Action OnQuarter;
    public System.Action OnEigth;

    private int[] _bar;
    private float _time;
    private float nextEigth;

    void Awake()
    {
        _interval = 60f / (float)(tempo * 2);
        _time = Time.time;
        nextEigth = Time.time + _interval;
        _bar = new int[3] { 1, 1, 1 };
    }

    void Update()
    {
        _time = Time.time;
        if (_time >= nextEigth)
        {
            nextEigth = Time.time + _interval;

            if (OnEigth != null) OnEigth();
            _bar[2]++;
            if (_bar[2] == 3)
            {
                _bar[2] = 1;
                if (OnQuarter != null) OnQuarter();
                _bar[1]++;
                if(_bar[1] == 5)
                {
                    _bar[1] = 1;
                    if (OnBar != null) OnBar();
                    _bar[0]++;
                }
            }
        }
    }
}
