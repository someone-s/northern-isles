using UnityEngine;

public class VesselClick : MonoBehaviour
{
    public Vessel Vessel { get; private set; }
    private void Awake()
    {
        Vessel = GetComponentInParent<Vessel>();
    }

    private void Start()
    {
        StatusDisplay.Instance.AddVessel(Vessel);
    }

    public void OnClick()
    {
        StatusDisplay.Instance.FocusVessel(Vessel);
    }
}
