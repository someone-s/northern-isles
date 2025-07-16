using UnityEngine;

public class StatusDisplay : MonoBehaviour
{
    public static StatusDisplay Instance;

    public StatusCargo Cargo { get; private set; }

    [SerializeField] private GameObject displayPrefab;
    [SerializeField] private RectTransform content;

    private StatusDisplay()
    {
        Instance = this;
    }

    private void Awake()
    {
        Cargo = GetComponent<StatusCargo>();
    }

    public void AddVessel(Vessel vessel)
    {
        GameObject newUI = Instantiate(displayPrefab, content);
        newUI.transform.SetAsLastSibling();

        var data = newUI.GetComponent<StatusData>();
        data.SetDisplay(this);
        data.SetVessel(vessel);
    }

    public void RemoveVessel()
    {

    }

    public void LoadRoute(Vessel vessel)
    {
        RouteDisplay.Instance.LoadVessel(vessel);
        gameObject.SetActive(false);
    }
}
