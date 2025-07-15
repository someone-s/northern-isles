using UnityEngine;

public class Port : MonoBehaviour
{
    public PortWaypoint WayPoint { get; private set; }

    public PortWarehouse Warehouse { get; private set; }

    private void Awake()
    {
        WayPoint = GetComponentInChildren<PortWaypoint>();
        Warehouse = GetComponentInChildren<PortWarehouse>();
    }
}