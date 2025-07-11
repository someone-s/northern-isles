using UnityEngine;

public class PortWayPoint : MonoBehaviour, IWayPoint
{
    public Vector3 GetLocation() => transform.position;
}
