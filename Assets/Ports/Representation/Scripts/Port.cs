using UnityEngine;

public class Port : MonoBehaviour
{
    public PortWaypoint WayPoint { get; private set; }
    public PortStorage Storage { get; private set; }
    public PortVisual Visual { get; private set; }
    public PortCargoDisplay Display { get; private set; }
    public PortAnchor Anchor { get; private set; }

    public string Name => Visual.Name;

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

    public string GetState()
    {
        return Storage.GetState();
    }

    public void SetState(string json)
    {
        Storage.SetState(json);
    }

    public void Rollback()
    {
        Storage.Rollback();
    }
}