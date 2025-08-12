using UnityEngine;
using UnityEngine.UI;

public class SpeedVisual : MonoBehaviour
{
    [SerializeField] private Image pauseTint;
    [SerializeField] private Image playTint;
    [SerializeField] private Image fastForwardTint;
    [SerializeField] private float blinkIntervalS;
    [SerializeField] private Gradient gradient;

    private int mode = 0;
    private float elapsedS = 0f;

    private void Awake()
    {

        SpeedControl.Instance.OnPause.AddListener(OnPausePressed);
        SpeedControl.Instance.OnPlay.AddListener(OnPlayPressed);
        SpeedControl.Instance.OnFastForward.AddListener(OnFastForwardPressed);
    }

    public void OnPausePressed()
    {
        elapsedS = 0f;
        mode = 0;
        playTint.color = Color.clear;
        fastForwardTint.color = Color.clear;
    }

    public void OnPlayPressed()
    {
        elapsedS = 0f;
        mode = 1;
        pauseTint.color = Color.clear;
        fastForwardTint.color = Color.clear;
    }

    public void OnFastForwardPressed()
    {
        elapsedS = 0f;
        mode = 2;
        pauseTint.color = Color.clear;
        playTint.color = Color.clear;
    }

    private void Update()
    {
        elapsedS += Time.deltaTime;
        elapsedS %= blinkIntervalS;
        switch (mode)
        {
            case 0:
                pauseTint.color = gradient.Evaluate(elapsedS / blinkIntervalS);
                break;
            case 1:
                playTint.color = gradient.Evaluate(elapsedS / blinkIntervalS);
                break;
            case 2:
                fastForwardTint.color = gradient.Evaluate(elapsedS / blinkIntervalS);
                break;
        }
    }
}