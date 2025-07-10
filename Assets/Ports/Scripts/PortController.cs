using UnityEngine;

public class PortController : MonoBehaviour, IWayPoint
{

    public Vector3 GetLocation() => transform.position;
}
