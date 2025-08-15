using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RegionVisual : MonoBehaviour, IStateProvider
{
    public static RegionVisual Instance { get; private set; }

    private RegionVisual()
    {
        Instance = this;
    }

    private bool allowed = false;

    [SerializeField] private Button enterButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject regionCanvas;
    private CanvasGroup canvasGroup;

    public UnityEvent OnRegionShown;
    public UnityEvent OnRegionHidden;

    private void Awake()
    {
        canvasGroup = regionCanvas.GetComponent<CanvasGroup>();

        enterButton.onClick.AddListener(Enter);
        exitButton.onClick.AddListener(Exit);

        regionCanvas.SetActive(false);
        enterButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);

        StateTrack.Instance.AddProvider(this);
    }

    public void ShowEnterButton()
    {
        allowed = true;
        enterButton.gameObject.SetActive(true);
    }

    public void AllowInput()
    {
        canvasGroup.interactable = true;
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

    private JToken cachedState;

    public string GetName() => "RegionVisual";

    public int GetPriority() => 100;

    private struct VisualState
    {
        public bool allowed;
        public bool interactable;
    }


    public JToken GetState()
    {
        cachedState = JToken.FromObject(new VisualState()
        {
            allowed = allowed,
            interactable = canvasGroup.interactable
        });

        return cachedState;
    }

    public void SetState(JToken json)
    {
        cachedState = json;

        var state = cachedState.ToObject<VisualState>();
        allowed = state.allowed;
        Exit();
        enterButton.gameObject.SetActive(allowed);

        canvasGroup.interactable = state.interactable;
    }

    public void Rollback()
    {
        SetState(cachedState);
    }
}
