using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour
{
    public static List<Port> Ports { get; private set; }
    public PortWaypoint WayPoint { get; private set; }
    public PortStorage Storage { get; private set; }
    public PortVisual Visual { get; private set; }

    private void Awake()
    {
        WayPoint = gameObject.GetComponentInChildren<PortWaypoint>();
        Storage = gameObject.GetComponentInChildren<PortStorage>();
        Visual = gameObject.GetComponentInChildren<PortVisual>();

        Ports ??= new();
        Ports.Add(this);
    }


    public void OnPortPressed()
    {
        PortEvent.Instance.PortPressed(this);
    }

}