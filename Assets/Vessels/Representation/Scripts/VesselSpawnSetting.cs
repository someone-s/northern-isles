using UnityEngine;
using UnityEngine.Events;

public class VesselSpawnSetting : MonoBehaviour
{
    public string type;
    public string vesselName;
    public Transform orientation;

    public UnityEvent<Vessel> OnVesselSpawn;
}