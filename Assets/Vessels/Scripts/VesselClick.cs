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
        var display = FindAnyObjectByType<StatusDisplay>(FindObjectsInactive.Include);
        display.AddVessel(Vessel);
    }

}
