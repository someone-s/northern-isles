using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatusDisplay : MonoBehaviour
{
    public static StatusDisplay Instance;

    private ScrollFocus focus;
    private Dictionary<Vessel, RectTransform> vesselRects; 

    [SerializeField] private GameObject displayPrefab;
    [SerializeField] private RectTransform content;

    public UnityEvent<Vessel, StatusData> OnAddedVessel;

    private StatusDisplay()
    {
        Instance = this;
    }

    private void Awake()
    {
        focus = GetComponent<ScrollFocus>();
        vesselRects = new();
    }

    public void AddVessel(Vessel vessel)
    {
        GameObject newUI = Instantiate(displayPrefab, content);
        newUI.transform.SetAsLastSibling();

        var data = newUI.GetComponent<StatusData>();
        data.SetDisplay(this);
        data.SetVessel(vessel);

        vesselRects.Add(vessel, newUI.transform as RectTransform);

        OnAddedVessel.Invoke(vessel, data);
    }

    public void RemoveVessel()
    {
        
    }

    public void FocusVessel(Vessel vessel)
    {
        if (vesselRects.TryGetValue(vessel, out RectTransform rect))
        {
            focus.GoTo(rect);
            var expand = rect.GetComponent<ExpandablePanel>();
            if (expand != null)
                expand.SetState(true);
        }
    }

    public void LoadRoute(Vessel vessel)
    {
        RouteDisplay.Instance.LoadVessel(vessel);
        gameObject.SetActive(false);
    }
}
