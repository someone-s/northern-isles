using UnityEngine;
using UnityEngine.UI;

public class GameStopWatchVisual : MonoBehaviour
{
    public static GameStopWatchVisual Instance;

    private GameStopWatchVisual()
    {
        Instance = this;
    }

    private void Awake()
    {
        gameObject.SetActive(false);
        StateTrack.Instance.OnBeginLoadState.AddListener(Reload);
        StateTrack.Instance.OnBeginRollback.AddListener(Reload);
    }

    private void Reload()
    {
        gameObject.SetActive(false);
    }

    [SerializeField] private Image image;

    public void SetProgress(float normalizedProgress)
    {
        image.fillAmount = normalizedProgress;
    }
}