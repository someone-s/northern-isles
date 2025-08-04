using UnityEngine;
using UnityEngine.Events;

public class NarrativeRouteEdited : MonoBehaviour
{
    [SerializeField] private Vessel vessel;

    public UnityEvent OnComplete;

    private void OnEnable()
    {
        RouteDisplay.Instance.OnVesselSelected.AddListener(Evaluate);
    }

    private void OnDisable()
    {
        RouteDisplay.Instance.OnVesselSelected.RemoveListener(Evaluate);
    }

    public void SetVessel(Vessel vessel)
    {
        this.vessel = vessel;
    }

    private void Evaluate(Vessel selectedVessel)
    {
        if (selectedVessel == vessel)
            OnComplete.Invoke();
    }
}
