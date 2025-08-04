using UnityEngine;

public class VesselClick : MonoBehaviour
{
    public Vessel Vessel { get; private set; }
    private void Awake()
    {
        Vessel = GetComponentInParent<Vessel>();
    }

    public void OnClick()
    {
        if (enabled)
            RouteDisplay.Instance.LoadVessel(Vessel);
    }
}
