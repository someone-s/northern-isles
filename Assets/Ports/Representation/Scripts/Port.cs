using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Port : MonoBehaviour
{
    public static List<Port> Ports { get; private set; }

    [field: SerializeField]
    public PortWaypoint WayPoint { get; private set; }

    [field: SerializeField]
    public PortWarehouse InboundWarehouse { get; private set; }

    [field: SerializeField]
    public PortWarehouse OutboundWarehouse { get; private set; }

    [field: SerializeField]
    public PortVisual Visual { get; private set; }

    private void Awake()
    {

        if (Ports == null)
            Ports = new();
        Ports.Add(this);
    }


    public void OnPortPressed()
    {
        PortEvent.Instance.PortPressed(this);
    }

}