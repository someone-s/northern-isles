using UnityEngine;
using UnityEngine.Events;

public class SpeedControl : MonoBehaviour
{
    public static SpeedControl Instance { get; private set; }

    public float TimeScale { get; private set; } = 1f;

    public UnityEvent<float> OnSpeedChange = new();
    public UnityEvent OnPause = new();
    public UnityEvent OnPlay = new();
    public UnityEvent OnFastForward = new();

    private SpeedControl()
    {
        Instance = this;
    }

    private void Start()
    {
        Pause();
    }

    public void Pause()
    {
        TimeScale = 0f;
        OnPause.Invoke();
        OnSpeedChange.Invoke(TimeScale);
    }

    public void Play()
    {
        TimeScale = 1f;
        OnPlay.Invoke();
        OnSpeedChange.Invoke(TimeScale);
    }

    public void FastForward()
    {
        TimeScale = 2f;
        OnFastForward.Invoke();
        OnSpeedChange.Invoke(TimeScale);
    }
}
