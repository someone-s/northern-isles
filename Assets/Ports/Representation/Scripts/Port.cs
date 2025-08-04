using Newtonsoft.Json.Linq;
using UnityEngine;

public class Port : MonoBehaviour
{
    public PortWaypoint WayPoint { get; private set; }
    public PortStorage Storage { get; private set; }
    public PortVisual Visual { get; private set; }
    public PortCargoDisplay Display { get; private set; }
    public PortAnchor Anchor { get; private set; }

    public string Name => gameObject.name;

    private void Awake()
    {
        WayPoint = gameObject.GetComponentInChildren<PortWaypoint>();
        Storage = gameObject.GetComponentInChildren<PortStorage>();
        Visual = gameObject.GetComponentInChildren<PortVisual>();
        Display = gameObject.GetComponentInChildren<PortCargoDisplay>();
        Anchor = gameObject.GetComponentInChildren<PortAnchor>();

        PortDatabase.Instance.AddPort(this);
    }

    public void OnPortPressed()
    {
        PortEvent.Instance.PortPressed(this);
    }

    public JToken GetState()
    {
        return Storage.GetState();
    }

    public void SetState(JToken json)
    {
        Storage.SetState(json);
    }

    public void Rollback()
    {
        Storage.Rollback();
    }
}