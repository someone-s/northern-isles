using UnityEngine;

public class Vessel : MonoBehaviour
{
    public VesselStorage Storage { get; private set; }
    public VesselNavigation Navigation { get; private set; }

    private void Awake()
    {
        Storage = gameObject.GetComponentInChildren<VesselStorage>();
        Navigation = gameObject.GetComponentInChildren<VesselNavigation>();
    }

}
