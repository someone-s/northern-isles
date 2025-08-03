using UnityEngine;

public class PortWaypoint : MonoBehaviour
{
    public Vector3 GetLocation() => transform.position;

    public Port Port { get; private set; }

    private void Awake()
    {
        Port = GetComponentInParent<Port>();
    }
    
}
