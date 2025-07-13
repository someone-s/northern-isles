using UnityEngine;

public class Port : MonoBehaviour
{
    public PortWayPoint WayPoint { get; private set; }

    public PortWarehouse Warehouse { get; private set; }

    private void Awake()
    {
        WayPoint = GetComponentInChildren<PortWayPoint>();
        Warehouse = GetComponentInChildren<PortWarehouse>();
    }
}