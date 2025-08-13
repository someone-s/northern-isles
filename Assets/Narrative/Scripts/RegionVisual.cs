using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RegionVisual : MonoBehaviour
{
    public static RegionVisual Instance { get; private set; }

    private RegionVisual()
    {
        Instance = this;
    }

    [SerializeField] private Button enterButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject regionCanvas;

    public UnityEvent OnRegionShown;
    public UnityEvent OnRegionHidden;

    private void Awake()
    {
        enterButton.onClick.AddListener(Enter);
        exitButton.onClick.AddListener(Exit);

        regionCanvas.SetActive(false);
        enterButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }

    public void ShowEnterButton()
    {
        enterButton.gameObject.SetActive(true);
    }

    private void Enter()
    {
        enterButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(true);

        OnRegionShown.Invoke();
        regionCanvas.SetActive(true);
    }

    public void Exit()
    {
        regionCanvas.SetActive(false);
        OnRegionHidden.Invoke();

        enterButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(false);
    }
}
