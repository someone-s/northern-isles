using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour
{
    public static List<Port> Ports { get; private set; }

    public PortWaypoint WayPoint { get; private set; }

    public PortWarehouse Warehouse { get; private set; }

    public PortVisual Visual { get; private set; }

    private void Awake()
    {
        WayPoint = GetComponentInChildren<PortWaypoint>();
        Warehouse = GetComponentInChildren<PortWarehouse>();
        Visual = GetComponentInChildren<PortVisual>();

        if (Ports == null)
            Ports = new();
        Ports.Add(this);
    }


    public void OnPortPressed()
    {
        PortEvent.Instance.PortPressed(this);
    }

}