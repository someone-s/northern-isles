using UnityEngine;
using UnityEngine.Events;

public class VesselSpawnSetting : MonoBehaviour
{
    public string type;
    public Transform orientation;

    public UnityEvent<Vessel> OnVesselSpawn;
}