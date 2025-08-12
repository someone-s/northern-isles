using System;
using UnityEngine;
using UnityEngine.Events;

public class GameStopWatch : MonoBehaviour
{
    public float ElapsedS { get; private set; } = 0f;

    public UnityEvent<float> OnElapseChange;

    private void Awake()
    {
        OnElapseChange ??= new();
    }

    private void Update()
    {
        ElapsedS += Time.deltaTime * SpeedControl.Instance.TimeScale;
        OnElapseChange.Invoke(ElapsedS);
    }

    public void Reset()
    {
        ElapsedS = 0f;   
    }
}
